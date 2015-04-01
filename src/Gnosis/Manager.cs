using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    public abstract class Manager
    {
        #region Protected Methods

        protected void Assert<T>(bool condition, T exception)
            where T : Exception
        {
            if (!condition)
            {
                throw exception;
            }
        }

        protected void AssertNotNull<T>(object o, T exception)
            where T : Exception
        {
            Assert(o != null, exception);
        }

        protected void AssertNotIsNullOrWhiteSpace<T>(string s, T exception)
            where T : Exception
        {
            Assert(!string.IsNullOrWhiteSpace(s), exception);
        }

        #endregion
    }
}
