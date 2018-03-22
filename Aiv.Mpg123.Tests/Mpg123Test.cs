using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using System.Reflection;
using System.IO;

namespace Aiv.Mpg123.Tests
{
    [TestFixture]
    public class Mpg123Test
    {
        [Test]
        public void TestInitialization()
        {
            Assert.That(Mpg123.IsLibraryInitialized, Is.True);
        }

        [Test]
        public void TestPlainStrError()
        {

            Assert.That(Mpg123.PlainStrError(Mpg123.Errors.OK), Is.Not.Null);
        }

        [Test]
        public void TestPlainStrErrorWithInvalid()
        {

            Assert.That(Mpg123.PlainStrError((Mpg123.Errors)(-100000)), Is.Not.Null);
        }

        [Test]
        public void TestDecoders()
        {
            Assert.That(Mpg123.Decoders.ToArray(), Has.Length.GreaterThan(0));
        }

        [Test]
        public void TestInstantiate()
        {
            Mpg123 mpg123 = new Mpg123();
            Assert.That(mpg123.HasValidHandle, Is.True);
        }

        [Test]
        public void TestInstantiateWithInvalidDecoder()
        {
            Assert.That(() => new Mpg123("foobar"), Throws.TypeOf<Mpg123.ErrorException>());
        }

        [Test]
        public void TestInstantiateWithDecoder()
        {
            string decoder = Mpg123.Decoders.ToArray()[0];
            Mpg123 mpg123 = new Mpg123(decoder);
            Assert.That(mpg123.HasValidHandle, Is.True);
        }

        [Test]

        public void TestOpenPathNull()
        {
            Mpg123 mpg123 = new Mpg123();
            Assert.That(() => mpg123.Open(null), Throws.Nothing);
        }

        [Test]

        public void TestOpenPathInvalid()
        {
            string path = "test.mp3";
            Mpg123 mpg123 = new Mpg123();
            Assert.That(() => mpg123.Open(path), Throws.TypeOf<Mpg123.ErrorException>());
        }

        [Test]
        public void TestOpenPathValid()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = dirName + "/bensound-epic.mp3";
            Mpg123 mpg123 = new Mpg123();
            Assert.That(() => mpg123.Open(path), Throws.Nothing);
        }

        [Test]
        public void TestCloseNoOpen()
        {
            Mpg123 mpg123 = new Mpg123();
            Assert.That(() => mpg123.Close(), Throws.Nothing);
        }

        [Test]
        public void TestCloseOpen()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = dirName + "/bensound-epic.mp3";

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);
            Assert.That(() => mpg123.Close(), Throws.Nothing);
        }

        public void TestSetParamGetParam()
        {
            long setValue = 1000;
            double setFValue = 2000;
            long getValue = 0;
            double getFValue = 0;


            Mpg123 mpg123 = new Mpg123();
            mpg123.SetParam(Mpg123.Mpg123Params.MPG123_DECODE_FRAMES, setValue, setFValue);
            mpg123.GetParam(Mpg123.Mpg123Params.MPG123_DECODE_FRAMES, ref getValue, ref getFValue);

            Assert.That(getValue, Is.EqualTo(1000));
            Assert.That(getFValue, Is.EqualTo(2000));
        }
    }
}
