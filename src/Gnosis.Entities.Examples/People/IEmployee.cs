using Gnosis.Entities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.Entities.Examples.People
{
    [EntityFieldsInterface]
    public interface IEmployee
    {
        [EntityField]
        string Employer { get; }
    }
}
