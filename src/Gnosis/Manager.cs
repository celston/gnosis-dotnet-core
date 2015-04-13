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

        protected void Assert(bool condition, Exception exception)
        {
            ValidationUtility.Assert(condition, exception);
        }

        protected void AssertNotNull(object o, Exception exception)
        {
            ValidationUtility.AssertNotNull(o, exception);
        }

        protected void AssertNotIsNullOrWhiteSpace(string s, Exception exception)
        {
            ValidationUtility.AssertNotIsNullOrWhiteSpace(s, exception);
        }

        protected void AssertHasValue<T>(Nullable<T> value, Exception exception)
            where T : struct
        {
            Assert(value.HasValue, exception);
        }

        #endregion
    }
}
