using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gnosis.Entities;
using Gnosis.Entities.Examples.Fruits;

namespace Gnosis.UnitTests.Entities.Fruits
{
    public class DirectQueryDbFruitTests : FruitTests
    {
        public class DirectQueryDbFruitDataManager : DirectQueryDbEntityDataManager, IFruitDataManager
        {
            public DirectQueryDbFruitDataManager()
                : base(ConfigurationManager.ConnectionStrings["entities"])
            {
            }
            
            public Dictionary<string, int> GetEntityTypeCounts()
            {
                throw new NotImplementedException();
            }
        }

        protected override IFruitDataManager GetDataManager()
        {
            return new DirectQueryDbFruitDataManager();
        }
    }
}
