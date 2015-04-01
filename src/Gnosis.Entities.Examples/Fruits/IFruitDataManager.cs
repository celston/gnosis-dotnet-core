﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.Entities.Examples.Fruits
{
    public interface IFruitDataManager : IEntityDataManager
    {
        Dictionary<string, int> GetEntityTypeCounts();
    }
}
