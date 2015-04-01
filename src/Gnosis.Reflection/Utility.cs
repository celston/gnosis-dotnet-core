using System;
using System.Linq;
using System.Reflection;

namespace Gnosis.Reflection
{
    public static class Utility
    {
        public static Type[] GetInterfaces(Type t)
        {
            TypeFilter typeFilter = new TypeFilter(_MyInterfaceFilter);

            return t.FindInterfaces(typeFilter, null);
        }

        public static bool ImplementsInterface(Type instanceType, Type interfaceType)
        {

            foreach (Type iface in GetInterfaces(instanceType))
            {
                if (iface == interfaceType)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsNullableEnum(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>) && t.GetGenericArguments()[0].IsEnum;
        }

        public static TAttribute GetAttribute<TAttribute>(Type t) where TAttribute : Attribute
        {
            return t.GetCustomAttributes<TAttribute>().FirstOrDefault();
        }

        public static TAttribute GetAttribute<TAttribute>(PropertyInfo pi) where TAttribute : Attribute
        {
            return pi.GetCustomAttributes<TAttribute>().FirstOrDefault();
        }

        public static bool HasAttribute<TAttribute>(PropertyInfo pi) where TAttribute : Attribute
        {
            TAttribute a = GetAttribute<TAttribute>(pi);
            return a != null;
        }

        public static bool HasAttribute<TAttribute>(Type t) where TAttribute : Attribute
        {
            TAttribute a = GetAttribute<TAttribute>(t);
            return a != null;
        }

        public static object InvokeGenericMethod(MethodInfo method, Type type, object obj, object[] parameters)
        {
            MethodInfo genericMethod = method.MakeGenericMethod(type);

            return genericMethod.Invoke(obj, parameters);
        }

        public static object InvokeGenericStaticMethod(Type staticClass, string methodName, Type genericTypeParameter, params object[] parameters)
        {
            MethodInfo method = staticClass.GetMethod(methodName);
            MethodInfo genericMethod = method.MakeGenericMethod(genericTypeParameter);

            return genericMethod.Invoke(null, parameters);
        }
        
        #region Private Methods

        private static bool _MyInterfaceFilter(Type typeObj, Object criteriaObj)
        {
            return true;
        }

        #endregion
    }
}
