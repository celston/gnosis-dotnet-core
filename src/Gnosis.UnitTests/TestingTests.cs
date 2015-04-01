using System;
using System.Diagnostics;

using NUnit.Framework;

using Gnosis.Testing;

namespace Gnosis.UnitTests
{
    public class TestingTests : TestFixture
    {
        [Test]
        public void GetRandomWord([Values(1, 10, 100)] int n)
        {
            for (int i = 0; i < n; i++)
            {
                Debug.Print(GetRandomWord());
            }
        }

        [Test]
        public void GetRandomPhrase([Values(1, 10, 100)]int n, [Values(1, 2, 4, 8)] int numWords)
        {
            for (int i = 0; i < n; i++)
            {
                Debug.Print(GetRandomPhrase(numWords));
            }
        }
    }
}
