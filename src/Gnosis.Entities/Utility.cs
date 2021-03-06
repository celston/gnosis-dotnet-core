﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Gnosis.Entities.Exceptions;
using Gnosis.Entities.Attributes;

namespace Gnosis.Entities
{
    public static class Utility
    {
        private static ConcurrentDictionary<Type, IEnumerable<EntityField>> fields = new ConcurrentDictionary<Type, IEnumerable<EntityField>>();
        private static ConcurrentDictionary<Type, IEnumerable<Type>> matchingTypes = new ConcurrentDictionary<Type, IEnumerable<Type>>();
        private static ConcurrentDictionary<string, ConcurrentDictionary<Type, IEnumerable<Type>>> matchingEntityTypes = new ConcurrentDictionary<string, ConcurrentDictionary<Type, IEnumerable<Type>>>();

        public static IEnumerable<EntityField> GetFields<T>()
        {
            return GetFields(typeof(T));
        }

        public static IEnumerable<EntityField> GetFields(Type t)
        {
            return fields.GetOrAdd(t, (t2) =>
            {
                List<EntityField> result = new List<EntityField>();

                foreach (Type iface in t.GetInterfaces())
                {
                    if (Reflection.Utility.HasAttribute<EntityFieldsInterfaceAttribute>(iface))
                    {
                        foreach (PropertyInfo ifaceProperty in iface.GetProperties())
                        {
                            EntityFieldAttribute efa = Reflection.Utility.GetAttribute<EntityFieldAttribute>(ifaceProperty);
                            if (efa != null)
                            {
                                PropertyInfo typeProperty = t.GetProperty(ifaceProperty.Name);
                                result.Add(new EntityField(efa, typeProperty));
                            }
                        }
                    }
                }

                foreach (PropertyInfo typeProperty in t.GetProperties())
                {
                    EntityFieldAttribute efa = Reflection.Utility.GetAttribute<EntityFieldAttribute>(typeProperty);
                    if (efa != null)
                    {
                        result.Add(new EntityField(efa, typeProperty));
                    }
                }

                return result;
            });
        }

        public static IEnumerable<EntityField> GetFields(IEnumerable<Type> types)
        {
            List<EntityField> result = new List<EntityField>();
            
            foreach (Type type in types)
            {
                IEnumerable<EntityField> fields = GetFields(type);
                foreach (EntityField field in fields)
                {
                    if (!result.Any(x => x.Property.DeclaringType.FullName == field.Property.DeclaringType.FullName && x.Name == field.Name))
                    {
                        result.Add(field);
                    }
                }
            }
            
            return result;
        }

        public static IEnumerable<string> GetDistinctFieldNames(IEnumerable<Type> types)
        {
            return GetFields(types).Select(x => x.Name).Distinct();
        }

        public static IEnumerable<EntityFieldValue> GetFieldValues<TDestination>(object source)
        {
            return GetFieldValues(source, typeof(TDestination));
        }

        public static IEnumerable<EntityFieldValue> GetFieldValues(object source, Type destinationType)
        {
            List<EntityFieldValue> result = new List<EntityFieldValue>();

            Type sourceType = source.GetType();

            IEnumerable<EntityField> sourceFields = Utility.GetFields(sourceType);
            IEnumerable<EntityField> destinationFields = Utility.GetFields(destinationType);

            foreach (EntityField sourceField in sourceFields)
            {
                EntityField destinationField = destinationFields.Where(x => x.Name == sourceField.Name).FirstOrDefault();
                
                if (destinationField == null)
                {
                    throw new Exception(string.Format("Destination type {0} does not matching field {1} from source type {2}", destinationType.Name, sourceField.Name, sourceType.Name));
                }

                result.Add(new EntityFieldValue(sourceField, destinationField.Property, sourceField.Property.GetValue(source)));
            }

            return result;
        }

        public static IEnumerable<Type> GetMatchingTypes<T>()
        {
            return GetMatchingTypes(typeof(T));
        }

        public static IEnumerable<Type> GetMatchingTypes(Type t)
        {
            return matchingTypes.GetOrAdd(t, (targetType) =>
            {
                List<Type> result = new List<Type>();

                string[] assemblyNames = Gnosis.Configuration.StringArraySetting("Gnosis_Entities_Assemblies", new string[] { });
                foreach (string assemblyName in assemblyNames)
                {
                    Assembly assembly = Assembly.Load(assemblyName);
                    foreach (Type type in assembly.GetTypes())
                    {
                        if ((type == targetType || type.IsSubclassOf(targetType)) && !type.IsAbstract)
                        {
                            result.Add(type);
                        }
                    }
                }

                if (result.Count == 0)
                {
                    throw new Exception(string.Format("Found no matching type for \"{0}\". Searched assemblies: {1}", targetType.FullName));
                }

                IEnumerable<Type> exactMatches = result.Where(x => x.FullName == targetType.FullName);
                if (exactMatches.Count() > 0)
                {
                    return exactMatches;
                }

                return result;
            });
        }

