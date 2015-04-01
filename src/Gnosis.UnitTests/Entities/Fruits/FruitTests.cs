using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

using Moq;
using NUnit.Framework;

using Gnosis.Testing;
using Gnosis.Entities;
using Gnosis.Entities.Requests;
using Gnosis.Entities.Testing.Data;
using Gnosis.Entities.Examples.Fruits;
using Gnosis.Entities.Examples.Fruits.Apples;
using Gnosis.Entities.Examples.Fruits.Oranges;
using Gnosis.Entities.Examples.Fruits.Bananas;
using Gnosis.Entities.Examples.Fruits.Baskets;
using Gnosis.Entities.Attributes;

namespace Gnosis.UnitTests.Entities
{
    public abstract class FruitTests : EntityTests
    {
        #region Protected Fields

        protected FruitManager manager;

        protected IFruitDataManager dataManager;

        #endregion

        #region Protected Override Methods

        protected override IEnumerable<Type> GetEntityTypes()
        {
            return FruitManager.Types;
        }

        protected override int ExpectedFieldCount
        {
            get { return 3; }
        }

        #endregion

        #region Protected Abstract Methods

        protected abstract IFruitDataManager GetDataManager();

        #endregion

        #region Nested Classes

        public class FruitCreateRequest : EntityCreateRequest, IFruitCreateRequest
        {
            public decimal Price { get; private set; }
        }

        public class AppleCreateRequest : FruitCreateRequest, IAppleCreateRequest
        {
            public string Color { get; set; }
        }

        public class OrangeCreateRequest : FruitCreateRequest, IOrangeCreateRequest
        {
        }

        public class BananaCreateRequest : FruitCreateRequest, IBananaCreateRequest
        {
        }

        public class BasketCreateRequest : EntityCreateRequest, IBasketCreateRequest
        {
            public List<Guid> Fruit { get; set; }

            public BasketCreateRequest()
            {
                Fruit = new List<Guid>();
            }
        }

        public class BasketByIds : Entity, IBasketByIds
        {
            public List<Guid> Fruit { get; private set; }
        }

        public class BasketByReferences : Entity, IBasketByReferences
        {
            public IEnumerable<FruitReference> Fruit { get; private set; }
        }

        public class BasketByOjects : Entity, IBasketByObjects
        {
            public IEnumerable<Fruit> Fruit { get; private set; }
        }

        #endregion

        #region Constructors

        public FruitTests()
        {
            dataManager = GetDataManager();
            manager = new FruitManager(dataManager);
        }

        #endregion

        #region Public Methods

