using System;
using System.Collections.Generic;

using Gnosis.Entities.Attributes;
using Gnosis.Entities.Requests;

namespace Gnosis.Entities.Examples.Fruits.Baskets
{
    [EntityFieldsInterface]
    public interface IBasketCreateRequest : IEntityCreateRequest, IBasketByIds
    {
    }
}
