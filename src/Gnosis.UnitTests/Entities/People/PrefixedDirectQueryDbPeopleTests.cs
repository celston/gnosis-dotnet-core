using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.UnitTests.Entities.People
{
    public class PrefixedDirectQueryDbPeopleTests : DirectQueryDbPeopleTests
    {
        protected override string DatabasePrefix
        {
            get
            {
                return "People_";
            }
        }
    }
}
