using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.Entities
{
    public interface IEntityDataManager : IEntityInitializer
    {
        bool EntityExists(Guid id);
        Task<bool> EntityExistsAsync(Guid id);
        void CreateEntity(string type, Guid id, Guid revision, Guid? author, string label, DateTime created, bool isProtected, IEnumerable<EntityFieldValue> fieldValues);
        Task CreateEntityAsync(string type, Guid id, Guid revision, Guid? author, string label, DateTime created, bool isProtected, IEnumerable<EntityFieldValue> fieldValues);
        void UpdateEntity(Guid id, Guid revision, Guid? author, string label, DateTime updated, IEnumerable<EntityFieldValue> fieldValues);
        IEnumerable<T> LoadEntities<T>(IEnumerable<Guid> ids, IEnumerable<EntityField> fields, IEnumerable<EntityField> nestedFields) where T : IEntity;
        Task<IEnumerable<T>> LoadEntitiesAsync<T>(IEnumerable<Guid> ids, IEnumerable<EntityField> fields, IEnumerable<EntityField> nestedFields) where T : IEntity;
        IEnumerable<string> GetDistinctEntityTypes(IEnumerable<Guid> ids);
        string GetEntityType(Guid id);
    }
}
