using System;

using Gnosis.Mvc.Models;

namespace Gnosis.Entities.Mvc.Models
{
    public abstract class EntityCreateResponseModel : ResponseModel
    {
        public Guid Revision { get; set; }
    }
}
