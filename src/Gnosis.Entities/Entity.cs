using System;
using System.Collections.Generic;

using Gnosis.Entities.Exceptions;

namespace Gnosis.Entities
{
    public abstract class Entity : IEntity
    {
        public Guid Id { get; protected set; }
        public Guid Revision { get; protected set; }
        public Guid? Author { get; protected set; }
        public bool IsProtected { get; protected set; }
        public DateTime Created { get; protected set; }
        public DateTime Updated { get; protected set; }

        public void GrantInitializeEntityDelegate(IEntityInitializer initializer)
        {
            initializer.AcceptInitializeDelegate((Guid id, Guid revision, Guid? author, DateTime created, DateTime updated, IEnumerable<EntityFieldValue> fields) =>
            {
                this.Id = id;
                this.Revision = revision;
                this.Author = author;
                this.Created = created;
                this.Updated = updated;

                foreach (EntityFieldValue field in fields)
                {
                    if (field == null)
                    {
                        continue;
                    }
                    if (field.Property == null)
                    {
                        throw new Exception(string.Format("property is null for {0}", field.Field.Name));
                    }
                    if (field.Property.SetMethod == null)
                    {
                        throw new MissingFieldSetMethodException(this.GetType(), field.Field.Name);
                    }
                    field.Property.SetValue(this, field.Value);
                }
            });
        }
    }
}
