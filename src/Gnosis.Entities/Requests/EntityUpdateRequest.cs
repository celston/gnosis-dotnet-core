using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.Entities.Requests
{
    public abstract class EntityUpdateRequest : IEntityUpdateRequest
    {
        public Guid Revision { get; set; }
        public DateTime Updated { get; set; }
        public Guid Id { get; set; }
        public Guid? Author { get; set; }

        public EntityUpdateRequest()
        {
            Updated = DateTime.UtcNow;
        }
    }
}
