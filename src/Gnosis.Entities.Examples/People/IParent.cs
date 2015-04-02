using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gnosis.Entities.Attributes;

namespace Gnosis.Entities.Examples.People
{
    [EntityFieldsInterface]
    public interface IParent<T>
        where T : IEntity, IPerson
    {
        [EntityField]
        IEnumerable<T> Children { get; }
    }
}
