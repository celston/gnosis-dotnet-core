using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;

using Gnosis.Data;

namespace Gnosis.Testing.Data
{
    public abstract class TestingDbDataManager : DbDataManager
    {
        #region Protected Properties

        protected abstract IEnumerable<string> ResetTablesList { get; }

        #endregion

        #region Constructors

        public TestingDbDataManager(ConnectionStringSettings connectionStringSettings)
            : base(connectionStringSettings)
        {
        }

        public TestingDbDataManager(ConnectionStringSettings connectionStringSettings, string prefix)
            : base(connectionStringSettings, prefix)
        {
        }

        #endregion

        #region Public Methods

        public void ResetTables()
        {
            using (DbConnection conn = GetConnection())
            {
                using (DbTransaction trans = conn.BeginTransaction())
                {
                    foreach (string table in ResetTablesList)
                    {
                        using (DbCommand cmd = CreateDeleteAllCommand(conn, trans, table))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                    trans.Commit();
                }
            }
            
        }

        #endregion

        #region Protected Methods

        protected int GetTableRowCount(string table)
        {
            using (DbConnection conn = GetConnection())
            {
                using (DbTransaction trans = conn.BeginTransaction())
                {
                    string sql = new SelectCountQueryBuilder().AddTable(prefix, table).ToString();

                    using (DbCommand cmd = CreateTextCommand(conn, trans, sql))
                    {
                        return (int)cmd.ExecuteScalar();
                    }
                }
            }
        }

        #endregion
    }
}
