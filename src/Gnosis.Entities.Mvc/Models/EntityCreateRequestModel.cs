using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.Entities.Mvc.Models
{
    public abstract class EntityCreateRequestModel
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public abstract Guid? Author { get; }

        public EntityCreateRequestModel()
        {
            Id = Guid.NewGuid();
            Created = DateTime.UtcNow;
        }
    }
}
