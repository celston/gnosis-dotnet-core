using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    public static class ValidationUtility
    {
        public static void Assert(bool condition, Exception exception)
        {
            if (!condition)
            {
                throw exception;
            }
        }

        public static void AssertNotNull(object o, Exception exception)
        {
            Assert(o != null, exception);
        }

        public static void AssertNotIsNullOrWhiteSpace(string s, Exception exception)
        {
            Assert(!string.IsNullOrWhiteSpace(s), exception);
        }
    }
}
