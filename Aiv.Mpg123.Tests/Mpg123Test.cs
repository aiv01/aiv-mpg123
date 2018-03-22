using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

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
        public void TestSetEqualizer()
        {
            Mpg123 mpg123 = new Mpg123();
            Assert.That(mpg123.Eq(Mpg123.Channels.LEFT, 10, 10), Is.True);
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
            Assert.That(mpg123.ResetEq(), Is.True);
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
            Assert.That(mpg123.Volume(20), Is.True);

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
            Assert.That(mpg123.Volume(20), Is.True);
            mpg123.GetVolume(ref base_, ref really, ref rva_db);
            Assert.That(base_, Is.Not.EqualTo(0));
        }
        [Test]
        public void TestVolumeChange()
        {
            Mpg123 mpg123 = new Mpg123();
            mpg123.Volume(20);

            Assert.That(mpg123.VolumeChange(10), Is.True);

            double expectedVolume = 30;

            double base_ = 0;
            double really = 0;
            double rva_db = 0;
            mpg123.GetVolume(ref base_, ref really, ref rva_db);

            Assert.That(base_,Is.EqualTo(expectedVolume));
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
            Assert.That(mpg123.GetVolume(ref base_, ref really, ref rva_db), Is.True);
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
            Assert.That(mpg123.GetVolume(ref base_, ref really, ref rva_db), Is.True);
            Assert.That(base_, Is.Not.EqualTo(20));
            Assert.That(really, Is.Not.EqualTo(20));
        }
    }
}
