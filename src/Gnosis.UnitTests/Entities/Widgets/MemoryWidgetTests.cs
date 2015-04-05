using System;

using Gnosis.Entities.Testing.Data;
using Gnosis.Entities.Examples.Widgets;

namespace Gnosis.UnitTests.Entities.Widgets
{
    public class MemoryWidgetTests : WidgetTests
    {
        public class MemoryWidgetDataManager : MemoryEntityDataManager, IWidgetDataManager
        {
        }

        protected override IWidgetDataManager GetDataManager()
        {
            return new MemoryWidgetDataManager();
        }
    }
}
