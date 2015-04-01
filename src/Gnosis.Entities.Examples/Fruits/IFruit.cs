using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gnosis.Entities.Attributes;

namespace Gnosis.Entities.Examples.Fruits
{
    [EntityFieldsInterface]
    public interface IFruit
    {
        [EntityField]
        decimal Price { get; }
    }
}
