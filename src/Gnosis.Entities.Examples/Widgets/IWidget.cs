using System;
using System.Collections.Generic;

using Gnosis.Entities.Attributes;

namespace Gnosis.Entities.Examples.Widgets
{
    [EntityFieldsInterface]
    public interface IWidget
    {
        string Label { get; }

        [EntityField]
        string S1 { get; }
        [EntityField]
        IEnumerable<string> S2 { get; }
    }
}
