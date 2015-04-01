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
    public abstract class EntityManager
    {
        #region Private Fields

        private IEntityDataManager entityDataManager;

        #endregion

        #region Protected Methods

        protected bool EntityExists(Guid id, string type)
        {
            return entityDataManager.EntityExists(type, id);
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
            if (entityDataManager.EntityExists(type, request.Id))
            {
                throw new EntityExistsException(request.Id);
            }

            Guid revision = Guid.NewGuid();
            entityDataManager.CreateEntity(type, request.Id, revision, request.Author, label, request.Created, isProtected, Utility.GetFieldValues(request, request.GetType()));

            return revision;
        }

        protected Guid UpdateEntity(IEntityUpdateRequest request, string type)
        {
            return UpdateEntity(request, type, null);
        }

        protected Guid UpdateEntity(IEntityUpdateRequest request, string type, string label)
        {
            if (!entityDataManager.EntityExists(type, request.Id))
            {
                throw new EntityNotFoundException(request.Id);
            }

            Guid revision = Guid.NewGuid();
            entityDataManager.UpdateEntity(request.Id, revision, request.Author, label, request.Updated, Utility.GetFieldValues(request, request.GetType()));

            return revision;
        }

        protected TResult LoadEntity<TResult>(Guid id, IEnumerable<string> allowedTypes)
            where TResult : IEntity
        {
            string entityType = GetEntityType<TResult>();
            if (!allowedTypes.Contains(entityType))
            {
                throw new UnpermittedEntityTypeException(entityType, allowedTypes);
            }
            
            return LoadEntity<TResult>(id, entityType);
        }

        protected TResult LoadEntity<TResult>(Guid id, string type)
            where TResult : IEntity
        {
            TResult result = entityDataManager.LoadEntity<TResult>(type, id, Utility.GetFields<TResult>().Values);

            if (result == null)
            {
                throw new EntityNotFoundException(id);
            }

            return result;
        }

        protected IEnumerable<TResult> LoadEntities<TResult>(IEnumerable<Guid> ids, IEnumerable<Type> types)
            where TResult : IEntity
        {
            return entityDataManager.LoadEntities<TResult>(ids, Utility.GetFields(types));
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
