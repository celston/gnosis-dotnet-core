using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    public static class RandomUtility
    {
        private static Lazy<Random> random = new Lazy<Random>(() =>
        {
            return new Random();
        });

        public static Int16 NextInt16()
        {
            return random.Value.NextInt16();
        }

        public static Int32 NextInt32()
        {
            return random.Value.NextInt32();
        }

        public static Int64 NextInt64()
        {
            return random.Value.NextInt64();
        }

        public static UInt16 NextUInt16()
        {
            return random.Value.NextUInt16();
        }

        public static UInt32 NextUInt32()
        {
            return random.Value.NextUInt32();
        }

        public static UInt64 NextUInt64()
        {
            return random.Value.NextUInt64();
        }

        public static double NextDouble()
        {
            byte[] buffer = new byte[sizeof(double)];
            random.Value.NextBytes(buffer);
            return BitConverter.ToDouble(buffer, 0);
        }

        public static bool NextBoolean()
        {
            return random.Value.NextBoolean();
        }

        public static decimal NextDecimal()
        {
            return random.Value.NextDecimal();
        }
    }
}
