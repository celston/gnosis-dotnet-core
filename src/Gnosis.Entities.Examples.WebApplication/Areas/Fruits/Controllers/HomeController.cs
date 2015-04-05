using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Gnosis.Entities.Attributes;
using Gnosis.Entities.Examples.Fruits;

namespace Gnosis.Entities.Examples.WebApplication.Areas.Fruits.Controllers
{
    public class HomeController : Controller
    {
        protected FruitManager manager;

        private Guid BasketId = Guid.Parse("780d93bd-1ce3-4bd1-90db-c1520ff4f95f");

        protected class SessionFruitDataManager : IFruitDataManager
        {
            protected class SessionEntity : IEntityRead
            {
                public Guid Id { get; set; }
                public Guid Revision { get; set; }
                public Guid? Author { get; set; }
                public string Type { get; set; }
                public string Label { get; set; }
                public DateTime Created { get; set; }
                public DateTime Updated { get; set; }
                public bool IsProtected { get; set; }
            }

            private InitializeEntityDelegate initializeEntityDelegate;
            private Lazy<Dictionary<Guid, SessionEntity>> entities;

            public SessionFruitDataManager()
            {
                entities = new Lazy<Dictionary<Guid, SessionEntity>>(() =>
                {
                    if (System.Web.HttpContext.Current.Session["Gnosis_Entities.Entities"] == null)
                    {
                        System.Web.HttpContext.Current.Session["Gnosis_Entities.Entities"] = new Dictionary<Guid, SessionEntity>();
                    }
                    return (Dictionary<Guid, SessionEntity>)System.Web.HttpContext.Current.Session["Gnosis_Entities.Entities"];
                });
            }
            
            public Dictionary<string, int> GetEntityTypeCounts()
            {
                throw new NotImplementedException();
            }

            public bool EntityExists(Guid id)
            {
                return entities.Value.ContainsKey(id);
            }

            public void CreateEntity(string type, Guid id, Guid revision, Guid? author, string label, DateTime created, bool isProtected, IEnumerable<EntityFieldValue> fieldValues)
            {
                throw new NotImplementedException();
            }

            public void UpdateEntity(Guid id, Guid revision, Guid? author, string label, DateTime updated, IEnumerable<EntityFieldValue> fieldValues)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<T> LoadEntities<T>(IEnumerable<Guid> ids, IEnumerable<EntityField> fields, IEnumerable<EntityField> nestedFields) where T : IEntity
            {
                List<T> result = new List<T>();

                foreach (Guid id in ids)
                {
                    SessionEntity me = entities.Value[id];
                    Type matchingType = Utility.GetMatchingEntityType<T>(me.Type);
                    T entity = (T)Activator.CreateInstance(matchingType);
                    IEnumerable<EntityField> filteredFields = fields.Where(x => x.Property.DeclaringType.IsAssignableFrom(matchingType));

                    entity.GrantInitializeEntityDelegate(this);
                    initializeEntityDelegate(me.Id, me.Revision, me.Author, me.Created, me.Updated, LoadEntityFields(me.Revision, filteredFields, nestedFields));

                    result.Add(entity);
                }

                return result;
            }

            private IEnumerable<EntityFieldValue> LoadEntityFields(Guid revision, IEnumerable<EntityField> filteredFields, IEnumerable<EntityField> nestedFields)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<string> GetDistinctEntityTypes(IEnumerable<Guid> ids)
            {
                throw new NotImplementedException();
            }

            public string GetEntityType(Guid id)
            {
                throw new NotImplementedException();
            }

            public void AcceptInitializeDelegate(InitializeEntityDelegate d)
            {
                throw new NotImplementedException();
            }
        }

        public class Basket : Entity
        {
            [EntityField]
            public IEnumerable<Fruit> Fruit { get; protected set; }
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return Json(manager.LoadBasket<Basket>(BasketId));
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            manager = new FruitManager(new SessionFruitDataManager());
        }

    }
}
