using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace Gnosis.Testing
{
    [TestFixture]
    public abstract class TestFixture
    {
        protected string GetRandomWord()
        {
            return Utility.GetRandomWord();
        }

        protected string GetRandomPhrase()
        {
            return Utility.GetRandomPhrase();
        }

        protected string GetRandomPhrase(int numWords)
        {
            return Utility.GetRandomPhrase(numWords);
        }
    }
}
