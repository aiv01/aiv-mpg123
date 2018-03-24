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
        public void TestStrError()
        {
            Mpg123 mpg123 = new Mpg123();
            Assert.That(mpg123.HasValidHandle, Is.True);
        }

        [Test]
        public void TestErrorCode()
        {
            Mpg123 mpg123 = new Mpg123();
            Assert.That(mpg123.HasValidHandle, Is.True);
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
        public void TestSupportedDecoders()
        {
            Assert.That(Mpg123.SupportedDecoders.ToArray(), Has.Length.GreaterThan(0));
        }

        [Test]
        public void TestSupportedDecodersValidLentgh()
        {
            var DecodersList = Mpg123.SupportedDecoders.ToArray();
            Assert.That(Mpg123.SupportedDecoders.ToArray(), Has.Length.LessThanOrEqualTo(DecodersList.Length));
        }

        [Test]
        public void TestGetDecoder()
        {
            string decoder = Mpg123.Decoders.ToArray()[0];

            Mpg123 mpg123 = new Mpg123(decoder);
            
            Assert.That(mpg123.Decoder, Is.EqualTo(decoder));
        }

        [Test]
        public void TestGetDecoderFalse()
        {
            string decoder = Mpg123.Decoders.ToArray()[0];
            string notDecoder = Mpg123.Decoders.ToArray()[1];

            Mpg123 mpg123 = new Mpg123(decoder);

            Assert.That(mpg123.Decoder, Is.Not.EqualTo(notDecoder));
        }

        [Test]
        public void TestSetDecoder()
        {
            Mpg123 mpg123 = new Mpg123();

            string decoder = Mpg123.Decoders.ToArray()[0];
            mpg123.Decoder = decoder;

            Assert.That(mpg123.Decoder, Is.EqualTo(decoder));
        }

        [Test]
        public void TestSetDecoderFalse()
        {
            Mpg123 mpg123 = new Mpg123();

            string decoder = Mpg123.Decoders.ToArray()[0];
            string notDecoder = Mpg123.Decoders.ToArray()[1];
            mpg123.Decoder = decoder;

            Assert.That(mpg123.Decoder, Is.Not.EqualTo(notDecoder));
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
            string path = Path.Combine(dirName, "bensound-epic.mp3");
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
            string path = Path.Combine(dirName, "bensound-epic.mp3");

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);
            Assert.That(() => mpg123.Close(), Throws.Nothing);
        }

        [Test]
        public void TestSetParamGetParam()
        {
            long setValue = 2;
            long getValue = 0;
            double getFValue = 0;

            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);

            mpg123.SetParam(Mpg123.Mpg123Params.MPG123_DOWNSPEED, setValue);
            mpg123.GetParam(Mpg123.Mpg123Params.MPG123_DOWNSPEED, ref getValue, ref getFValue);

            Assert.That(getValue, Is.EqualTo(2));
            Assert.That(getFValue, Is.EqualTo(0));
        }

        [Test]
        public void TestFeature()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = dirName + "/bensound-epic.mp3";

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);

            Assert.That(() => mpg123.Feature(Mpg123.Mpg123FeatureSet.MPG123_FEATURE_EQUALIZER), Throws.Nothing);
        }

        [Test]
        public void TestReadNullBuffer()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);
            uint done = 0;
            Assert.That(() => mpg123.Read(null, ref done), Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void TestReadNoOpen()
        {
            Mpg123 mpg123 = new Mpg123();
            uint done = 0;
            byte[] buffer = new byte[100];
            Assert.That(() => mpg123.Read(buffer, ref done), Throws.TypeOf<Mpg123.ErrorException>());
        }

        [Test]
        public void TestReadOpen()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");
            byte[] buffer = new byte[1000];
            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);
            uint done = 0;
            Assert.That(() => mpg123.Read(buffer, ref done), Throws.Nothing);
        }

        [Test]
        public void TestReadNewFormat()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);

            uint done = 0;
            byte[] buffer = new byte[1000];

            Assert.That(mpg123.Read(buffer, ref done), Is.EqualTo(Mpg123.Errors.NEW_FORMAT));
        }

        [Test]
        public void TestReadNoNewFormat()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);

            uint done = 0;
            byte[] buffer = new byte[1000];

            int b = 0, c = 0;
            long a = 0;
            mpg123.GetFormat(ref a, ref b, ref c);

            Assert.That(mpg123.Read(buffer, ref done), Is.EqualTo(Mpg123.Errors.OK));
        }

        [Test]
        public void TestReadDoneBytes()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-scifi.mp3");

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);
            
            uint done = 0;
            byte[] buffer = new byte[1000];

            while (mpg123.Read(buffer, ref done) != Mpg123.Errors.OK) { }
            Assert.That(done, Is.EqualTo(1000));
        }

        [Test]
        public void TestReadEmptyBuffer()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);

            uint done = 0;
            byte[] buffer = new byte[0];

            Mpg123.Errors error = Mpg123.Errors.OK;
            while (error == Mpg123.Errors.OK)
            {
                Assert.That(() => error = mpg123.Read(buffer, ref done), Throws.Nothing);
            }

            Assert.That(done, Is.EqualTo(0));
        }

        #region SEEKS_TESTS

        [Test]
        public void TestTellIsZero()
        {
            string decoder = Mpg123.Decoders.ToArray().FirstOrDefault();
            Mpg123 mpg123 = new Mpg123();

            long seekPositon = mpg123.Tell();

            Assert.That(seekPositon, Is.Zero);
        }

        [Test]
        public void TestTellFrameIsZero()
        {
            string decoder = Mpg123.Decoders.ToArray().FirstOrDefault();
            Mpg123 mpg123 = new Mpg123();

            long seekPositon = mpg123.TellFrame();

            Assert.That(seekPositon, Is.Zero);
        }

        [Test]
        public void TestTellStreamIsNegative()
        {
            string decoder = Mpg123.Decoders.ToArray().FirstOrDefault();
            Mpg123 mpg123 = new Mpg123();

            long seekPositon = mpg123.TellStream();

            Assert.That(seekPositon, Is.EqualTo(-1));
        }

        [Test]
        public void TestSeekInvalid()
        {
            string decoder = Mpg123.Decoders.ToArray().FirstOrDefault();
            Mpg123 mpg123 = new Mpg123();

            Assert.That(() => mpg123.Seek(10, SeekOrigin.Current), Throws.TypeOf<Mpg123.ErrorException>());
        }

        [Test]
        public void TestSeekFrameInvalid()
        {
            string decoder = Mpg123.Decoders.ToArray().FirstOrDefault();
            Mpg123 mpg123 = new Mpg123();

            Assert.That(() => mpg123.SeekFrame(5, SeekOrigin.Begin), Throws.TypeOf<Mpg123.ErrorException>());
        }

        #endregion
    }
}
