using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis
{
    public static class RandomExtensions
    {
        public static Int16 NextInt16(this Random rnd)
        {
            byte[] buffer = new byte[sizeof(Int16)];
            rnd.NextBytes(buffer);
            return BitConverter.ToInt16(buffer, 0);
        }

        public static Int32 NextInt32(this Random rnd)
        {
            byte[] buffer = new byte[sizeof(Int32)];
            rnd.NextBytes(buffer);
            return BitConverter.ToInt32(buffer, 0);
        }

        public static Int64 NextInt64(this Random rnd)
        {
            byte[] buffer = new byte[sizeof(Int64)];
            rnd.NextBytes(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }

        public static UInt16 NextUInt16(this Random rnd)
        {
            byte[] buffer = new byte[sizeof(UInt16)];
            rnd.NextBytes(buffer);
            return BitConverter.ToUInt16(buffer, 0);
        }

        public static UInt32 NextUInt32(this Random rnd)
        {
            byte[] buffer = new byte[sizeof(UInt32)];
            rnd.NextBytes(buffer);
            return BitConverter.ToUInt32(buffer, 0);
        }

        public static UInt64 NextUInt64(this Random rnd)
        {
            byte[] buffer = new byte[sizeof(UInt64)];
            rnd.NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }

        public static double NextDouble(this Random rnd)
        {
            byte[] buffer = new byte[sizeof(double)];
            rnd.NextBytes(buffer);
            return BitConverter.ToDouble(buffer, 0);
        }

        public static decimal NextDecimal(this Random rng)
        {
            byte scale = (byte)rng.Next(29);
            bool sign = rng.Next(2) == 1;
            return new decimal(rng.NextInt32(),
                               rng.NextInt32(),
                               rng.NextInt32(),
                               sign,
                               scale);
        }

        public static bool NextBoolean(this Random rnd)
        {
            return rnd.NextDouble() > 0.5;
        }
    }
}
