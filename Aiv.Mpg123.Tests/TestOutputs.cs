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
        public void TestFormatNoneOK()
        {
            Mpg123 handle = new Mpg123();
            Assert.That(handle.FormatNone(), Is.EqualTo(Mpg123.Errors.OK));
        }

        [Test]
        public void TestFormatAllOK()
        {
            Mpg123 handle = new Mpg123();
            Assert.That(handle.FormatAll(), Is.EqualTo(Mpg123.Errors.OK));
        }
        [Test]
        public void TestFormatOK()
        {
            Mpg123 handle = new Mpg123();
            Assert.That(handle.Format(44100,2,0),Is.EqualTo(Mpg123.Errors.OK));
        }

        [Test]
        public void TestFormatSupportedNoEx()
        {
            Mpg123 handle = new Mpg123();
            //Mpg123.ChannelCount state = handle.IsFormatSupported(100, 20);
            Assert.That(() => handle.IsFormatSupported(100, 20), Throws.Nothing);
        }

        [Test]
        public void TestGetFormatOK()   //must fail because there isn't stream loaded
        {
            Mpg123 handle = new Mpg123();
            long rate = 0;
            int channels = 0, encoding = 0;
            Assert.That(handle.GetFormat(ref rate, ref channels, ref encoding), Is.Not.EqualTo(Mpg123.Errors.OK));
        }

        //TO DO: GET FORMAT TEST
        //CALL DEQUEUE AND WHEN HAVE RIGHT RETURN MESSAGE CALL GET FORMAT
        //THE SAME FOR GET FORMAT 2
    }

   
}
