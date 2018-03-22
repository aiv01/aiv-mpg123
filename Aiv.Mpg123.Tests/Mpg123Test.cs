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
        public void TestStrError()
        {
            Mpg123 mpg123 = new Mpg123();
            Assert.That(Mpg123.StrError(mpg123.handle), Is.Not.Null);
        }

        [Test]
        public void TestErrorCode()
        {
            Mpg123 mpg123 = new Mpg123();
            Assert.That(Mpg123.ErrorCode(mpg123.handle), Is.Not.Null);
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

    }
}
