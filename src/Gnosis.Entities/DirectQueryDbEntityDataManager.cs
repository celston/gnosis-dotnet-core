using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gnosis.Data;
using Gnosis.Entities.Exceptions;

namespace Gnosis.Entities
{
    public class DirectQueryDbEntityDataManager : DbDataManager, IEntityDataManager
    {
        private InitializeEntityDelegate initializeEntityDelegate;
        
        #region Constructors

        public DirectQueryDbEntityDataManager(ConnectionStringSettings connectionStringSettings)
            : base(connectionStringSettings)
        {
        }

        public DirectQueryDbEntityDataManager(ConnectionStringSettings connectionStringSettings, string prefix)
            : base(connectionStringSettings, prefix)
        {
        }

        #endregion

        #region Public Methods

        public bool EntityExists(string type, Guid id)
        {
            bool result = false;

            using (DbConnection conn = GetConnection())
            {
                using (DbTransaction trans = conn.BeginTransaction())
                {
                    string sql = new SelectCountQueryBuilder()
                    .AddTable(prefix, "Entity")
                    .AddWhere("Entity.type = @type")
                    .AddWhere("Entity.status = 1")
                    .AddWhere("Entity.id = @id")
                    .ToString();

                    using (DbCommand cmd = CreateTextCommand(conn, trans, sql))
                    {
                        AddParameter(cmd, "@type", type);
                        AddParameter(cmd, "@id", id);

                        int count = (int)cmd.ExecuteScalar();
                        result = count > 0;
                    }
                }
            }

            return result;
        }

        public void CreateEntity(string type, Guid id, Guid revision, Guid? author, string label, DateTime created, bool isProtected, IEnumerable<EntityFieldValue> fieldValues)
        {
            using (DbConnection conn = GetConnection())
            {
                using (DbTransaction trans = conn.BeginTransaction())
                {
                    CreateEntityHelper(conn, trans, type, id, revision, author, label, created, isProtected, fieldValues);

                    trans.Commit();
                }
            }
        }

        public void UpdateEntity(Guid id, Guid revision, Guid? author, string label, DateTime updated, IEnumerable<EntityFieldValue> fieldValues)
        {
            using (DbConnection conn = GetConnection())
            {
                using (DbTransaction trans = conn.BeginTransaction())
                {
                    CopyFields(conn, trans, id, revision, fieldValues);
                    
                    UpdateEntity(conn, trans, id, revision, label, updated);
                    InsertEntityRevision(conn, trans, id, revision, author, label, updated);

                    SaveFields(conn, trans, id, revision, fieldValues);

                    trans.Commit();
                }
            }
        }

        public T LoadEntity<T>(string type, Guid id, IEnumerable<EntityField> fields) where T : IEntity
        {
            T result = Activator.CreateInstance<T>();
            
            using (DbConnection conn = GetConnection())
            {
                using (DbTransaction trans = conn.BeginTransaction())
                {
                    Guid revision;
                    Guid? author = null;
                    DateTime created;
                    DateTime updated;
                    
                    using (DbCommand cmd = SelectEntity(conn, trans, id, type))
                    {
                        using (DbDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                revision = (Guid)dr["revision"];
                                if (dr["author"] != DBNull.Value)
                                {
                                    author = (Guid)dr["author"];
                                }
                                created = (DateTime)dr["created"];
                                updated = (DateTime)dr["updated"];
                            }
                            else
                            {
                                throw new EntityNotFoundException(id);
                            }
                        }
                    }

                    result.GrantInitializeEntityDelegate(this);
                    initializeEntityDelegate(id, revision, author, created, updated, LoadEntityFieldValues(conn, trans, revision, fields));
                }
            }

            return result;
        }

        public IEnumerable<string> GetDistinctEntityTypes(IEnumerable<Guid> ids)
        {
            List<string> result = new List<string>();

            if (ids.Count() > 0)
            {
                using (DbConnection conn = GetConnection())
                {
                    using (DbTransaction trans = conn.BeginTransaction())
                    {
                        string sql = new SelectQueryBuilder()
                        .AddColumn("Entity", "type")
                        .AddTable(prefix, "Entity")
                        .AddInWhere("Entity", "id", ids.Count())
                        .ToString();

                        using (DbCommand cmd = CreateTextCommand(conn, trans, sql))
                        {
                            int i = 0;
                            foreach (Guid id in ids)
                            {
                                AddParameter(cmd, string.Format("@id{0}", i++), id);
                            }

                            using (DbDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    result.Add((string)dr["type"]);
                                }
                            }
                        }

                        trans.Commit();
                    }   
                }
            }

