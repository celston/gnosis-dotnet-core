using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using Gnosis.Entities;
using Gnosis.Entities.Examples.People;
using Gnosis.Entities.Testing.Data;


namespace Gnosis.UnitTests.Entities.People
{
    public class DirectQueryDbPeopleTests : PeopleTests
    {
        private EntityTestingDbDataManager entityTestingDbDataManager = new EntityTestingDbDataManager(ConfigurationManager.ConnectionStrings["entities"]);
        
        [SetUp]
        public void SetUp()
        {
            entityTestingDbDataManager.ResetTables();
        }

        #region Protected Overridden Methods

        protected override IPeopleDataManager GetDataManager()
        {
            return new DirectQueryDbPeopleDataManager(ConfigurationManager.ConnectionStrings["entities"]);
        }

        protected override void CreatePerson_MultipleWithSimilarNames_Asserts()
        {
            Assert.AreEqual(3, entityTestingDbDataManager.GetEntityCount());
            Assert.AreEqual(3, entityTestingDbDataManager.GetEntityRevisionCount());
            Assert.AreEqual(3, entityTestingDbDataManager.GetFieldDateTimeCount());
            Assert.AreEqual(4, entityTestingDbDataManager.GetDataTextCount());
            Assert.AreEqual(6, entityTestingDbDataManager.GetFieldTextCount());
        }

        protected override void CreatePerson_Myself_Asserts()
        {
            Assert.AreEqual(1, entityTestingDbDataManager.GetEntityCount());
            Assert.AreEqual(1, entityTestingDbDataManager.GetEntityRevisionCount());
            Assert.AreEqual(1, entityTestingDbDataManager.GetFieldDateTimeCount());
            Assert.AreEqual(2, entityTestingDbDataManager.GetDataTextCount());
            Assert.AreEqual(2, entityTestingDbDataManager.GetFieldTextCount());
        }

        protected override void CreatePerson_UpdateEmployee_UpdateSocialNetworkProfile_Asserts()
        {
            Assert.AreEqual(1, entityTestingDbDataManager.GetEntityCount(), "Entity count");
            Assert.AreEqual(3, entityTestingDbDataManager.GetEntityRevisionCount(), "EntityRevision count");
            Assert.AreEqual(3, entityTestingDbDataManager.GetFieldDateTimeCount(), "FieldDateTime count");
            Assert.AreEqual(4, entityTestingDbDataManager.GetDataTextCount(), "DataText count");
            Assert.AreEqual(9, entityTestingDbDataManager.GetFieldTextCount(), "FieldText count");
            Assert.AreEqual(1, entityTestingDbDataManager.GetFieldBitCount(), "FieldBit count");
        }

        #endregion
    }
}