        public static Type GetMatchingEntityType<T>(string entityType)
            where T : IEntity
        {
            IEnumerable<Type> types = GetMatchingEntityTypes<T>(entityType);

            if (types.Count() == 0)
            {
                throw new MissingEntityTypeException(entityType, typeof(T));
            }
            if (types.Count() > 1)
            {
                throw new MultipleEntityTypesException(entityType, typeof(T), types);
            }

            return types.First();
        }

        public static IEnumerable<Type> GetMatchingEntityTypes<T>(string entityType)
            where T : IEntity
        {
            return GetMatchingEntityTypes(entityType, typeof(T));
        }

        public static IEnumerable<Type> GetMatchingEntityTypes<T>(IEnumerable<string> entityTypes)
            where T : IEntity
        {
            return entityTypes.SelectMany(x => GetMatchingEntityTypes<T>(x));
        }

        private static IEnumerable<Type> GetMatchingEntityTypes(string entityType, Type type)
        {
            IEnumerable<Type> matchingTypes = GetMatchingTypes(type);
            List<Type> result = new List<Type>();

            foreach (Type matchingType in matchingTypes)
            {
                EntityTypeAttribute eta = Reflection.Utility.GetAttribute<EntityTypeAttribute>(matchingType);
                if (eta != null && eta.Name == entityType)
                {
                    result.Add(matchingType);
                }
                else if (matchingType.Name == entityType)
                {
                    result.Add(matchingType);
                }
            }

            return result;
        }

        public static Type GetMatchingEntityReferenceType<T>(string entityType) where T : IEntityReference
        {
            return GetMatchingEntityReferenceType(entityType, typeof(T));
        }

        public static Type GetMatchingEntityReferenceType(string entityType, Type type)
        {
            IEnumerable<Type> matchingTypes = GetMatchingTypes(type);
            List<Type> result = new List<Type>();

            foreach (Type matchingType in matchingTypes)
            {
                EntityTypeAttribute eta = Reflection.Utility.GetAttribute<EntityTypeAttribute>(matchingType);
                if (eta != null && eta.Name == entityType)
                {
                    result.Add(matchingType);
                }
                else if (matchingType.Name == entityType + "Reference")
                {
                    result.Add(matchingType);
                }
            }

            if (result.Count == 0)
            {
                throw new MissingEntityReferenceTypeException(entityType, type);
            }
            if (result.Count > 1)
            {
                throw new MultipleEntityReferenceTypesException(entityType, type, result);
            }

            return result.First();
        }

        public static IEnumerable<T> GenerateEntityReferenceList<T>(IEntityDataManager entityDataManager, IEnumerable<Guid> ids)
            where T : IEntityReference
        {
            List<T> result = new List<T>();

            foreach (Guid id in ids)
            {
                string entityType = entityDataManager.GetEntityType(id);

                Type type = Utility.GetMatchingEntityReferenceType<T>(entityType);
                T r = (T)Activator.CreateInstance(type);
                r.Id = id;

                result.Add(r);
            }

            return result;
        }

        public static IEnumerable<T> GenerateEntityList<T>(IEntityDataManager entityDataManager, IEnumerable<Guid> ids, IEnumerable<EntityField> fields)
            where T : IEntity
        {
            return entityDataManager.LoadEntities<T>(ids, fields, new EntityField[] {});
        }

        public static object GenerateEntityListGeneric(EntityField ef, IEntityDataManager entityDataManager, IEnumerable<Guid> ids, IEnumerable<EntityField> fields)
        {
            return GenerateEntityListGeneric(ef.PropertyFirstGenericTypeArgument, entityDataManager, ids, fields);
        }

        public static object GenerateEntityListGeneric(Type genericTypeParameter, IEntityDataManager entityDataManager, IEnumerable<Guid> ids, IEnumerable<EntityField> fields)
        {
            return Gnosis.Reflection.Utility.InvokeGenericStaticMethod(typeof(Utility), "GenerateEntityList", genericTypeParameter, entityDataManager, ids, fields);
        }

        public static object GenerateEntityReferenceListGeneric(EntityField ef, IEntityDataManager entityDataManager, IEnumerable<Guid> ids)
        {
            return GenerateEntityReferenceListGeneric(ef.PropertyFirstGenericTypeArgument, entityDataManager, ids);
        }

        public static object GenerateEntityReferenceListGeneric(Type genericTypeParameter, IEntityDataManager entityDataManager, IEnumerable<Guid> ids)
        {
            return Gnosis.Reflection.Utility.InvokeGenericStaticMethod(typeof(Utility), "GenerateEntityReferenceList", genericTypeParameter, entityDataManager, ids);
        }
    }
}
