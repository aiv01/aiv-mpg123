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
        public void TestSetEqualizer()
        {
            Mpg123 mpg123 = new Mpg123();
            Assert.That(()=>mpg123.Eq(Mpg123.Channels.LEFT, 10, 10),Throws.Nothing);
        }
        [Test]
        public void TestSetEqualizerRedLight()
        {
            Mpg123 mpg123 = new Mpg123();
            int ErrorValue = 1000;
            Assert.That(() => mpg123.Eq(Mpg123.Channels.LEFT, ErrorValue, 20), Throws.TypeOf<Mpg123.ErrorException>());
        }
        [Test]
        public void TestGetEqualizer()
        {
            Mpg123 mpg123 = new Mpg123();
            mpg123.Eq(Mpg123.Channels.LR, 20, 20);
            double aspectedValue = 20;
            Assert.That(mpg123.GetEq(Mpg123.Channels.LR, 20), Is.EqualTo(aspectedValue));
        }
        [Test]
        public void TestGetEqualizerRedLight()
        {
            Mpg123 mpg123 = new Mpg123();
            mpg123.Eq(Mpg123.Channels.LR, 20, 30);
            double value = 20;
            Assert.That(mpg123.GetEq(Mpg123.Channels.LR, 20), Is.Not.EqualTo(value));
        }
        [Test]
        public void TestResetEqualizer()
        {
            Mpg123 mpg123 = new Mpg123();
            Assert.That(() => mpg123.ResetEq(), Throws.Nothing);
        }

        [Test]
        public void TestResetEqualizerRedLight()
        {
            Mpg123 mpg123 = new Mpg123();
            mpg123.Dispose();
            Assert.That(() => mpg123.ResetEq(), Throws.TypeOf<Mpg123.ErrorException>());
        }

        [Test]
        public void TestVolume()
        {
            Mpg123 mpg123 = new Mpg123();
            Assert.That(() => mpg123.Volume(20), Throws.Nothing);

            double base_ = 0;
            double really = 0;
            double rva_db = 0;

            mpg123.GetVolume(ref base_, ref really, ref rva_db);
            Assert.That(base_, Is.EqualTo(20));
            Assert.That(really, Is.EqualTo(20));
        }

        [Test]
        public void TestVolumeRedLight()
        {
            Mpg123 mpg123 = new Mpg123();
            double base_ = 0;
            double really = 0;
            double rva_db = 0;
            Assert.That(() => mpg123.Volume(20), Throws.Nothing);
            mpg123.GetVolume(ref base_, ref really, ref rva_db);
            Assert.That(base_, Is.Not.EqualTo(0));
        }
        [Test]
        public void TestVolumeChange()
        {
            Mpg123 mpg123 = new Mpg123();
            mpg123.Volume(20);

            Assert.That(() => mpg123.VolumeChange(10), Throws.Nothing);

            double expectedVolume = 30;

            double base_ = 0;
            double really = 0;
            double rva_db = 0;
            mpg123.GetVolume(ref base_, ref really, ref rva_db);

            Assert.That(base_, Is.EqualTo(expectedVolume));
        }

        [Test]
        public void TestVolumeChangeRedLight()
        {
            Mpg123 mpg123 = new Mpg123();
            mpg123.Volume(50);
            mpg123.VolumeChange(25);

            double base_ = 0;
            double really = 0;
            double rva_db = 0;
            mpg123.GetVolume(ref base_, ref really, ref rva_db);

            Assert.That(base_, Is.Not.EqualTo(50));
        }

        [Test]
        public void TestGetVolume()
        {
            Mpg123 mpg123 = new Mpg123();
            mpg123.Volume(50);
            double base_ = 0;
            double really = 0;
            double rva_db = 0;
            Assert.That(() => mpg123.GetVolume(ref base_, ref really, ref rva_db), Throws.Nothing);
            Assert.That(base_, Is.EqualTo(50));
            Assert.That(really, Is.EqualTo(50));
        }

        [Test]
        public void TestGetVolumeRedLight()
        {
            Mpg123 mpg123 = new Mpg123();
            mpg123.Volume(50);
            double base_ = 20;
            double really = 20;
            double rva_db = 0;
            Assert.That(() => mpg123.GetVolume(ref base_, ref really, ref rva_db), Throws.Nothing);
            Assert.That(base_, Is.Not.EqualTo(20));
            Assert.That(really, Is.Not.EqualTo(20));
        }

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

        [Test]
        public void TestDecodeFrameNotOpen()
        {
            Mpg123 mpg123 = new Mpg123();

            byte[] buffer = null;
            int num = 0;
            uint bytes = 0;

            Assert.That(() => mpg123.DecodeFrame(ref num, ref buffer, ref bytes), Throws.TypeOf<Mpg123.ErrorException>());
        }

        [Test]
        public void TestDecodeFrameOpen()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);

            byte[] buffer = null;
            int num = 0;
            uint bytes = 0;

            Mpg123.Errors error = Mpg123.Errors.OK;
            while (error == Mpg123.Errors.OK)
            {
                Assert.That(() => error = mpg123.DecodeFrame(ref num, ref buffer, ref bytes), Throws.Nothing);
            }
        }

        [Test]
        public void TestDecodeFrameBufferNotNull()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);

            byte[] buffer = null;
            int num = 0;
            uint bytes = 0;

            while (mpg123.DecodeFrame(ref num, ref buffer, ref bytes) != Mpg123.Errors.OK) { }

            Assert.That(buffer, Is.Not.EqualTo(null));
        }

        [Test]
        public void TestDecodeFrameBufferNotEmpty()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);

            byte[] buffer = null;
            int num = 0;
            uint bytes = 0;

            while (mpg123.DecodeFrame(ref num, ref buffer, ref bytes) != Mpg123.Errors.OK) { }

            Assert.That(buffer, Is.Not.Empty);
        }

        [Test]
        public void TestDecodeFrameBufferLength0()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);

            byte[] buffer = null;
            int num = 0;
            uint bytes = 0;

            while (mpg123.DecodeFrame(ref num, ref buffer, ref bytes) != Mpg123.Errors.OK) { }

            Assert.That(buffer.Length, Is.EqualTo(bytes));
        }

        [Test]
        public void TestDecodeFrameBufferLength1()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-scifi.mp3");

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);

            byte[] buffer = null;
            int num = 0;
            uint bytes = 0;

            int b = 0, c = 0;
            long a = 0;
            mpg123.GetFormat(ref a, ref b, ref c);

            mpg123.DecodeFrame(ref num, ref buffer, ref bytes);
            Assert.That(buffer.Length, Is.EqualTo(188));

            mpg123.DecodeFrame(ref num, ref buffer, ref bytes);
            Assert.That(buffer.Length, Is.EqualTo(4608));
        }

        [Test]
        public void TestDecodeFrameNewFormat()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);

            byte[] buffer = null;
            int num = 0;
            uint bytes = 0;

            Assert.That(mpg123.DecodeFrame(ref num, ref buffer, ref bytes), Is.EqualTo(Mpg123.Errors.NEW_FORMAT));
        }

        [Test]
        public void TestDecodeFrameNoNewFormat()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);

            byte[] buffer = null;
            int num = 0;
            uint bytes = 0;

            int b = 0, c = 0;
            long a = 0;
            mpg123.GetFormat(ref a, ref b, ref c);

            Assert.That(mpg123.DecodeFrame(ref num, ref buffer, ref bytes), Is.EqualTo(Mpg123.Errors.OK));
        }

        [Test]
        public void TestDecodeFrameDone()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);

            byte[] buffer = null;
            int num = 0;
            uint bytes = 0;

            int b = 0, c = 0;
            long a = 0;
            mpg123.GetFormat(ref a, ref b, ref c);

            Mpg123.Errors error = Mpg123.Errors.OK;
            while (error == Mpg123.Errors.OK)
            {
                error = mpg123.DecodeFrame(ref num, ref buffer, ref bytes);
            }

            Assert.That(error, Is.EqualTo(Mpg123.Errors.DONE));
        }

        [Test]
        public void TestDecodeFrameNumFrames()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-scifi.mp3");

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);

            byte[] buffer = null;
            int num = 0;
            uint bytes = 0;

            int b = 0, c = 0;
            long a = 0;
            mpg123.GetFormat(ref a, ref b, ref c);

            Mpg123.Errors error = Mpg123.Errors.OK;
            while (error == Mpg123.Errors.OK)
            {
                error = mpg123.DecodeFrame(ref num, ref buffer, ref bytes);
            }

            Assert.That(num, Is.GreaterThan(0));
        }

        [Test]
        public void TestOpenFeedNoOpen()
        {
            Mpg123 mpg123 = new Mpg123();
            Assert.That(() => mpg123.OpenFeed(), Throws.Nothing);
        }

        [Test]
        public void TestOpenFeedOpen()
        {
            Mpg123 mpg123 = new Mpg123();
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");
            mpg123.Open(path);

            Assert.That(() => mpg123.OpenFeed(), Throws.Nothing);
        }

        [Test]
        public void TestDecodeFrameNeedMore()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");

            Mpg123 mpg123 = new Mpg123();

            mpg123.OpenFeed();

            byte[] fileBytes = File.ReadAllBytes(path);
            byte[] buffer = new byte[8];
            Array.Copy(fileBytes, buffer, 8);

            int num = 0;
            uint bytes = 0;

            int b = 0, c = 0;
            long a = 0;
            mpg123.GetFormat(ref a, ref b, ref c);

            Mpg123.Errors error = Mpg123.Errors.OK;
            Assert.That(() => error = mpg123.DecodeFrame(ref num, ref buffer, ref bytes), Throws.Nothing);

            Assert.That(error, Is.EqualTo(Mpg123.Errors.NEED_MORE));
        }

        [Test]
        public void TestDecodeFrameNumFramesTotal()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);

            byte[] buffer = null;
            int num = 1;
            uint bytes = 0;

            Mpg123.Errors error = Mpg123.Errors.OK;
            while (error != Mpg123.Errors.DONE)
            {
                error = mpg123.DecodeFrame(ref num, ref buffer, ref bytes);
            }

            Assert.That(num, Is.EqualTo(6834));
        }

        [Test]
        public void TestReadNeedMore()
        {
            Mpg123 mpg123 = new Mpg123();

            mpg123.OpenFeed();

            byte[] buffer = new byte[8];

            uint done = 0;

            int b = 0, c = 0;
            long a = 0;
            mpg123.GetFormat(ref a, ref b, ref c);

            Mpg123.Errors error = Mpg123.Errors.OK;
            Assert.That(() => error = mpg123.Read(buffer, ref done), Throws.Nothing);

            Assert.That(error, Is.EqualTo(Mpg123.Errors.NEED_MORE));
        }

        [Test]
        public void TestFeed()
        {
            Mpg123 mpg123 = new Mpg123();

            mpg123.OpenFeed();

            byte[] buffer = new byte[8];

            Assert.That(() => mpg123.Feed(buffer), Throws.Nothing);
        }

        [Test]
        public void TestFeedReadNeedMore()
        {
            Mpg123 mpg123 = new Mpg123();

            mpg123.OpenFeed();

            byte[] inBuffer = new byte[8];
            byte[] outBuffer = new byte[8];
            uint done = 0;

            mpg123.Feed(inBuffer);
            Assert.That(mpg123.Read(outBuffer, ref done), Is.EqualTo(Mpg123.Errors.NEED_MORE));
        }

        [Test]
        public void TestFeedReadNoNeedMore()
        {
            Mpg123 mpg123 = new Mpg123();

            mpg123.OpenFeed();

            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");

            byte[] inBuffer = File.ReadAllBytes(path);
            byte[] outBuffer = new byte[inBuffer.Length];
            uint done = 0;

            mpg123.Feed(inBuffer);

            long a = 0;
            int b = 0, c = 0;
            mpg123.GetFormat(ref a, ref b, ref c);

            Assert.That(mpg123.Read(outBuffer, ref done), Is.EqualTo(Mpg123.Errors.OK));
            Assert.That(done, Is.EqualTo(inBuffer.Length));
        }

        [Test]
        public void TestDecodeNullInput()
        {
            Mpg123 mpg123 = new Mpg123();

            mpg123.OpenFeed();

            byte[] outBuffer = new byte[8];
            uint done = 0;

            Assert.That(() => mpg123.Decode(null, 0, outBuffer, 8, ref done), Throws.Nothing);
        }

        [Test]
        public void TestDecodeNullOutput()
        {
            Mpg123 mpg123 = new Mpg123();

            mpg123.OpenFeed();

            byte[] inBuffer = new byte[8];
            uint done = 0;

            Assert.That(() => mpg123.Decode(inBuffer, 8, null, 0, ref done), Throws.Nothing);
        }
        [Test]
        public void TestDecodeNullInputAndOutput()
        {
            Mpg123 mpg123 = new Mpg123();

            mpg123.OpenFeed();

            uint done = 0;

            Assert.That(() => mpg123.Decode(null, 0, null, 0, ref done), Throws.Nothing);
        }

        [Test]
        public void TestDecodeNoOpen()
        {
            Mpg123 mpg123 = new Mpg123();

            uint done = 0;

            Assert.That(() => mpg123.Decode(null, 0, null, 0, ref done), Throws.TypeOf<Mpg123.ErrorException>());
        }

        [Test]
        public void TestDecodeNeedMore()
        {
            Mpg123 mpg123 = new Mpg123();

            mpg123.OpenFeed();

            byte[] inBuffer = new byte[8];
            uint done = 0;

            Assert.That(mpg123.Decode(inBuffer, 8, null, 0, ref done), Is.EqualTo(Mpg123.Errors.NEED_MORE));
        }

        [Test]
        public void TestDecodeNewFormat()
        {
            Mpg123 mpg123 = new Mpg123();

            mpg123.OpenFeed();

            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");
            byte[] inBuffer = File.ReadAllBytes(path);
            uint done = 0;

            Assert.That(mpg123.Decode(inBuffer, (uint)inBuffer.Length, null, 0, ref done), Is.EqualTo(Mpg123.Errors.NEW_FORMAT));
        }

        [Test]
        public void TestDecodeDoneBytes()
        {
            Mpg123 mpg123 = new Mpg123();

            mpg123.OpenFeed();

            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-epic.mp3");
            byte[] inBuffer = File.ReadAllBytes(path);
            uint done = 0;

            mpg123.Decode(inBuffer, (uint)inBuffer.Length, null, 0, ref done);

            byte[] outBuffer = new byte[100];

            while (mpg123.Decode(null, 0, outBuffer, 100, ref done) != Mpg123.Errors.NEED_MORE)
            {
                Assert.That(done, Is.EqualTo(100));
            }
        }

        [Test]
        public void TestDecodeInconsistentInputSizeMajor()
        {
            Mpg123 mpg123 = new Mpg123();

            mpg123.OpenFeed();

            byte[] inBuffer = new byte[100];
            uint done = 0;

            Assert.That(() => mpg123.Decode(inBuffer, 300, null, 0, ref done), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void TestDecodeInconsistentInputSizeMinor()
        {
            Mpg123 mpg123 = new Mpg123();

            mpg123.OpenFeed();

            byte[] inBuffer = new byte[100];
            uint done = 0;

            Assert.That(() => mpg123.Decode(inBuffer, 50, null, 0, ref done), Throws.Nothing);
        }

        [Test]
        public void TestDecodeInconsistentOutputSizeMajor()
        {
            Mpg123 mpg123 = new Mpg123();

            mpg123.OpenFeed();

            byte[] outBuffer = new byte[100];
            uint done = 0;

            Assert.That(() => mpg123.Decode(null, 0, outBuffer, 300, ref done), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void TestDecodeInconsistentOutputSizeMinor()
        {
            Mpg123 mpg123 = new Mpg123();

            mpg123.OpenFeed();

            byte[] outBuffer = new byte[100];
            uint done = 0;

            Assert.That(() => mpg123.Decode(null, 0, outBuffer, 50, ref done), Throws.Nothing);
        }

        [Test]
        public void TestFramePosNoParsedFrame()
        {
            Mpg123 mpg123 = new Mpg123();

            Assert.That(mpg123.FramePos(), Is.EqualTo(-1));
        }


        [Test]
        public void TestFramePosParsedFrame()
        {
            string dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dirName, "bensound-scifi.mp3");

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open(path);

            byte[] buffer = null;
            int num = 0;
            uint bytes = 0;

            mpg123.DecodeFrame(ref num, ref buffer, ref bytes);

            Assert.That(mpg123.FramePos(), Is.GreaterThan(0));
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
