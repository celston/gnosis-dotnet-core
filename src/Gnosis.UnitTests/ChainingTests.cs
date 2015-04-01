using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using Gnosis.Testing;

namespace Gnosis.UnitTests
{
    public class ChainingTests : TestFixture
    {
        public abstract class Base<T>
            where T : Base<T>
        {
            private StringBuilder sb = new StringBuilder();

            public T Append(string s)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" ");
                }
                sb.Append(s);

                return (T)this;
            }

            public override string ToString()
            {
                return sb.ToString();
            }
        }

        public class Derived : Base<Derived>
        {
        }
        
        [Test]
        public void Foo()
        {
            Derived d = new Derived();

            d.Append("The").Append("quick").Append("brown").Append("fox").Append("ran").Append("over").Append("the").Append("lazy").Append("dog.");
            Debug.Print(d.ToString());
        }
    }
}
