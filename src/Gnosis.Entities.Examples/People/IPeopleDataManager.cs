using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.Entities.Examples.People
{
    public interface IPeopleDataManager : IEntityDataManager
    {
        void AddChild(Guid motherId, Guid motherRevision, Guid fatherId, Guid fatherRevision, IPersonCreateRequest childCreateRequest, Guid childRevision, string childLabel, IEnumerable<EntityFieldValue> childFieldValues);
    }
}
