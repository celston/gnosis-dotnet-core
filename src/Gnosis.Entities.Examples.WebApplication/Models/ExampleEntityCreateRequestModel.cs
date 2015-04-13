using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Gnosis.Entities.Mvc.Models;

namespace Gnosis.Entities.Examples.WebApplication.Models
{
    public abstract class ExampleEntityCreateRequestModel : EntityCreateRequestModel
    {
        public override Guid? Author
        {
            get { return null; }
        }
    }
}