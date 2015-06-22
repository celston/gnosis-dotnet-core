using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

namespace Gnosis.Entities.MongoDb
{
    public class MongoDbEntityDataManager : IEntityDataManager
    {
        private MongoClient client;
        private IMongoDatabase database;
        private Lazy<IMongoCollection<StoredEntity>> entityCollectionProxy;
        private Lazy<IMongoCollection<StoredEntityFieldValue>> fieldValueCollectionProxy;
        private InitializeEntityDelegate initializeEntityDelegate;

        private class StoredEntity
        {
            public string Type { get; set; }
            [BsonId]
            public Guid Id { get; set; }
            public Guid Revision { get; set; }
            public Guid? Author { get; set; }
            public string Label { get; set; }
            public DateTime Created { get; set; }
            public DateTime Updated { get; set; }
            public bool IsProtected { get; set; }
        }

        private class StoredEntityFieldValue
        {
            [BsonId]
            public Guid FieldValueId { get; set; }
            public Guid Revision { get; set; }
            public string Name { get; set; }
            public object Value { get; set; }
        }

        public MongoDbEntityDataManager(string connectionString, string databaseName)
        {
            client = new MongoClient(connectionString);
            database = client.GetDatabase(databaseName);
            
            entityCollectionProxy = new Lazy<IMongoCollection<StoredEntity>>(() =>
            {
                return database.GetCollection<StoredEntity>("entity");
            });
            fieldValueCollectionProxy = new Lazy<IMongoCollection<StoredEntityFieldValue>>(() =>
            {
                return database.GetCollection<StoredEntityFieldValue>("fieldValue");
            });
        }

        private IMongoCollection<StoredEntity> entityCollection
        {
            get
            {
                return entityCollectionProxy.Value;
            }
        }

        private IMongoCollection<StoredEntityFieldValue> fieldValueCollection
        {
            get
            {
                return fieldValueCollectionProxy.Value;
            }
        }
        
        public bool EntityExists(Guid id)
        {
            throw new NotImplementedException();
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
            initializeEntityDelegate = d;
        }

        public async Task<bool> EntityExistsAsync(Guid id)
        {
            long count = await entityCollection.Find(x => x.Id == id).CountAsync();

            return count > 0;
        }

        public async Task CreateEntityAsync(string type, Guid id, Guid revision, Guid? author, string label, DateTime created, bool isProtected, IEnumerable<EntityFieldValue> fieldValues)
        {
            StoredEntity entity = new StoredEntity()
            {
                Type = type,
                Id = id,
                Revision = revision,
                Author = author,
                Label = label,
                Created = created,
                IsProtected = isProtected
            };
            
            await entityCollection.InsertOneAsync(entity);

            IEnumerable<StoredEntityFieldValue> storedFieldValues = fieldValues
                .Select(x => new StoredEntityFieldValue()
                {
                    Revision = revision,
                    Name = x.Field.Name,
                    Value = x.Value
                });

            await fieldValueCollection.InsertManyAsync(storedFieldValues);
        }

        public async Task<IEnumerable<T>> LoadEntitiesAsync<T>(IEnumerable<Guid> ids, IEnumerable<EntityField> fields, IEnumerable<EntityField> nestedFields) where T : IEntity
        {
            List<T> result = new List<T>();

            IEnumerable<string> fieldNames = fields.Select(x => x.Name);

            foreach (Guid id in ids)
            {
                List<StoredEntity> storedEntities = await entityCollection.FindAsync(x => x.Id == id).Result.ToListAsync();
                StoredEntity first = storedEntities.First();

                T entity = Activator.CreateInstance<T>();
                entity.GrantInitializeEntityDelegate(this);

                List<EntityFieldValue> fieldValues = new List<EntityFieldValue>();
                foreach (EntityField field in fields)
                {
                    List<StoredEntityFieldValue> storedFieldValues = await fieldValueCollection.FindAsync(x => x.Revision == first.Revision && x.Name == field.Name).Result.ToListAsync();
                    if (storedFieldValues.Count > 0)
                    {
                        fieldValues.Add(new EntityFieldValue(field, field.Property, storedFieldValues.First().Value));
                    }
                }

                initializeEntityDelegate.Invoke(first.Id, first.Revision, first.Author, first.Created, first.Updated, fieldValues);
                result.Add(entity);
            }

            return result;
        }
    }
}
