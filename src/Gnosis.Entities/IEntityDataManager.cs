using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.Entities
{
    public interface IEntityDataManager : IEntityInitializer
    {
        bool EntityExists(string type, Guid id);
        void CreateEntity(string type, Guid id, Guid revision, Guid? author, string label, DateTime created, bool isProtected, IEnumerable<EntityFieldValue> fieldValues);
        void UpdateEntity(Guid id, Guid revision, Guid? author, string label, DateTime updated, IEnumerable<EntityFieldValue> fieldValues);
        T LoadEntity<T>(string type, Guid id, IEnumerable<EntityField> fields) where T : IEntity;
        IEnumerable<string> GetDistinctEntityTypes(IEnumerable<Guid> ids);
        string GetEntityType(Guid id);
        IEnumerable<T> LoadEntities<T>(IEnumerable<Guid> ids, IEnumerable<EntityField> fields) where T : IEntity;
    }
}
