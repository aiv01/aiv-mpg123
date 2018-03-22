using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;


namespace Aiv.Mpg123.Tests
{
    [TestFixture]
    public class TestOutputs
    {
        [Test]
        public void TestRatesLength()
        {
            Assert.That(Mpg123.Rates.ToArray(), Has.Length.GreaterThan(0));

        }
        [Test]
        public void TestEncodingsLength()
        {
            Assert.That(Mpg123.Encodings.ToArray(), Has.Length.GreaterThan(0));

        }
    }

   
}
