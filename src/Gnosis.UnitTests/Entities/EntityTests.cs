using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using Gnosis.Testing;
using Gnosis.Entities;

namespace Gnosis.UnitTests.Entities
{
    public abstract class EntityTests : TestFixture
    {
        protected abstract IEnumerable<Type> GetEntityTypes();
        protected abstract int ExpectedFieldCount { get; }

        [Test]
        public void GetFields()
        {
            IEnumerable<EntityField> fields = Gnosis.Entities.Utility.GetFields(GetEntityTypes());

            foreach (EntityField field in fields)
            {
                Debug.Print("{0}, {1}", field.Name, field.Property.DeclaringType.FullName);
            }
            Assert.AreEqual(ExpectedFieldCount, fields.Count());
        }

        
    }
}
