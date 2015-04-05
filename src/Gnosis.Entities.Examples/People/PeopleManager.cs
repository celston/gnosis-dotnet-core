using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gnosis.Entities.Exceptions;

namespace Gnosis.Entities.Examples.People
{
    public class PeopleManager : EntityManager
    {
        public const string TYPE_PERSON = "Person";

        private IPeopleDataManager dataManager;
        
        public PeopleManager(IPeopleDataManager dataManager)
        {
            this.dataManager = dataManager;
            SetEntityDataManager(dataManager);
        }

        public bool PersonExists(Guid id)
        {
            return EntityExists(id);
        }

        public T LoadPerson<T>(Guid id) where T : IEntity, IPerson
        {
            return LoadEntity<T>(id, TYPE_PERSON);
        }

        public IEnumerable<string> GetDistinctEntityTypes(IEnumerable<Guid> ids)
        {
            return dataManager.GetDistinctEntityTypes(ids);
        }

        public Guid CreatePerson(IPersonCreateRequest request)
        {
            return CreateEntity(request, TYPE_PERSON, GetPersonLabel(request));
        }

        public Guid UpdateEmployee(IEmployeeUpdateRequest request)
        {
            return UpdateEntity(request, TYPE_PERSON);
        }

        public Guid UpdateSocialNetworkProfile(ISocialNetworkProfileUpdateRequest request)
        {
            return UpdateEntity(request, TYPE_PERSON);
        }

        public void AddChild(Guid motherId, Guid fatherId, IPersonCreateRequest childCreateRequest)
        {
            AssertEntityExists(motherId);
            AssertEntityExists(fatherId);
            AssertEntityDoesntExist(childCreateRequest.Id);
            
            Guid motherRevision = Guid.NewGuid();
            Guid fatherRevision = Guid.NewGuid();
            Guid childRevision = Guid.NewGuid();

            dataManager.AddChild(motherId, motherRevision, fatherId, fatherRevision, childCreateRequest, childRevision, GetPersonLabel(childCreateRequest), Utility.GetFieldValues(childCreateRequest, childCreateRequest.GetType()));
        }

        private string GetPersonLabel(IPerson person)
        {
            return string.Format("{0}, {1}", person.LastName, person.FirstName);
        }
    }
}
