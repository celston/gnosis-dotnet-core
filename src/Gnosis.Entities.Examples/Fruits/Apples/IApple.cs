using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gnosis.Entities.Attributes;

namespace Gnosis.Entities.Examples.Fruits.Apples
{
    [EntityFieldsInterface]
    public interface IApple : IFruit
    {
        [EntityField]
        string Color { get; }
    }
}
