using System;
using System.Collections.Generic;

using Gnosis.Entities.Attributes;

namespace Gnosis.Entities.Examples.Fruits.Baskets
{
    [EntityFieldsInterface]
    public interface IBasketByReferences
    {
        [EntityField]
        IEnumerable<FruitReference> Fruit { get; }
    }
}