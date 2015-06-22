using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

using Gnosis.Testing;
using System.Runtime.InteropServices;
using Gnosis.Data;
using System.Data.Common;
using System.Data;

namespace Gnosis.UnitTests
{
    [TestFixture]
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

        public class ChimeraDataManager : DbDataManager
        {
            public ChimeraDataManager()
                : base("gnosis")
            {
                using (DbConnection conn = GetConnection())
                using (DbTransaction trans = conn.BeginTransaction())
                using (DbCommand cmd = CreateTextCommand(conn, trans, "DELETE FROM DataVarbinaryMax"))
                {
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
            }

            public void SaveChimera(Chimera chimera)
            {
                using (DbConnection conn = GetConnection())
                using (DbTransaction trans = conn.BeginTransaction())
                {
                    foreach (PropertyInfo pi in typeof(Chimera).GetProperties())
                    {
                        using (DbCommand cmd = CreateTextCommand(conn, trans, "INSERT INTO DataVarbinaryMax (name, value) VALUES (@name, @value)"))
                        {
                            AddParameter(cmd, "@name", pi.Name);
                            if (pi.PropertyType == typeof(Int16))
                            {
                                Int16 value = (Int16)pi.GetValue(chimera);
                                Debug.Print("{0}", value);
                                byte[] bytes = BitConverter.GetBytes(value);
                                Debug.Print("{0}", BitConverter.ToString(bytes));
                                Int16 value2 = BitConverter.ToInt16(bytes, 0);
                                Debug.Print("{0}", value2);
                                AddParameter(cmd, DbType.Binary, "@value", bytes);
                                cmd.ExecuteNonQuery();
                            }
                            else if (pi.PropertyType == typeof(Int32))
                            {
                                Int32 value = (Int32)pi.GetValue(chimera);
                                Debug.Print("{0}", value);
                                byte[] bytes = BitConverter.GetBytes(value);
                                Debug.Print("{0}", BitConverter.ToString(bytes));
                                Int32 value2 = BitConverter.ToInt32(bytes, 0);
                                Debug.Print("{0}", value2);
                                AddParameter(cmd, DbType.Binary, "@value", bytes);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    using (DbCommand cmd = CreateTextCommand(conn, trans, "SELECT value FROM DataVarbinaryMax WHERE name = 'Int16'"))
                    using (DbDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            byte[] bytes = (byte[])dr["value"];
                            Int16 value = BitConverter.ToInt16(bytes, 0);
                            Debug.Print("{0}", value);
                        }
                    }

                    trans.Commit();

                    
                }
            }
        }

        public class Chimera
        {
            public Int16 Int16 { get; set; }
            public Int32 Int32 { get; set; }
            public Int64 Int64 { get; set; }
            public UInt16 Uint16 { get; set; }
            public UInt32 Uint32 { get; set; }
            public UInt64 Uint64 { get; set; }

            public double Double { get; set; }
            public decimal Decimal { get; set; }

            public bool Boolean { get; set; }

            public Chimera()
            {
                Int16 = Gnosis.RandomUtility.NextInt16();
                Int32 = Gnosis.RandomUtility.NextInt32();
                Int64 = Gnosis.RandomUtility.NextInt64();
                Uint16 = Gnosis.RandomUtility.NextUInt16();
                Uint32 = Gnosis.RandomUtility.NextUInt32();
                Uint64 = Gnosis.RandomUtility.NextUInt64();

                Double = Gnosis.RandomUtility.NextDouble();
                Decimal = Gnosis.RandomUtility.NextDecimal();

                Boolean = Gnosis.RandomUtility.NextBoolean();
            }
        }

        [Test]
        public void Foo()
        {
            Random random = new Random();
            Chimera chimera = new Chimera();
            ChimeraDataManager dataManager = new ChimeraDataManager();

            Type type = chimera.GetType();
            Debug.Print("{0}", type.FullName);

            foreach (PropertyInfo pi in type.GetProperties())
            {
                Debug.Print("{0} ({1}) {2} = {3}", pi.PropertyType.Name, Marshal.SizeOf(pi.GetValue(chimera)), pi.Name, pi.GetValue(chimera));
            }

            dataManager.SaveChimera(chimera);
        }
    }
}
