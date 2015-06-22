using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Gnosis.Data
{
    public abstract class DbDataManager
    {
        #region Private Fields

        protected ConnectionStringSettings connectionStringSettings;
        protected string connectionString;
        protected DbProviderFactory factory;
        protected string prefix;

        #endregion

        #region Nested Classes

        protected abstract class SelectQueryBuilder<T>
            where T : SelectQueryBuilder<T>
        {
            protected string prefix;
            protected bool distinct = false;
            protected List<string> columns = new List<string>();
            protected List<string> tables = new List<string>();
            protected List<string> wheres = new List<string>();
            private List<string> groupBys = new List<string>();

            public SelectQueryBuilder(string prefix)
            {
                this.prefix = prefix;
            }

            public T AddJoin(string table, string clause)
            {
                return AddJoin(table, table, clause);
            }

            public T AddJoin(string table, string alias, string clause)
            {
                if (tables.Count == 0)
                {
                    throw new SelectQueryEmptyTablesOnJoinException(table);
                }
                
                this.tables.Add(string.Format("JOIN {0}{1} {2} ON {3}", prefix, table, alias, clause));

                return (T)this;
            }

            public T AddLeftJoin(string table, string alias, string clause)
            {
                if (tables.Count == 0)
                {
                    throw new SelectQueryEmptyTablesOnJoinException(table);
                }

                this.tables.Add(string.Format("LEFT JOIN {0}{1} {2} ON {3}", prefix, table, alias, clause));

                return (T)this;
            }

            public T SetTable(string table)
            {
                return SetTable(table, table);
            }

            public T SetTable(string table, string alias)
            {
                if (this.tables.Count > 0)
                {
                    throw new SelectQueryMultipleTablesException(table, tables.First());
                }
                
                this.tables.Add(string.Format("{0}{1} {2}", prefix, table, alias));

                return (T)this;
            }

            public T AddTables(IEnumerable<string> tables)
            {
                this.tables.AddRange(tables.Select(x => string.Format("{0}{1} {1}", prefix, x)));

                return (T)this;
            }

            public T AddInWhere(string table, string column, int count)
            {
                string p = string.Join(", ", Enumerable.Range(0, count).Select(x => string.Format("@{0}{1}", column, x)));

                return AddWhere(string.Format("{0}.{1} IN ({2})", table, column, p));
            }

            public T AddWhere(string where)
            {
                this.wheres.Add(where);

                return (T)this;
            }

            public T AddWheres(IEnumerable<string> wheres)
            {
                this.wheres.AddRange(wheres);

                return (T)this;
            }

            public T AddGroupBy(string groupBy)
            {
                groupBys.Add(groupBy);

                return (T)this;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT ");
                if (distinct)
                {
                    sb.Append("DISTINCT ");
                }
                sb.Append(string.Join(", ", columns));

                sb.Append(" FROM ");
                sb.Append(string.Join(" ", tables));

                if (wheres.Count() > 0)
                {
                    sb.Append(" WHERE ");
                    sb.Append(string.Join(" AND ", wheres));
                }

                if (groupBys.Count() > 0)
                {
                    sb.Append(" GROUP BY ");
                    sb.Append(string.Join(", ", groupBys));
                }

                return sb.ToString();
            }
        }

        protected class SelectCountQueryBuilder : SelectQueryBuilder<SelectCountQueryBuilder>
        {
            public SelectCountQueryBuilder(string prefix)
                : base(prefix)
            {
                columns.Add("COUNT(*)");
            } 
        }

        protected class SelectQueryBuilder : SelectQueryBuilder<SelectQueryBuilder>
        {
            public SelectQueryBuilder(string prefix)
                : base(prefix)
            {
            }
            
            public SelectQueryBuilder SetDistinct()
            {
                distinct = true;

                return this;
            }

            public SelectQueryBuilder AddAllTableColumns(string table)
            {
                columns.Add(string.Format("{0}.*", table));

                return this;
            }

            public SelectQueryBuilder AddDynamicColumn(string column, string alias)
            {
                columns.Add(string.Format("{0} {1}", column, alias));

                return this;
            }

            public SelectQueryBuilder AddColumn(string table, string columnName)
            {
                return AddColumn(table, columnName, columnName);
            }

            public SelectQueryBuilder AddColumn(string table, string columnName, string alias)
            {
                columns.Add(string.Format("{0}.{1} {2}", table, columnName, alias));

                return this;
            }

            public SelectQueryBuilder AddColumns(IEnumerable<string> columns)
            {
                this.columns.AddRange(columns);

                return this;
            }

            public SelectQueryBuilder AddConditionalWhere(bool condition, string where)
            {
                if (condition)
                {
                    wheres.Add(where);
                }
                
                return this;
            }
        }

        protected abstract class ChangeQueryBuilder<T>
            where T : ChangeQueryBuilder<T>
        {
            protected string table;

            public T SetTable(string prefix, string table)
            {
                this.table = string.Format("{0}{1}", prefix, table);

                return (T)this;
            }
        }

        protected class InsertQueryBuilder : ChangeQueryBuilder<InsertQueryBuilder>
        {
            private List<string> columns = new List<string>();

            public InsertQueryBuilder AddColumn(string column)
            {
                this.columns.Add(column);

                return this;
            }

            public InsertQueryBuilder AddColumns(IEnumerable<string> columns)
            {
                this.columns.AddRange(columns);

                return this;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("INSERT INTO ");
                sb.Append(table);
                sb.Append(" (");
                sb.Append(string.Join(", ", columns));
                sb.Append(") VALUES (");
                sb.Append(string.Join(", ", columns.Select(x => string.Format("@{0}", x))));
                sb.Append(")");

                return sb.ToString();
            }
        }

        protected class UpdateQueryBuilder : ChangeQueryBuilder<UpdateQueryBuilder>
        {
            protected List<string> sets = new List<string>();
            protected List<string> wheres = new List<string>();

            public UpdateQueryBuilder AddSet(string set)
            {
                this.sets.Add(set);

                return this;
            }

            public UpdateQueryBuilder AddSets(IEnumerable<string> sets)
            {
                this.sets.AddRange(sets);

                return this;
            }

            public UpdateQueryBuilder AddInWhere(string table, string column, int count)
            {
                string p = string.Join(", ", Enumerable.Range(0, count).Select(x => string.Format("@{0}{1}", column, x)));

                return AddWhere(string.Format("{0}.{1} IN ({2})", table, column, p));
            }

            public UpdateQueryBuilder AddWhere(string where)
            {
                this.wheres.Add(where);

                return this;
            }

            public UpdateQueryBuilder AddWheres(IEnumerable<string> wheres)
            {
                this.wheres.AddRange(wheres);

                return this;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("UPDATE ");
                sb.Append(table);
                
                sb.Append(" SET ");
                sb.Append(string.Join(", ", sets.Select(x => string.Format("{0} = @{0}", x))));

                if (wheres.Count() > 0)
                {
                    sb.Append(" WHERE ");
                    sb.Append(string.Join(" AND ", wheres));
                }

                return sb.ToString();
            }
        }

        #endregion
        
        #region Constructors

        public DbDataManager(string name) : this(ConfigurationManager.ConnectionStrings[name])
        {
        }

        public DbDataManager(ConnectionStringSettings connectionStringSettings)
        {
            this.connectionStringSettings = connectionStringSettings;
            factory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
        }

        public DbDataManager(ConnectionStringSettings connectionStringSettings, string prefix)
            : this(connectionStringSettings)
        {
            this.prefix = prefix;
        }

        #endregion

        #region Protected Methods

        protected DbConnection GetConnection()
        {
            DbConnection result = factory.CreateConnection();
            result.ConnectionString = connectionStringSettings.ConnectionString;
            result.Open();

            return result;
        }

        protected DbCommand CreateDeleteAllCommand(DbConnection conn, DbTransaction trans, string table)
        {
            return CreateTextCommand(conn, trans, "DELETE FROM {0}{1}", prefix, table);
        }

        protected void AddParameter(DbCommand cmd, string parameterName, Guid? value)
        {
            if (value.HasValue)
            {
                AddParameter(cmd, DbType.Guid, parameterName, value.Value);
            }
            else
            {
                AddParameter(cmd, DbType.Guid, parameterName, DBNull.Value);
            }
        }

        protected void AddParameter(DbCommand cmd, string parameterName, DateTime? value)
        {
            if (value.HasValue)
            {
                AddParameter(cmd, DbType.DateTime, parameterName, value.Value);
            }
            else
            {
                AddParameter(cmd, DbType.DateTime, parameterName, DBNull.Value);
            }
        }

        protected void AddParameter(DbCommand cmd, string parameterName, int? value)
        {
            if (value.HasValue)
            {
                AddParameter(cmd, DbType.Int64, parameterName, value.Value);
            }
            else
            {
                AddParameter(cmd, DbType.Int64, parameterName, DBNull.Value);
            }
        }

        protected void AddParameter(DbCommand cmd, string parameterName, string value)
        {
            if (value != null)
            {
                AddParameter(cmd, DbType.String, parameterName, value);
            }
            else
            {
                AddParameter(cmd, DbType.String, parameterName, DBNull.Value);
            }
        }

        protected void AddParameter(DbCommand cmd, string parameterName, bool? value)
        {
            if (value.HasValue)
            {
                AddParameter(cmd, DbType.Boolean, parameterName, value.Value);
            }
            else
            {
                AddParameter(cmd, DbType.Boolean, parameterName, DBNull.Value);
            }
        }

        protected void AddParameter(DbCommand cmd, DbType dbType, string parameterName, object value)
        {
            DbParameter p = cmd.CreateParameter();
            p.DbType = dbType;
            p.ParameterName = parameterName;
            p.Value = value;

            cmd.Parameters.Add(p);
        }

        protected DbCommand CreateCommand(DbConnection conn, DbTransaction trans, string sql)
        {
            return CreateCommand(conn, trans, prefix + sql, CommandType.StoredProcedure);
        }

        protected DbCommand CreateTextCommand(DbConnection conn, DbTransaction trans, string sql, params object[] args)
        {
            return CreateTextCommand(conn, trans, string.Format(sql, args));
        }

        protected DbCommand CreateTextCommand(DbConnection conn, DbTransaction trans, string sql)
        {
            return CreateCommand(conn, trans, sql, CommandType.Text);
        }

        protected DbCommand CreateCommand(DbConnection conn, DbTransaction trans, string sql, CommandType commandType)
        {
            DbCommand result = conn.CreateCommand();
            result.Transaction = trans;
            result.CommandText = sql;
            result.CommandType = commandType;

            return result;
        }

        #endregion
    }
}
