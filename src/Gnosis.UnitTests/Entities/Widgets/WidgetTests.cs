using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;

using Gnosis.Testing;
using Gnosis.Entities.Examples.Widgets;
using Gnosis.Entities.Requests;
using Gnosis.Entities;

namespace Gnosis.UnitTests.Entities.Widgets
{
    public abstract class WidgetTests : EntityTests
    {
        protected abstract IWidgetDataManager GetDataManager();

        private WidgetManager manager;

        public class WidgetCreateRequest : EntityCreateRequest, IWidgetCreateRequest
        {
            public string Label { get; set; }

            public string S1 { get; set; }
            public IEnumerable<string> S2 { get; set; }
            public string S3 { get; set; }
        }

        public class Widget : Entity, IWidget
        {
            public string Label { get; protected set; }
            public string S1 { get; protected set; }
            public IEnumerable<string> S2 { get; protected set; }
            public string S3 { get; protected set; }
        }

        public WidgetTests()
        {
            manager = new WidgetManager(GetDataManager());
        }

        [Test]
        public async Task CreateAndLoadAsync([Values(1, 10, 100)]int n)
        {
            List<WidgetCreateRequest> requests = new List<WidgetCreateRequest>();

            for (int i = 0; i < n; i++)
            {
                WidgetCreateRequest request = new WidgetCreateRequest()
                {
                    S1 = Gnosis.Testing.Utility.GetRandomPhrase()
                };
                Debug.Print(request.S1);

                requests.Add(request);
                await manager.CreateWidgetAsync(request);
            }

            List<Widget> widgets = (await manager.LoadWidgetsAsync<Widget>(requests.Select(x => x.Id))).ToList();
            Assert.AreEqual(n, widgets.Count());

            for (int i = 0; i < n; i++)
            {
                Assert.AreEqual(requests[i].Id, widgets[i].Id);
                Assert.AreEqual(requests[i].S1, widgets[i].S1);
            }
        }

        [Test]
        public void CreateAndLoad([Values(1, 10, 100)] int n)
        {
            List<WidgetCreateRequest> requests = new List<WidgetCreateRequest>();

            for (int i = 0; i < n; i++)
            {
                WidgetCreateRequest request = new WidgetCreateRequest()
                {
                    S1 = Gnosis.Testing.Utility.GetRandomPhrase()
                };
                Debug.Print(request.S1);

                requests.Add(request);
                manager.CreateWidget(request);
            }

            List<Widget> widgets = manager.LoadWidgets<Widget>(requests.Select(x => x.Id)).ToList();
            Assert.AreEqual(n, widgets.Count());

            for (int i = 0; i < n; i++)
            {
                Assert.AreEqual(requests[i].Id, widgets[i].Id);
                Assert.AreEqual(requests[i].S1, widgets[i].S1);
            }
        }

        protected override IEnumerable<Type> GetEntityTypes()
        {
            return new Type[] { typeof(Widget) };
        }

        protected override int ExpectedFieldCount
        {
            get { return 0; }
        }
    }
}
