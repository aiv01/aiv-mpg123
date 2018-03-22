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

        [Test]
        public void TestEncodingSizeValidValue()    //always a valid value
        {
            Assert.That(Mpg123.GetEncodingSize(Mpg123.Encodings.ToArray()[0]), Is.Not.Zero);
        }

        //TODO: TEST WITH INVALID VALUE?

        [Test]
        public void TestFormatNoneWithNullThrowEx()
        {
            Assert.That(() => Mpg123.FormatNone(null), Throws.TypeOf<NullReferenceException>());
        }
        [Test]
        public void TestFormatNoneOK()
        {
            Mpg123 handle = new Mpg123();
            Assert.That(Mpg123.FormatNone(handle), Is.EqualTo(Mpg123.Errors.OK));
        }

        [Test]
        public void TestFormatAllWithNullThrowEx()
        {
            Assert.That(() => Mpg123.FormatAll(null), Throws.TypeOf<NullReferenceException>());
        }
        [Test]
        public void TestFormatAllOK()
        {
            Mpg123 handle = new Mpg123();
            Assert.That(Mpg123.FormatAll(handle), Is.EqualTo(Mpg123.Errors.OK));
        }
    }

   
}
