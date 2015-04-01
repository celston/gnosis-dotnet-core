using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.Entities.Examples.Fruits.Apples
{
    public abstract class Apple : Fruit, IApple
    {
        public string Color { get; protected set; }
    }
}
