using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gnosis.Entities.Examples.Fruits;
using Gnosis.Entities.Testing.Data;

namespace Gnosis.UnitTests.Entities.Fruits
{
    public class MemoryFruitTests : FruitTests
    {
        public class MemoryFruitDataManager : MemoryEntityDataManager, IFruitDataManager
        {
            public Dictionary<string, int> GetEntityTypeCounts()
            {
                Dictionary<string, int> result = new Dictionary<string, int>();

                foreach (MemoryEntity me in entities.Values)
                {
                    if (!result.ContainsKey(me.Type))
                    {
                        result.Add(me.Type, 0);
                    }
                    result[me.Type]++;
                }

                return result;
            }
        }

        protected override IFruitDataManager GetDataManager()
        {
            return new MemoryFruitDataManager();
        }
    }
}
