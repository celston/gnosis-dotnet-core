using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gnosis.Entities.Examples.Fruits.Apples;
using Gnosis.Entities.Examples.Fruits.Oranges;
using Gnosis.Entities.Examples.Fruits.Bananas;
using Gnosis.Entities.Examples.Fruits.Baskets;

namespace Gnosis.Entities.Examples.Fruits
{
    public class FruitManager : EntityManager
    {
        public static IEnumerable<Type> Types = new Type[] { typeof(GalaApple), typeof(HoneycrispApple), typeof(FujiApple), typeof(NavelOrange), typeof(ValenciaOrange), typeof(BloodOrange), typeof(Banana) };
        
        #region Constructors

        public FruitManager(IFruitDataManager dataManager)
        {
            SetEntityDataManager(dataManager);
        }

        #endregion

        #region Public Methods

        public Guid CreateApple<T>(IAppleCreateRequest request)
            where T : Apple
        {
            return CreateEntity(request, GetEntityType<T>());
        }

        public T LoadApple<T>(Guid id)
            where T : Apple
        {
            return LoadEntity<T>(id, new string[] { GetEntityType<FujiApple>(), GetEntityType<HoneycrispApple>(), GetEntityType<GalaApple>() });
        }

        public Guid CreateOrange<T>(IOrangeCreateRequest request)
            where T : Orange
        {
            return CreateEntity(request, GetEntityType<T>());
        }

        public Guid CreateBanana(IBananaCreateRequest request)
        {
            return CreateEntity(request, GetEntityType(typeof(Banana)));
        }

        public Guid CreateBasket(IBasketCreateRequest request)
        {
            return CreateEntity(request, "Basket");
        }

        public IEnumerable<Fruit> LoadFruits(IEnumerable<Guid> ids)
        {
            return LoadEntities<Fruit>(ids, Types);
        }

        public T LoadBasket<T>(Guid id) where T : IEntity
        {
            return LoadEntity<T>(id, "Basket");
        }

        #endregion
    }
}
