using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gnosis.Entities.Attributes;

namespace Gnosis.Entities.Examples.People
{
    [EntityFieldsInterface]
    public interface IPerson
    {
        [EntityField]
        string FirstName { get; }
        [EntityField]
        string LastName { get; }
        [EntityField]
        DateTime BirthDate { get; }
    }
}
