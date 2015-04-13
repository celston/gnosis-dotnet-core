using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Gnosis.Entities.Examples.Fruits.Bananas;

namespace Gnosis.Entities.Examples.WebApplication.Areas.Fruits.Models
{
    public class BananaCreateRequestModel : FruitCreateRequestModel, IBananaCreateRequest
    {
    }
}