        [Test]
        public void CreateFruitAndBasket()
        {
            BasketCreateRequest basketCreateRequest = new BasketCreateRequest();

            basketCreateRequest.Fruit.Add(CreateApple<FujiApple>());
            basketCreateRequest.Fruit.Add(CreateApple<GalaApple>());
            basketCreateRequest.Fruit.Add(CreateApple<HoneycrispApple>());

            basketCreateRequest.Fruit.Add(CreateOrange<BloodOrange>());
            basketCreateRequest.Fruit.Add(CreateOrange<NavelOrange>());
            basketCreateRequest.Fruit.Add(CreateOrange<ValenciaOrange>());

            for (int i = 0; i < 3; i++)
            {
                basketCreateRequest.Fruit.Add(CreateBanana());
            }

            Dictionary<string, int> entityTypeCounts = dataManager.GetEntityTypeCounts();

            foreach (string entityType in new string[] { "FujiApple", "GalaApple", "HoneycrispApple", "BloodOrange", "NavelOrange", "ValenciaOrange" })
            {
                Assert.True(entityTypeCounts.ContainsKey(entityType));
                Assert.AreEqual(1, entityTypeCounts[entityType]);
            }
            Assert.True(entityTypeCounts.ContainsKey("Banana"));
            Assert.AreEqual(3, entityTypeCounts["Banana"]);
            Assert.False(entityTypeCounts.ContainsKey("EmpireApple"));

            manager.CreateBasket(basketCreateRequest);

            BasketByIds b1 = manager.LoadBasket<BasketByIds>(basketCreateRequest.Id);
            BasketByReferences b2 = manager.LoadBasket<BasketByReferences>(basketCreateRequest.Id);
            BasketByOjects b3 = manager.LoadBasket<BasketByOjects>(basketCreateRequest.Id);

            Assert.AreEqual(basketCreateRequest.Fruit.Count, b1.Fruit.Count);
            Assert.AreEqual(basketCreateRequest.Fruit.Count, b2.Fruit.Count());

            for (var i = 0; i < 9; i++)
            {
                Assert.AreEqual(basketCreateRequest.Fruit[i], b1.Fruit[i]);
                Assert.AreEqual(basketCreateRequest.Fruit[i], b2.Fruit.ElementAt(i).Id);
                Assert.AreEqual(basketCreateRequest.Fruit[i], b3.Fruit.ElementAt(i).Id);
            }

            Assert.IsInstanceOf<FujiAppleReference>(b2.Fruit.ElementAt(0));
            Assert.IsInstanceOf<GalaAppleReference>(b2.Fruit.ElementAt(1));
            Assert.IsInstanceOf<HoneycrispAppleReference>(b2.Fruit.ElementAt(2));
            Assert.IsInstanceOf<BloodOrangeReference>(b2.Fruit.ElementAt(3));
            Assert.IsInstanceOf<NavelOrangeReference>(b2.Fruit.ElementAt(4));
            Assert.IsInstanceOf<ValenciaOrangeReference>(b2.Fruit.ElementAt(5));
            Assert.IsInstanceOf<BananaReference>(b2.Fruit.ElementAt(6));
            Assert.IsInstanceOf<BananaReference>(b2.Fruit.ElementAt(7));
            Assert.IsInstanceOf<BananaReference>(b2.Fruit.ElementAt(8));
        }

        [Test]
        public void GetDistinctFieldNames()
        {
            IEnumerable<string> fieldNames = Gnosis.Entities.Utility.GetDistinctFieldNames(FruitManager.Types);
            Debug.Print(string.Join(", ", fieldNames));
            Assert.AreEqual(3, fieldNames.Count());
        }

        [Test]
        public void GetFields([Values(typeof(GalaApple), typeof(HoneycrispApple), typeof(FujiApple), typeof(NavelOrange), typeof(ValenciaOrange), typeof(BloodOrange), typeof(Banana))] Type type)
        {
            IDictionary<string, EntityField> fields = Gnosis.Entities.Utility.GetFields(type);
            foreach (KeyValuePair<string, EntityField> kvp in fields)
            {
                Assert.AreEqual(kvp.Key, kvp.Value.Name);
                Debug.Print("{0}, {1}", kvp.Key, kvp.Value.Property.DeclaringType.FullName);
                Debug.Print("{0}", kvp.Value.Property.DeclaringType.IsAssignableFrom(type));
            }
        }

        #endregion

        #region Private Methods

        private Guid CreateApple<T>() where T : Apple
        {
            AppleCreateRequest request = new AppleCreateRequest();
            request.Color = "red";
            manager.CreateApple<T>(request);

            IEnumerable<Fruit> fruits = manager.LoadFruits(new Guid[] { request.Id });
            Debug.Print("{0}, {1}", fruits.First().Id, fruits.OfType<Apple>().First().Color);

            return request.Id;
        }

        private Guid CreateOrange<T>() where T : Orange
        {
            OrangeCreateRequest request = new OrangeCreateRequest();
            manager.CreateOrange<T>(request);

            return request.Id;
        }

        private Guid CreateBanana()
        {
            BananaCreateRequest request = new BananaCreateRequest();
            manager.CreateBanana(request);

            return request.Id;
        }

        #endregion
    }
}
