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
        public void TestDriversOK() {
            Out123 handle = new Out123();
            Assert.That(handle.Drivers().ToArray(), Has.Length.GreaterThan(0));
            /*
            try { 
            } catch (Out123.ErrorException e) {
                //Console.WriteLine(handle.LastError());
                //Console.WriteLine("CURRENT: " + e);
                Assert.Fail();
            }
            */
        }
        #endregion

        #region API Tests: Open
        [Test]
        public void TestOpenWithDefaultOK() {
            //Console.WriteLine(System.IO.Path.GetFullPath("."));
            //Tryed to absolute point at plugin folder to understand if the error was due to releative path issue.
            Out123 handle = new Out123("C:\\_fdf\\projects\\workspace_aiv\\LEZ39_20180322\\aiv-mpg123_git\\Aiv.Mpg123.Tests\\bin\\Debug\\plugins");
            handle.Open();
            Assert.AreEqual(Out123.Errors.OK, handle.LastErrorString());
        }
        #endregion

        #region API Tests: Driver Info
        [Test]
        public void TestDriverInfoOK() {
            Out123 handle = new Out123();
            handle.DriverInfo();
            Assert.AreEqual(Out123.Errors.OK, handle.LastErrorCode());
        }
        #endregion
    }
}
