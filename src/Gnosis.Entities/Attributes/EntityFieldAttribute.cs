using System;

namespace Gnosis.Entities.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EntityFieldAttribute : Attribute
    {
        public string Name { get; private set; }

        public EntityFieldAttribute()
        {
        }

        public EntityFieldAttribute(string name)
        {
            this.Name = name;
        }
    }
}