            return result;
        }

        public string GetEntityType(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> LoadEntities<T>(IEnumerable<Guid> ids, IEnumerable<EntityField> fields) where T : IEntity
        {
            throw new NotImplementedException();
        }

        public void AcceptInitializeDelegate(InitializeEntityDelegate d)
        {
            initializeEntityDelegate = d;
        }

        #endregion

        #region Protected Methods

        protected void CreateEntityHelper(DbConnection conn, DbTransaction trans, string type, Guid id, Guid revision, Guid? author, string label, DateTime created, bool isProtected, IEnumerable<EntityFieldValue> fieldValues)
        {
            InsertEntity(conn, trans, type, id, revision, author, label, created, isProtected);
            InsertEntityRevision(conn, trans, id, revision, author, label, created);

            SaveFields(conn, trans, id, revision, fieldValues);
        }

        protected void CopyFields(DbConnection conn, DbTransaction trans, Guid id, Guid revision, IEnumerable<EntityFieldValue> fieldValues)
        {
            string fieldNames = string.Join(", ", fieldValues.Select(x => string.Format("'{0}'", x.Field.Name)));
            foreach (string table in new string[] { "FieldBit", "FieldInteger", "FieldDecimal", "FieldText", "FieldDateTime" })
            {
                string selectSql = new SelectQueryBuilder()
                    .AddColumn("Entity", "id")
                    .AddDynamicColumn("@revision", "revision")
                    .AddColumn("Field", "fieldName")
                    .AddColumn("Field", "delta")
                    .AddColumn("Field", "value")
                    .AddTable(prefix, "Entity")
                    .AddJoin(prefix, table, "Field", string.Format("Entity.revision = Field.revision", table))
                    .AddWhere("Entity.id = @id")
                    .AddConditionalWhere(fieldNames.Count() > 0, string.Format("Field.fieldName NOT IN ({0})", fieldNames))
                    .ToString();

                string sql = string.Format("INSERT INTO {0}{1} (id, revision, fieldName, delta, value) {2}", prefix, table, selectSql);
                Debug.Print(sql);
                using (DbCommand cmd = CreateTextCommand(conn, trans, sql))
                {
                    AddParameter(cmd, "@id", id);
                    AddParameter(cmd, "@revision", revision);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected void UpdateEntity(DbConnection conn, DbTransaction trans, Guid id, Guid revision, string label, DateTime updated)
        {
            UpdateQueryBuilder qb = new UpdateQueryBuilder()
                .SetTable(prefix, "Entity")
                .AddSets(new string[] { "revision", "updated" })
                .AddWhere("id = @id");

            if (!string.IsNullOrWhiteSpace(label))
            {
                qb.AddSet("label");
            }

            using (DbCommand cmd = CreateTextCommand(conn, trans, qb.ToString()))
            {
                AddParameter(cmd, "@revision", revision);
                if (!string.IsNullOrWhiteSpace(label))
                {
                    AddParameter(cmd, "@label", label);
                }
                AddParameter(cmd, "@updated", updated);
                AddParameter(cmd, "@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        protected void InsertEntityRevision(DbConnection conn, DbTransaction trans, Guid id, Guid revision, Guid? author, string label, DateTime created)
        {
            string sql = new InsertQueryBuilder()
                    .SetTable(prefix, "EntityRevision")
                    .AddColumns(new string[] { "id", "revision", "author", "label", "created" })
                    .ToString();
            Debug.Print(sql);

            using (DbCommand cmd = CreateTextCommand(conn, trans, sql))
            {
                AddParameter(cmd, "@id", id);
                AddParameter(cmd, "@revision", revision);
                AddParameter(cmd, "@author", author);
                AddParameter(cmd, "@label", label);
                AddParameter(cmd, "@created", created);

                cmd.ExecuteNonQuery();
            }
        }

        protected DbCommand SelectEntity(DbConnection conn, DbTransaction trans, Guid id, string type)
        {
            string sql = new SelectQueryBuilder()
                .AddTable(prefix, "Entity")
                .AddAllTableColumns("Entity")
                .AddWhere("id = @id")
                .AddWhere("status = 1")
                .AddWhere("type = @type")
                .ToString();

            DbCommand cmd = CreateTextCommand(conn, trans, sql);
            AddParameter(cmd, "@id", id);
            AddParameter(cmd, "@type", type);

            return cmd;
        }

        protected IEnumerable<EntityFieldValue> LoadEntityFieldValues(DbConnection conn, DbTransaction trans, Guid revision, IEnumerable<EntityField> fields)
        {
            List<EntityFieldValue> result = new List<EntityFieldValue>();

            Dictionary<string, Dictionary<int, object>> temp = new Dictionary<string, Dictionary<int, object>>();
            
            IEnumerable<EntityField> textFields = fields.Where(x => x.IsString);
            if (textFields.Count() > 0)
            {
                string fieldNames = string.Join(", ", textFields.Select(x => string.Format("'{0}'", x.Name)));
                string sql = new SelectQueryBuilder()
                    .AddColumn("FieldText", "fieldName")
                    .AddColumn("FieldText", "delta")
                    .AddColumn("DataText", "data", "value")
                    .AddTable(prefix, "FieldText")
                    .AddJoin(prefix, "DataText", "FieldText.value = DataText.md5")
                    .AddWhere("FieldText.revision = @revision")
                    .AddWhere(string.Format("FieldText.fieldName IN ({0})", fieldNames))
                    .ToString();

                ProcessLoadEntityFields(conn, trans, sql, revision, temp);
            }

            IEnumerable<EntityField> dateTimeFields = fields.Where(x => x.IsDateTime);
            if (dateTimeFields.Count() > 0)
            {
                string fieldNames = string.Join(", ", dateTimeFields.Select(x => string.Format("'{0}'", x.Name)));
                string sql = new SelectQueryBuilder()
                    .AddColumn("FieldDateTime", "fieldName")
                    .AddColumn("FieldDateTime", "delta")
                    .AddColumn("FieldDateTime", "value")
                    .AddTable(prefix, "FieldDateTime")
                    .AddWhere("FieldDateTime.revision = @revision")
                    .AddWhere(string.Format("FieldDateTime.fieldName IN ({0})", fieldNames))
                    .ToString();

                ProcessLoadEntityFields(conn, trans, sql, revision, temp);
            }

            foreach (EntityField field in fields)
            {
                if (temp.ContainsKey(field.Name))
                {
                    object value;
                    if (field.IsList)
                    {
                        value = temp[field.Name].Values;
                    }
                    else
                    {
                        value = temp[field.Name][0];
                    }

                    result.Add(new EntityFieldValue(field, field.Property, value));
                }
            }

            return result;
        }

        protected void ProcessLoadEntityFields(DbConnection conn, DbTransaction trans, string sql, Guid revision, Dictionary<string, Dictionary<int, object>> temp)
        {
            using (DbCommand cmd = CreateTextCommand(conn, trans, sql))
            {
                AddParameter(cmd, "revision", revision);

                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string fieldName = (string)dr["fieldName"];
                        int delta = (int)dr["delta"];
                        object value = dr["value"];

                        if (!temp.ContainsKey(fieldName))
                        {
                            temp.Add(fieldName, new Dictionary<int, object>());
                        }
                        temp[fieldName].Add(delta, value);
                    }
                }
            }
        }

        protected void SaveFields(DbConnection conn, DbTransaction trans, Guid id, Guid revision, IEnumerable<EntityFieldValue> fieldValues)
        {
            using (MD5 md5 = MD5.Create())
            {
                foreach (EntityFieldValue efv in fieldValues)
                {
                    if (efv.Field.IsDateTime)
                    {
                        if (efv.Field.IsList)
                        {
                            throw new NotImplementedException();
                        }
                        else
                        {
                            InsertField(conn, trans, "FieldDateTime", id, revision, efv.Field.Name, efv.Value, DbType.DateTime);
                        }
                    }
                    else if (efv.Field.IsString)
                    {
                        if (efv.Field.IsList)
                        {
                            throw new NotImplementedException();
                        }
                        else
                        {
                            string value = (string)efv.Value;
                            byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(value));
                            Guid guid = new Guid(hash);

                            if (SelectCountDataText(conn, trans, guid) == 0)
                            {
                                InsertDataText(conn, trans, guid, value);
                            }
                            InsertField(conn, trans, "FieldText", id, revision, efv.Field.Name, guid, DbType.Guid);
                        }
                    }
                    else if (efv.Field.IsBoolean)
                    {
                        if (efv.Field.IsList)
                        {
                            throw new NotImplementedException();
                        }
                        else
                        {
                            InsertField(conn, trans, "FieldBit", id, revision, efv.Field.Name, efv.Value, DbType.Boolean);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

        protected int SelectCountDataText(DbConnection conn, DbTransaction trans, Guid md5)
        {
            string sql = new SelectCountQueryBuilder()
                .AddTable(prefix, "DataText")
                .AddWhere("md5 = @md5")
                .ToString();

            using (DbCommand cmd = CreateTextCommand(conn, trans, sql))
            {
                AddParameter(cmd, "@md5", md5);

                return (int)cmd.ExecuteScalar();
            }
        }

        protected void InsertDataText(DbConnection conn, DbTransaction trans, Guid md5, string data)
        {
            string sql = new InsertQueryBuilder()
                                        .SetTable(prefix, "DataText")
                                        .AddColumns(new string[] { "md5", "data" })
                                        .ToString();

            using (DbCommand cmd = CreateTextCommand(conn, trans, sql))
            {
                AddParameter(cmd, "@md5", md5);
                AddParameter(cmd, "@data", data);

                cmd.ExecuteNonQuery();
            }
        }

        protected void InsertField(DbConnection conn, DbTransaction trans, string table, Guid id, Guid revision, string fieldName, object value, DbType dbType)
        {
            string sql = new InsertQueryBuilder()
                                    .SetTable(prefix, table)
                                    .AddColumns(new string[] { "id", "revision", "fieldName", "delta", "value" })
                                    .ToString();

            using (DbCommand cmd = CreateTextCommand(conn, trans, sql))
            {
                AddParameter(cmd, "@id", id);
                AddParameter(cmd, "@revision", revision);
                AddParameter(cmd, "@fieldName", fieldName);
                AddParameter(cmd, "@delta", 0);
                AddParameter(cmd, dbType, "@value", value);

                cmd.ExecuteNonQuery();
            }
        }

        protected void AppendFieldValue(DbConnection conn, DbTransaction trans, string table, Guid revision, string fieldName, object value, DbType dbType)
        {
            string selectSql = new SelectQueryBuilder()
                .AddColumn("Entity", "id")
                .AddColumn("Entity", "revision")
                .AddDynamicColumn("@fieldName", "fieldName")
                .AddDynamicColumn("COUNT(Field.id)", "delta")
                .AddDynamicColumn("@value", "value")
                .AddTable(prefix, "Entity")
                .AddLeftJoin(prefix, table, "Field", "Entity.revision = Field.revision AND Field.fieldName = @fieldName")
                .AddWhere("Entity.revision = @revision")
                .AddGroupBy("Entity.id")
                .AddGroupBy("Entity.revision")
                .ToString();
            
            string sql = string.Format("INSERT INTO {0}{1} {2}", prefix, table, selectSql);
            Debug.Print(sql);

            using (DbCommand cmd = CreateTextCommand(conn, trans, sql))
            {
                AddParameter(cmd, "@revision", revision);
                AddParameter(cmd, "@fieldName", fieldName);
                AddParameter(cmd, dbType, "@value", value);

                cmd.ExecuteNonQuery();
            }
        }

        protected void InsertEntity(DbConnection conn, DbTransaction trans, string type, Guid id, Guid revision, Guid? author, string label, DateTime created, bool isProtected)
        {
            string sql = new InsertQueryBuilder()
                    .SetTable(prefix, "Entity")
                    .AddColumns(new string[] { "id", "revision", "type", "author", "label", "created", "updated", "protected" })
                    .ToString();

            using (DbCommand cmd = CreateTextCommand(conn, trans, sql))
            {
                AddParameter(cmd, "@id", id);
                AddParameter(cmd, "@revision", revision);
                AddParameter(cmd, "@type", type);
                AddParameter(cmd, "@author", author);
                AddParameter(cmd, "@label", label);
                AddParameter(cmd, "@created", created);
                AddParameter(cmd, "@updated", created);
                AddParameter(cmd, "@protected", isProtected);

                cmd.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
