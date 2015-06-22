using System;

using Gnosis.Entities.MongoDb;
using Gnosis.Entities.Examples.Fruits;

namespace Gnosis.UnitTests.Entities.Fruits
{
    public class MongoDbFruitTests : FruitTests
    {
        public class MongoDbFruitDataManger : MongoDbEntityDataManager, IFruitDataManager
        {
            public MongoDbFruitDataManger()
                : base("mongodb://localhost:27017", "fruit")
            {
            }

            public System.Collections.Generic.Dictionary<string, int> GetEntityTypeCounts()
            {
                throw new NotImplementedException();
            }
        }

        protected override IFruitDataManager GetDataManager()
        {
            return new MongoDbFruitDataManger();
        }
    }
}
