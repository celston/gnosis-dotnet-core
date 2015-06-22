using System;

using Gnosis.Entities.MongoDb;
using Gnosis.Entities.Examples.Widgets;

namespace Gnosis.UnitTests.Entities.Widgets
{
    public class MongoDbWidgetTests : WidgetTests
    {
        public class MongoDbWidgetDataManager : MongoDbEntityDataManager, IWidgetDataManager
        {
            public MongoDbWidgetDataManager(string connectionString, string databaseName)
                : base(connectionString, databaseName)
            {
            }
        }
        
        protected override Gnosis.Entities.Examples.Widgets.IWidgetDataManager GetDataManager()
        {
            return new MongoDbWidgetDataManager("mongodb://localhost:27017", "widgets");
        }
    }
}
