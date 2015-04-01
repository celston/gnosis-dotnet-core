using System;
using System.Collections.Generic;

using Gnosis.Entities.Attributes;

namespace Gnosis.Entities.Examples.Fruits.Baskets
{
    [EntityFieldsInterface]
    public interface IBasketByObjects
    {
        [EntityField]
        IEnumerable<Fruit> Fruit { get; }
    }
}