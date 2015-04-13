using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Gnosis.Entities.Examples.Fruits;
using Gnosis.Entities.Mvc.Models;
using Gnosis.Entities.Examples.WebApplication.Models;

namespace Gnosis.Entities.Examples.WebApplication.Areas.Fruits.Models
{
    public abstract class FruitCreateRequestModel : ExampleEntityCreateRequestModel, IFruitCreateRequest
    {
        public decimal Price { get; set; }
    }
}