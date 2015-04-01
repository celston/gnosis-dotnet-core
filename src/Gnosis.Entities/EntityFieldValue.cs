using System;
using System.Reflection;

namespace Gnosis.Entities
{
    public class EntityFieldValue
    {
        public EntityField Field { get; private set; }
        public PropertyInfo Property { get; private set; }
        public object Value { get; private set; }

        public EntityFieldValue(EntityField field, PropertyInfo property, object value)
        {
            this.Field = field;
            this.Property = property;
            this.Value = value;
        }
    }
}
