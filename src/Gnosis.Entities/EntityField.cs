using Gnosis.Entities.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gnosis.Entities
{
    public class EntityField
    {
        public string Name { get; private set; }
        public PropertyInfo Property { get; private set; }

        private Lazy<bool> isEntityReference;
        private Lazy<bool> isEntity;

        public bool IsEntityReference
        {
            get
            {
                return isEntityReference.Value;
            }
        }

        public bool IsEntity
        {
            get
            {
                return isEntity.Value;
            }
        }

        public bool IsNullable
        {
            get
            {
                return Property.PropertyType.IsGenericType && Property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            }
        }

        public bool IsList
        {
            get
            {
                if (Property.PropertyType.IsGenericType)
                {
                    if (Reflection.Utility.ImplementsInterface(Property.PropertyType.GetGenericTypeDefinition(), typeof(IEnumerable)))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool IsInteger
        {
            get
            {
                return IsBasic<int>();
            }
        }

        public bool IsBoolean
        {
            get
            {
                return IsBasic<bool>();
            }
        }

        public bool IsDecimal
        {
            get
            {
                return IsBasic<decimal>();
            }
        }

        public bool IsDateTime
        {
            get
            {
                return IsBasic<DateTime>();
            }
        }

        public bool IsGuid
        {
            get
            {
                return IsBasic<Guid>();
            }
        }

        public bool IsByteArray
        {
            get
            {
                return IsBasic<byte[]>();
            }
        }

        public bool IsString
        {
            get
            {
                if (Property.PropertyType == typeof(string))
                {
                    return true;
                }
                if (IsList && PropertyFirstGenericTypeArgument == typeof(string))
                {
                    return true;
                }
                return false;
            }
        }

        private bool IsBasic<T>()
        {
            if (Property.PropertyType == typeof(T))
            {
                return true;
            }
            if (IsNullable && PropertyFirstGenericTypeArgument == typeof(T))
            {
                return true;
            }
            if (IsList && PropertyFirstGenericTypeArgument == typeof(T))
            {
                return true;
            }
            return false;
        }

        public Type PropertyFirstGenericTypeArgument
        {
            get
            {
                return Property.PropertyType.GenericTypeArguments.First();
            }
        }

        public EntityField(EntityFieldAttribute attribute, PropertyInfo property)
        {
            this.Property = property;

            if (!string.IsNullOrEmpty(attribute.Name))
            {
                this.Name = attribute.Name;
            }
            else
            {
                this.Name = property.Name;
            }

            isEntityReference = new Lazy<bool>(() =>
            {
                if (Property.PropertyType.GetInterfaces().Contains(typeof(IEntityReference)))
                {
                    return true;
                }
                if (IsList && PropertyFirstGenericTypeArgument.GetInterfaces().Contains(typeof(IEntityReference)))
                {
                    return true;
                }
                return false;
            });

            isEntity = new Lazy<bool>(() =>
            {
                if (Property.PropertyType.GetInterfaces().Contains(typeof(IEntity)))
                {
                    return true;
                }
                if (IsList && PropertyFirstGenericTypeArgument.GetInterfaces().Contains(typeof(IEntity)))
                {
                    return true;
                }
                return false;
            });
        }
    }
}
