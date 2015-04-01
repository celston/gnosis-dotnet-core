using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    public static class DateTimeUtility
    {
        public static bool DateTimeEquals(DateTime? a, DateTime? b)
        {
            if (!a.HasValue)
            {
                if (b.HasValue)
                {
                    return false;
                }
            }
            else
            {
                if (!b.HasValue)
                {
                    return false;
                }
            }

            TimeSpan ts = a.Value - b.Value;

            return ts.TotalSeconds < 1;
        }
    }
}
