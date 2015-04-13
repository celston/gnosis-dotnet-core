using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    public static class StringUtility
    {
        public static string LowercaseFirst(string s)
        {
            return Char.ToLowerInvariant(s[0]) + s.Substring(1);
        }
    }
}
