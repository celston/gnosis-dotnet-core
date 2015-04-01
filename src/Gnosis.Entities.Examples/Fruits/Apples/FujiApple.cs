using System;

using Gnosis.Entities.Attributes;

namespace Gnosis.Entities.Examples.Fruits.Apples
{
    public class FujiApple : Apple
    {
        [EntityField]
        public string Pattern { get; private set; }
    }
}
