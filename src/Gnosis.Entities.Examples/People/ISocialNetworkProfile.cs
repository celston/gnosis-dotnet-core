﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gnosis.Entities.Attributes;

namespace Gnosis.Entities.Examples.People
{
    [EntityFieldsInterface]
    public interface ISocialNetworkProfile
    {
        [EntityField]
        bool UsesTwitter { get; }
        [EntityField]
        string TwitterUsername { get; }
    }
}
