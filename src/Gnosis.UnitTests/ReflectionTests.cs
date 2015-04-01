using System;
using System.Diagnostics;

using NUnit.Framework;

using Gnosis.Testing;

namespace Gnosis.UnitTests
{
    public class ReflectionTests : TestFixture
    {
        public static class ExampleStaticClass
        {
            public static string GetGenericTypeParameterName<T>()
            {
                return typeof(T).FullName;
            }
        }

        public class ExampleClass
        {
        }

        [Test]
        public void Foo()
        {
            Assert.AreEqual(ExampleStaticClass.GetGenericTypeParameterName<ExampleClass>(), Gnosis.Reflection.Utility.InvokeGenericStaticMethod(typeof(ExampleStaticClass), "GetGenericTypeParameterName", typeof(ExampleClass)));
        }
    }
}
