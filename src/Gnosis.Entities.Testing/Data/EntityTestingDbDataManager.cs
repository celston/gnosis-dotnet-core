using System;
using System.Collections.Generic;
using System.Data.Common;

using Gnosis.Testing.Data;
using System.Configuration;

namespace Gnosis.Entities.Testing.Data
{
    public class EntityTestingDbDataManager : TestingDbDataManager
    {
        #region Protected Override Properties

        protected override IEnumerable<string> ResetTablesList
        {
            get
            {
                return new string[] { "Entity", "EntityRevision", "FieldInteger", "FieldBit", "FieldDecimal", "FieldDateTime", "FieldUniqueIdentifier", "FieldText", "DataText", "DataVarbinary" };
            }
        }

        #endregion

        #region Constructors

        public EntityTestingDbDataManager(ConnectionStringSettings connectionStringSettings)
            : base(connectionStringSettings)
        {
        }

        public EntityTestingDbDataManager(ConnectionStringSettings connectionStringSettings, string prefix)
            : base(connectionStringSettings, prefix)
        {
        }

        #endregion

        #region Public Methods

        public int GetDataTextCount()
        {
            return GetTableRowCount("DataText");
        }

        public int GetFieldTextCount()
        {
            return GetTableRowCount("FieldText");
        }

        public int GetFieldDateTimeCount()
        {
            return GetTableRowCount("FieldDateTime");
        }

        public int GetEntityCount()
        {
            return GetTableRowCount("Entity");
        }

        public int GetEntityRevisionCount()
        {
            return GetTableRowCount("EntityRevision");
        }

        public object GetFieldBitCount()
        {
            return GetTableRowCount("FieldBit");
        }

        #endregion

        
    }
}
