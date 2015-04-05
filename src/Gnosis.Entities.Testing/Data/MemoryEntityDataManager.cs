using Gnosis.Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.Entities.Testing.Data
{
    public class MemoryEntityDataManager : IEntityDataManager
    {
        #region Private Classes

        protected class MemoryEntity : IEntityRead
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

        #endregion

        #region Private Fields

        private InitializeEntityDelegate initializeEntityDelegate;
        private Dictionary<Guid, Dictionary<string, object>> fieldValues = new Dictionary<Guid, Dictionary<string, object>>();
        private Lazy<MethodInfo> generateEntityReferenceListMethod;

        #endregion

        #region Constructors

        public MemoryEntityDataManager()
        {
            generateEntityReferenceListMethod = new Lazy<MethodInfo>(() =>
            {
                return this.GetType().GetMethod("GenerateEntityReferenceList", BindingFlags.NonPublic | BindingFlags.Instance);
            });
        }

        #endregion

        #region Protected Fields

        protected Dictionary<Guid, MemoryEntity> entities = new Dictionary<Guid, MemoryEntity>();        

        #endregion

        #region Public Methods

        public bool EntityExists(Guid id)
        {
            return entities.ContainsKey(id);
        }

        public void CreateEntity(string type, Guid id, Guid revision, Guid? author, string label, DateTime created, bool isProtected, IEnumerable<EntityFieldValue> fieldValues)
        {
            MemoryEntity entity = new MemoryEntity()
            {
                Id = id,
                Revision = revision,
                Author = author,
                Type = type,
                Label = label,
                Created = created,
                Updated = created,
                IsProtected = isProtected
            };

            entities.Add(id, entity);

            this.fieldValues.Add(revision, new Dictionary<string,object>());
            foreach (EntityFieldValue efv in fieldValues)
            {
                this.fieldValues[revision].Add(efv.Field.Name, efv.Value);
            }
        }

        public void UpdateEntity(Guid id, Guid revision, Guid? author, string label, DateTime updated, IEnumerable<EntityFieldValue> fieldValues)
        {
            if (!entities.ContainsKey(id))
            {
                throw new EntityNotFoundException(id);
            }

            Guid oldRevision = entities[id].Revision;
            this.fieldValues.Add(revision, new Dictionary<string, object>());
            IEnumerable<string> fieldNames = fieldValues.Select(x => x.Field.Name);
            IEnumerable<string> valuesToBeCopied = this.fieldValues[oldRevision].Keys.Where(x => !fieldNames.Contains(x));
            foreach (string valueToBeCopied in valuesToBeCopied)
            {
                this.fieldValues[revision].Add(valueToBeCopied, this.fieldValues[oldRevision][valueToBeCopied]);
            }

            entities[id].Revision = revision;
            entities[id].Updated = updated;
            if (!string.IsNullOrWhiteSpace(label))
            {
                entities[id].Label = label;
            }

            foreach (EntityFieldValue efv in fieldValues)
            {
                this.fieldValues[revision].Add(efv.Field.Name, efv.Value);
            }
        }
        
        public void AcceptInitializeDelegate(InitializeEntityDelegate d)
        {
            initializeEntityDelegate = d;
        }

        public string GetEntityType(Guid id)
        {
            return entities[id].Type;
        }

        public IEnumerable<string> GetDistinctEntityTypes(IEnumerable<Guid> ids)
        {
            return entities.Values.Where(x => ids.Contains(x.Id)).Select(x => x.Type).Distinct();
        }

        public IEnumerable<T> LoadEntities<T>(IEnumerable<Guid> ids, IEnumerable<EntityField> fields, IEnumerable<EntityField> nestedFields) where T : IEntity
        {
            List<T> result = new List<T>();

            foreach (Guid id in ids)
            {
                MemoryEntity me = entities[id];
                Type matchingType = Utility.GetMatchingEntityType<T>(me.Type);
                T entity = (T)Activator.CreateInstance(matchingType);
                IEnumerable<EntityField> filteredFields = fields.Where(x => x.Property.DeclaringType.IsAssignableFrom(matchingType));

                entity.GrantInitializeEntityDelegate(this);
                initializeEntityDelegate(me.Id, me.Revision, me.Author, me.Created, me.Updated, LoadEntityFields(me.Revision, filteredFields, nestedFields));

                result.Add(entity);
            }

            return result;
        }

        #endregion

        #region Protected Methods

        protected IEnumerable<EntityFieldValue> LoadEntityFields(Guid revision, IEnumerable<EntityField> fields, IEnumerable<EntityField> nestedFields)
        {
            List<EntityFieldValue> result = new List<EntityFieldValue>();

            foreach (EntityField ef in fields)
            {
                EntityFieldValue efv = null;
                if (ef.IsEntityReference)
                {
                    if (ef.IsList)
                    {
                        efv = new EntityFieldValue(ef, ef.Property, GenerateEntityReferenceListGeneric(ef, revision));
                    }
                }
                else if (ef.IsEntity)
                {
                    if (ef.IsList)
                    {
                        efv = new EntityFieldValue(ef, ef.Property, GenerateEntityListGeneric(ef, revision, nestedFields));
                    }
                }
                else
                {
                    if (this.fieldValues.ContainsKey(revision) && this.fieldValues[revision].ContainsKey(ef.Name))
                    {
                        efv = new EntityFieldValue(ef, ef.Property, this.fieldValues[revision][ef.Name]);
                    }
                }
                
                result.Add(efv);
            }

            return result;
        }

        protected object GenerateEntityListGeneric(EntityField ef, Guid revision, IEnumerable<EntityField> fields)
        {
            return Utility.GenerateEntityListGeneric(ef, this, (IEnumerable<Guid>)fieldValues[revision][ef.Name], fields);
        }

        protected object GenerateEntityReferenceListGeneric(EntityField ef, Guid revision)
        {
            return Utility.GenerateEntityReferenceListGeneric(ef, this, (IEnumerable<Guid>)fieldValues[revision][ef.Name]);
        }

        #endregion
    }
}
