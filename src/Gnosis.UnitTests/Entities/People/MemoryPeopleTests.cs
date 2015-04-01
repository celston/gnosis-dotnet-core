using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gnosis.Entities;
using Gnosis.Entities.Testing.Data;
using Gnosis.Entities.Examples.People;

namespace Gnosis.UnitTests.Entities.People
{
    public class MemoryPeopleTests : PeopleTests
    {
        public class MemoryPeopleDataManager : MemoryEntityDataManager, IPeopleDataManager
        {
            public void AddChild(Guid motherId, Guid motherRevision, Guid fatherId, Guid fatherRevision, IPersonCreateRequest childCreateRequest, Guid childRevision, string childLabel, IEnumerable<EntityFieldValue> childFieldValues)
            {
                throw new NotImplementedException();
            }
        }
        
        protected override IPeopleDataManager GetDataManager()
        {
            return new MemoryPeopleDataManager();
        }
    }
}
