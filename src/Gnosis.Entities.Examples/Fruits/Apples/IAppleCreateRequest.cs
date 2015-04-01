using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gnosis.Entities.Requests;

namespace Gnosis.Entities.Examples.Fruits.Apples
{
    public interface IAppleCreateRequest : IFruitCreateRequest, IApple
    {
    }
}
