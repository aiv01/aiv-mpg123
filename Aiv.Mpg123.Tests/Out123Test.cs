using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace Aiv.Mpg123.Tests
{
    [TestFixture]
    public class Out123Test {
        [Test]
        public void TestCreationOK() {
            Out123 handle = new Out123();
            Assert.True(true, "No exception is launched!");
        }
        /* Non testabile
        [Test]
        public void TestCreationFailure() {
            Assert.That(() => new Out123("unexistentPluginsFolder"), Throws.TypeOf<Out123.ErrorException>());
        }
        */

        [Test]
        public void TestDelete() {
            Out123 handle = new Out123();
            handle.Dispose();
            Assert.True(true, "Api Call ended succesfully!!");
        }

        //FDF: Capire come gestire lastError e LastErrorCode

        [Test]
        public void TestLastErrorNoProblem() {
            Out123 handle = new Out123();
            string result = handle.LastErrorString();
            Assert.AreEqual("no problem", result);
        }

        [Test]
        public void TestLastErrorCodeOK() {
            Out123 handle = new Out123();
            Out123.Errors result = handle.LastErrorCode();
            Assert.AreEqual(Out123.Errors.OK, result);
        }

        [Test]
        public void TestSetBufferOK() {
            Out123 handle = new Out123();
            handle.SetBufferSize(10);
            Assert.AreEqual(Out123.Errors.OK,  handle.LastErrorCode());
        }

        #region API Tests: SetParam
        [Test]
        public void TestSetParamIntOK() {
            Out123 handle = new Out123();
            handle.SetParamInt(Out123.Params.FLAGS, 1);
            Assert.AreEqual(Out123.Errors.OK, handle.LastErrorCode());
        }

        [Test]
        public void TestSetParamFloatOK() {
            Out123 handle = new Out123();
            handle.SetParamFloat(Out123.Params.FLAGS, 1.0d);
            Assert.AreEqual(Out123.Errors.OK, handle.LastErrorCode());
        }

        [Test]
        public void TestSetParamStringOK() {
            Out123 handle = new Out123();
            handle.SetParamString(Out123.Params.BINDIR, "myDir");
            Assert.AreEqual(Out123.Errors.OK, handle.LastErrorCode());
        }
        #endregion

        #region API Tests: GetParam
        [Test]
        public void TestGetParamIntOK() {
            Out123 handle = new Out123();
            handle.SetParamInt(Out123.Params.FLAGS, 2);
            long result = handle.GetParamInt(Out123.Params.FLAGS);
            Assert.AreEqual(2, result);
        }
        [Test]
        public void TestGetParamFloatOK() {
            Out123 handle = new Out123();
            handle.SetParamFloat(Out123.Params.GAIN, 5.0);
            double result = handle.GetParamFloat(Out123.Params.GAIN);
            Assert.AreEqual(5, result);
            //Test should work. Strange behavior just with the double value. Maybe Bug in Out123?!?!
        }
        [Test]
        public void TestGetParamStringOK() {
            Out123 handle = new Out123();
            handle.SetParamString(Out123.Params.BINDIR, "mydir");
            string result = handle.GetParamString(Out123.Params.BINDIR);
            Assert.AreEqual("mydir", result);
        }
        #endregion

        #region API Tests: CopyParamFrom
        [Test]
        public void TestCopyParamFromOK() {
            Out123 handle = new Out123();
            handle.SetParamInt(Out123.Params.FLAGS, 0);
            Out123 other = new Out123();
            other.SetParamInt(Out123.Params.FLAGS, 1);
            handle.CopyParamFrom(other);
            Assert.AreEqual(1, handle.GetParamInt(Out123.Params.FLAGS));
        }
        #endregion

        #region API Tests: Drivers
        [Test]
        public void TestDriversOnOutNotOpenedGoesInException() {
            Out123 handle = new Out123();
            Assert.That(() => handle.Drivers().ToArray(), Throws.TypeOf<Out123.ErrorException>());
            // Misleading exception. The call goes in error but the error message returned is "OK No Problem" 
        }
        #endregion

        #region API Tests: Open
        [Test]
        public void TestOpenWithDefaultOK() {
            Out123 handle = new Out123("invalid-plugin-path");            
            Assert.That(() => handle.Open(), Throws.TypeOf<Out123.ErrorException>());
        }
        #endregion

        #region API Tests: Driver Info
        [Test]
        public void TestGetDriverInfoOnOutNotOpenedGoesInException() {
            Out123 handle = new Out123();
            Assert.That(() => handle.GetDriverInfo(), Throws.TypeOf<Out123.ErrorException>());
        }
        #endregion

        #region API Tests: Close
        [Test]
        public void TestCloseHandleNotOpenedOK() {
            Out123 handle = new Out123();
            handle.Close();
            Assert.AreEqual(Out123.Errors.OK, handle.LastErrorCode());
        }
        #endregion

        #region API Tests: Encodings
        [Test]
        public void TestEncodingsOnOutNotOpenedGoesInException() {
            Out123 handle = new Out123();
            Assert.That(() => handle.EncodingsFor(44100, 2), Throws.TypeOf<Out123.ErrorException>());
        }
        #endregion

        #region API Tests: EncodingSize
        [Test]
        public void TestEncodingSizeForInvalidEncoding() {
            Out123 handle = new Out123();
            int result = handle.EncodingSize(0);
            Assert.AreEqual(0, result);
        }
        #endregion

        #region API Tests: Pause
        [Test]
        public void TestPauseOK() {
            Out123 handle = new Out123();
            handle.Pause();
            Assert.AreEqual(Out123.Errors.OK, handle.LastErrorCode());
        }
        #endregion

        #region API Tests: Continue
        [Test]
        public void TestContinueOK() {
            Out123 handle = new Out123();
            handle.Continue();
            Assert.AreEqual(Out123.Errors.OK, handle.LastErrorCode());
        }
        #endregion

        #region API Tests: Stop
        [Test]
        public void TestStopOK() {
            Out123 handle = new Out123();
            handle.Stop();
            Assert.AreEqual(Out123.Errors.OK, handle.LastErrorCode());
        }
        #endregion

        #region API Tests: Drop
        [Test]
        public void TestDropOK() {
            Out123 handle = new Out123();
            handle.Drop();
            Assert.AreEqual(Out123.Errors.OK, handle.LastErrorCode());
        }
        #endregion

        #region API Tests: Drain
        [Test]
        public void TestDrainOK() {
            Out123 handle = new Out123();
            handle.Drain();
            Assert.AreEqual(Out123.Errors.OK, handle.LastErrorCode());
        }

        [Test]
        public void TestNDrainSomeBytesOK() {
            Out123 handle = new Out123();
            handle.Drain(10);
            Assert.AreEqual(Out123.Errors.OK, handle.LastErrorCode());
        }
        #endregion

        #region API Tests: Buffered
        [Test]
        public void TestBufferedOK() {
            Out123 handle = new Out123();
            ulong result = handle.Buffered();
            Assert.AreEqual(0, result);
        }
        #endregion

        #region API Tests: GetFormat
        [Test]
        public void TestGetFormatOnNotOpenedOutGoesInException() {
            Out123 handle = new Out123();
            Assert.That(() => handle.GetFormat(), Throws.TypeOf<Out123.ErrorException>());
        }
        #endregion

    }
}
