using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using Gnosis.Testing;
using Gnosis.Entities;
using Gnosis.Entities.Examples.People;
using Gnosis.Entities.Requests;

namespace Gnosis.UnitTests.Entities.People
{
    public abstract class PeopleTests : EntityTests
    {
        #region Protected Fields

        protected PeopleManager manager;

        protected IPeopleDataManager dataManager;

        #endregion

        #region Protected Override Methods

        protected override IEnumerable<Type> GetEntityTypes()
        {
            return new Type[] { typeof(Person), typeof(Employee) };
        }

        protected override int ExpectedFieldCount
        {
            get { return 4; }
        }

        #endregion

        #region Protected Abstract Methods

        protected abstract IPeopleDataManager GetDataManager();

        #endregion

        #region Nested Classes

        public class Person : Entity, IPerson
        {
            public string FirstName { get; protected set; }
            public string LastName { get; protected set; }
            public DateTime BirthDate { get; protected set; }
        }

        public class Employee : Person, IEmployee
        {
            public string Employer { get; private set; }
            public DateTime HireDate { get; private set; }
        }

        public class SocialNetworkProfile : Entity, ISocialNetworkProfile
        {
            public bool UsesTwitter { get; private set; }
            public string TwitterUsername { get; private set; }
        }

        public class PersonCreateRequest : EntityCreateRequest, IPersonCreateRequest
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime BirthDate { get; set; }
        }

        public class EmployeeUpdateRequest : EntityUpdateRequest, IEmployeeUpdateRequest
        {
            public string Employer { get; set; }
        }

        public class SocialNetworkProfileUpdateRequest : EntityUpdateRequest, ISocialNetworkProfileUpdateRequest
        {
            public bool UsesTwitter { get; set; }
            public string TwitterUsername { get; set; }
        }

        #endregion

        #region Constructors

        public PeopleTests()
        {
            dataManager = GetDataManager();
            manager = new PeopleManager(dataManager);
        }

        #endregion

        [Test]
        public void PersonExists_False()
        {
            Assert.False(manager.PersonExists(Guid.NewGuid()));
        }

        [Test]
        public void GetDistinctEntityTypes_NotFound()
        {
            IEnumerable<string> types = manager.GetDistinctEntityTypes(new Guid[] { Guid.NewGuid() });
            Assert.AreEqual(0, types.Count());
        }

        [Test]
        public void GetDistinctEntityTypes_Empty()
        {
            IEnumerable<string> types = manager.GetDistinctEntityTypes(new Guid[] {});
            Assert.AreEqual(0, types.Count());
        }

        [Test]
        public void CreatePerson_Myself()
        {
            CreateMyself();

            CreatePerson_Myself_Asserts();
        }

        [Test]
        public void CreatePerson_MultipleWithSimilarNames()
        {
            CreatePerson("Chris", "Elston", new DateTime(1980, 7, 26));
            CreatePerson("Amy", "Elston", new DateTime(1986, 6, 6));
            CreatePerson("Chris", "Hemsworth", new DateTime(1983, 8, 1));

            CreatePerson_MultipleWithSimilarNames_Asserts();
        }

        [Test]
        public void CreatePerson_UpdateEmployee_UpdateSocialNetworkProfile()
        {
            Guid id = CreateMyself();

            EmployeeUpdateRequest employeeUpdateRequest = new EmployeeUpdateRequest()
            {
                Id = id,
                Employer = "VML"
            };
            manager.UpdateEmployee(employeeUpdateRequest);

            Employee employee = manager.LoadPerson<Employee>(id);
            Assert.AreEqual(employeeUpdateRequest.Employer, employee.Employer);
            Assert.AreEqual("Chris", employee.FirstName);
            Assert.AreEqual("Elston", employee.LastName);
            Assert.AreEqual(new DateTime(1980, 7, 26), employee.BirthDate);

            SocialNetworkProfileUpdateRequest socialNetworkProfileUpdateRequest = new SocialNetworkProfileUpdateRequest()
            {
                Id = id,
                UsesTwitter = true,
                TwitterUsername = "celston80"
            };
            manager.UpdateSocialNetworkProfile(socialNetworkProfileUpdateRequest);
            
            CreatePerson_UpdateEmployee_UpdateSocialNetworkProfile_Asserts();
        }

        protected virtual void CreatePerson_Myself_Asserts()
        {
        }

        protected virtual void CreatePerson_MultipleWithSimilarNames_Asserts()
        {
        }

        protected virtual void CreatePerson_UpdateEmployee_UpdateSocialNetworkProfile_Asserts()
        {
        }

        private Guid CreateMyself()
        {
            Guid id = CreatePerson("Chris", "Elston", new DateTime(1980, 7, 26));
            
            return id;
        }

        private Guid CreatePerson(string firstName, string lastName, DateTime birthDate)
        {
            PersonCreateRequest request = new PersonCreateRequest()
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate
            };

            manager.CreatePerson(request);

            Person person = manager.LoadPerson<Person>(request.Id);
            Assert.AreEqual(request.FirstName, person.FirstName);
            Assert.AreEqual(request.LastName, person.LastName);
            Assert.AreEqual(request.BirthDate, person.BirthDate);

            return request.Id;
        }
    }
}
