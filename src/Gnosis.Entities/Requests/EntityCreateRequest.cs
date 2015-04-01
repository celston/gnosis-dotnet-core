using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.Entities.Requests
{
    public abstract class EntityCreateRequest : IEntityCreateRequest
    {
        public DateTime Created { get; set; }
        public Guid Id { get; set; }
        public Guid? Author { get; set; }

        public EntityCreateRequest()
        {
            Created = DateTime.UtcNow;
            Id = Guid.NewGuid();
        }
    }
}
