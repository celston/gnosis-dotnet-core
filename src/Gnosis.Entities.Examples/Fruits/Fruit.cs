using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.Entities.Examples.Fruits
{
    public abstract class Fruit : Entity, IFruit
    {
        public decimal Price { get; protected set; }
    }
}
