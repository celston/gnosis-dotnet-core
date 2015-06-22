using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gnosis.Entities.Exceptions;
using Gnosis.Entities.Requests;
using Gnosis.Entities.Attributes;

namespace Gnosis.Entities
{
    public abstract class EntityManager : Manager
    {
        #region Constructors

        public EntityManager()
        {
        }

        public EntityManager(IEntityDataManager entityDataManager)
        {
            SetEntityDataManager(entityDataManager);
        }

        #endregion

        #region Private Fields

        private IEntityDataManager entityDataManager;

        #endregion

        #region Protected Methods

        protected bool EntityExists(Guid id)
        {
            return entityDataManager.EntityExists(id);
        }

        protected async Task<Guid> CreateEntityAsync(IEntityCreateRequest request, string type)
        {
            return await CreateEntityAsyc(request, type, "", false);
        }

        protected async Task<Guid> CreateEntityAsyc(IEntityCreateRequest request, string type, string label, bool isProtected)
        {
            AssertValidEntityType(type);
            await AssertEntityDoesntExistAsync(request.Id);

            Guid revision = Guid.NewGuid();
            await entityDataManager.CreateEntityAsync(type, request.Id, revision, request.Author, label, request.Created, isProtected, Utility.GetFieldValues(request, request.GetType()));

            return revision;
        }

        protected Guid CreateEntity(IEntityCreateRequest request, string type)
        {
            return CreateEntity(request, type, "", false);
        }

        protected Guid CreateEntity(IEntityCreateRequest request, string type, string label)
        {
            return CreateEntity(request, type, label, false);
        }

        protected Guid CreateEntity(IEntityCreateRequest request, string type, bool isProtected)
        {
            return CreateEntity(request, type, "", isProtected);
        }
        
        protected Guid CreateEntity(IEntityCreateRequest request, string type, string label, bool isProtected)
        {
            AssertValidEntityType(type);
            AssertEntityDoesntExist(request.Id);
            
            Guid revision = Guid.NewGuid();
            entityDataManager.CreateEntity(type, request.Id, revision, request.Author, label, request.Created, isProtected, Utility.GetFieldValues(request, request.GetType()));

            return revision;
        }

        protected void AssertEntitiesExist(IEnumerable<Guid> ids)
        {
            foreach (Guid id in ids)
            {
                AssertEntityExists(id);
            }
        }

        protected async Task AssertEntitiesExistAsync(IEnumerable<Guid> ids)
        {
            foreach (Guid id in ids)
            {
                await AssertEntityExistsAsync(id);
            }
        }

        protected void AssertEntityExists(Guid id)
        {
            Assert(entityDataManager.EntityExists(id), new EntityNotFoundException(id));
        }

        protected async Task AssertEntityExistsAsync(Guid id)
        {
            Assert(await entityDataManager.EntityExistsAsync(id), new EntityNotFoundException(id));
        }

        protected void AssertValidEntityType(string type)
        {
            AssertNotIsNullOrWhiteSpace(type, new InvalidEntityTypeException(type));
        }

        protected Guid UpdateEntity(IEntityUpdateRequest request, string type)
        {
            return UpdateEntity(request, type, null);
        }

        protected Guid UpdateEntity(IEntityUpdateRequest request, string type, string label)
        {
            AssertValidEntityType(type);
            AssertEntityExists(request.Id);

            Guid revision = Guid.NewGuid();
            entityDataManager.UpdateEntity(request.Id, revision, request.Author, label, request.Updated, Utility.GetFieldValues(request, request.GetType()));

            return revision;
        }

        protected async Task AssertEntityDoesntExistAsync(Guid id)
        {
            bool result = await entityDataManager.EntityExistsAsync(id);
            Assert(!result, new EntityNotFoundException(id));
        }
        
        protected void AssertEntityDoesntExist(Guid id)
        {
            Assert(!entityDataManager.EntityExists(id), new EntityNotFoundException(id));
        }

        protected Task<IEnumerable<T>> LoadEntitiesAsync<T>(IEnumerable<Guid> ids)
            where T : IEntity
        {
            IEnumerable<EntityField> fields = Utility.GetFields(typeof(T));
            return LoadEntitiesHelperAsync<T>(ids, fields, fields);
        }

        protected IEnumerable<T> LoadEntities<T>(IEnumerable<Guid> ids)
            where T : IEntity
        {
            IEnumerable<EntityField> fields = Utility.GetFields(typeof(T));
            return LoadEntitiesHelper<T>(ids, fields, fields);
        }

        protected IEnumerable<T> LoadEntities<T>(IEnumerable<Guid> ids, Type type)
            where T : IEntity
        {
            IEnumerable<EntityField> fields = Utility.GetFields(type);
            return LoadEntitiesHelper<T>(ids, fields, fields);
        }

        protected T LoadEntity<T>(Guid id, string type)
            where T : IEntity
        {
            IEnumerable<EntityField> fields = Utility.GetFields(Utility.GetMatchingEntityType<T>(type));

            return LoadEntitiesHelper<T>(new Guid[] { id }, fields, fields).FirstOrDefault();
        }

        protected T LoadEntity<T>(Guid id, string entityType, params Type[] types)
            where T : IEntity
        {
            return LoadEntitiesHelper<T>(new Guid[] { id }, Utility.GetFields<T>(), Utility.GetFields(types)).FirstOrDefault();
        }

        protected IEnumerable<T> LoadEntities<T>(IEnumerable<Guid> ids, IEnumerable<Type> types)
            where T : IEntity
        {
            IEnumerable<EntityField> fields = Utility.GetFields(types);

            return LoadEntitiesHelper<T>(ids, fields, fields);
        }

        protected T LoadEntity<T>(Guid id, params Type[] types)
            where T : IEntity
        {
            IEnumerable<EntityField> fields = Utility.GetFields(types);

            return LoadEntitiesHelper<T>(new Guid[] { id }, fields, fields).FirstOrDefault();
        }

        private async Task<IEnumerable<TResult>> LoadEntitiesHelperAsync<TResult>(IEnumerable<Guid> ids, IEnumerable<EntityField> fields, IEnumerable<EntityField> nestedFields)
            where TResult : IEntity
        {
            await AssertEntitiesExistAsync(ids);

            return await entityDataManager.LoadEntitiesAsync<TResult>(ids, fields, nestedFields);
        }

        private IEnumerable<TResult> LoadEntitiesHelper<TResult>(IEnumerable<Guid> ids, IEnumerable<EntityField> fields, IEnumerable<EntityField> nestedFields)
            where TResult : IEntity
        {
            AssertEntitiesExist(ids);
            
            return entityDataManager.LoadEntities<TResult>(ids, fields, nestedFields);
        }

        protected void SetEntityDataManager(IEntityDataManager entityDataManager)
        {
            this.entityDataManager = entityDataManager;
        }

        protected string GetEntityType<T>()
        {
            return GetEntityType(typeof(T));
        }

        protected string GetEntityType(Type t)
        {
            EntityTypeAttribute entityTypeAttribute = Reflection.Utility.GetAttribute<EntityTypeAttribute>(t);
            if (entityTypeAttribute != null)
            {
                return entityTypeAttribute.Name;
            }

            return t.Name;
        }

        #endregion
    }
}
