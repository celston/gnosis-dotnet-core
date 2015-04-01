using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.Entities.Requests
{
    public interface IEntityCreateRequest : IEntityMinimumRead
    {
        DateTime Created { get; }
    }
}
