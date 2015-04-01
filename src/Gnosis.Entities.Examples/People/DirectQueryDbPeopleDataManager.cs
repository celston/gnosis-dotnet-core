using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.Entities.Examples.People
{
    public class DirectQueryDbPeopleDataManager : DirectQueryDbEntityDataManager, IPeopleDataManager
    {
        #region Constructors

        public DirectQueryDbPeopleDataManager(ConnectionStringSettings connectionStringSettings)
            : base(connectionStringSettings)
        {
        }

        public DirectQueryDbPeopleDataManager(ConnectionStringSettings connectionStringSettings, string prefix)
            : base(connectionStringSettings, prefix)
        {
        }

        #endregion

        #region Public Methods

        public void AddChild(Guid motherId, Guid motherRevision, Guid fatherId, Guid fatherRevision, IPersonCreateRequest childCreateRequest, Guid childRevision, string childLabel, IEnumerable<EntityFieldValue> childFieldValues)
        {
            using (DbConnection conn = GetConnection())
            {
                using (DbTransaction trans = conn.BeginTransaction())
                {
                    List<EntityFieldValue> emptyFieldValues = new List<EntityFieldValue>();
                    CopyFields(conn, trans, motherId, motherRevision, emptyFieldValues);
                    CopyFields(conn, trans, fatherId, fatherRevision, emptyFieldValues);

                    UpdateEntity(conn, trans, motherId, motherRevision, null, childCreateRequest.Created);
                    InsertEntityRevision(conn, trans, motherId, motherRevision, childCreateRequest.Author, null, childCreateRequest.Created);
                    UpdateEntity(conn, trans, fatherId, fatherRevision, null, childCreateRequest.Created);
                    InsertEntityRevision(conn, trans, fatherId, fatherRevision, childCreateRequest.Author, null, childCreateRequest.Created);

                    CreateEntityHelper(conn, trans, PeopleManager.TYPE_PERSON, childCreateRequest.Id, childRevision, childCreateRequest.Author, childLabel, childCreateRequest.Created, false, childFieldValues);
                }
            }
        }

        #endregion

        
    }
}
