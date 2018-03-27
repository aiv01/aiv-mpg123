using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;


namespace Aiv.Mpg123 {
    public class Out123 : IDisposable {
        public class ErrorException : Exception {
            public ErrorException(Out123 handle) : base(LastErrorFrom(handle)) {
            }

            public ErrorException(Errors error) : base(PlainStrError(error)) {
            }
            public ErrorException(String error) : base(error) {
            }
        }
        private static string LastErrorFrom(Out123 handle) {
            return "[" + handle.LastErrorCode() + "]["+ handle.LastErrorString() + "]";
        }


        public static string PlainStrError(Errors error) {
            IntPtr errorPtr = Out123NativeMethods.PlainStrError(error);
            if (errorPtr == IntPtr.Zero)
                return "unknown error";
            string errorMessage = Marshal.PtrToStringAnsi(errorPtr);
            return errorMessage;
        }

        public enum Errors {
            ERR = -1,
            OK = 0,
            DOOM, BAD_DRIVER_NAME,
            BAD_DRIVER, NO_DRIVER,
            NOT_LIVE, DEV_PLAY,
            DEV_OPEN, BUFFER_ERROR,
            MODULE_ERROR, ARG_ERROR,
            BAD_PARAM, SET_RO_PARAM,
            BAD_HANDLE, ERRCOUNT
        }

        public enum Params
        {
            FLAGS = 1, 
            PRELOAD,
            GAIN,
            VERBOSE,
            DEVICEBUFFER,
            PROPFLAGS,
            NAME,
            BINDIR
        };

        public class Driver {
            public String name { get; private set; }
            public String desc { get; private set; }
            public Driver(String aName, String aDesc) {
                name = aName;
                desc = aDesc;
            }

            public override string ToString() {
                return "Driver {" + name + ", " + desc + "}";
            }
        }


        private IntPtr handle;
        private bool isDisposed;
        
        public Out123(string pluginFolder = "./plugins") {
            System.Environment.SetEnvironmentVariable("MPG123_MODDIR", pluginFolder);
            handle = Out123NativeMethods.New();
            if (handle == IntPtr.Zero) {
                System.Environment.SetEnvironmentVariable("MPG123_BINDIR", ""); //remove env variable
                throw new Out123.ErrorException("Unable to initialize lib Out123");
            }    
        }

        ~Out123() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool isDisposingManually) {
            if (isDisposed)
                return;

            if (handle != IntPtr.Zero) {
                Out123NativeMethods.Del(handle);
                handle = IntPtr.Zero;
            }

            if (isDisposingManually) {
                // cleanup dependancies
            }

            isDisposed = true;
        }

        public string LastErrorString() {
            IntPtr errorPtr = Out123NativeMethods.StrError(handle);
            Console.WriteLine("2: " + handle.ToInt64());
            return Marshal.PtrToStringAnsi(errorPtr);
        }

        public Out123.Errors LastErrorCode() {
            return Out123NativeMethods.ErrCode(handle);
        }

        public void SetBufferSize(ulong bufferSize) {
            Out123.Errors error = Out123NativeMethods.SetBuffer(handle, new UIntPtr(bufferSize));
            if (error != Out123.Errors.OK)
                throw new Out123.ErrorException(error);
        }

        public void SetParamInt(Out123.Params aParam, long value) {
            Out123.Errors error = Out123NativeMethods.Param(handle, aParam, new IntPtr(value), 0, IntPtr.Zero);
            if (error != Out123.Errors.OK)
                throw new Out123.ErrorException(error);
        }

        public void SetParamFloat(Out123.Params aParam, double value) {
            Out123.Errors error = Out123NativeMethods.Param(handle, aParam, IntPtr.Zero, value, IntPtr.Zero);
            if (error != Out123.Errors.OK)
                throw new Out123.ErrorException(error);
        }

        public void SetParamString(Out123.Params aParam, string value) {
            IntPtr sValuePtr = IntPtr.Zero;
            if (sValuePtr != null) {
                sValuePtr = Marshal.StringToHGlobalAnsi(value);
            }
            Out123.Errors result = Out123NativeMethods.Param(handle, aParam, IntPtr.Zero, 0, sValuePtr);
            if (sValuePtr != IntPtr.Zero)
                Marshal.FreeHGlobal(sValuePtr);
        }

        public long GetParamInt(Out123.Params aParam) {
            long result = 0;
            double null_fValue = 0; // not interested in float value because want the int value.
            IntPtr null_sValue = IntPtr.Zero;
            Out123.Errors error = Out123NativeMethods.GetParam(handle, aParam, ref result, ref null_fValue, null_sValue);
            if (error != Out123.Errors.OK)
                throw new Out123.ErrorException(error);
            return result;
        }

        public double GetParamFloat(Out123.Params aParam) {
            long null_value = 0; // not interested in int value because want the doule value.
            double result = 0;
            IntPtr null_sValue = IntPtr.Zero;
            Out123.Errors error = Out123NativeMethods.GetParam(handle, aParam, ref null_value, ref result, null_sValue);
            Console.WriteLine(null_value);
            if (error != Out123.Errors.OK)
                throw new Out123.ErrorException(this);
            return result;
        }

        public string GetParamString(Out123.Params aParam) {
            long null_value = 0; // not interested in int value because want the string value.
            double null_fvalue = 0; // not interested in double value because want the string value.
            IntPtr strPtr = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>());
            Out123.Errors error = Out123NativeMethods.GetParam(handle, aParam, ref null_value, ref null_fvalue, strPtr);
            if (error != Out123.Errors.OK)
                throw new Out123.ErrorException(error);

            IntPtr subPtr = Marshal.ReadIntPtr(strPtr);
            if (subPtr == IntPtr.Zero) {
                Marshal.FreeHGlobal(strPtr);
                throw new Out123.ErrorException("unknow error");
            } else {
                string result = Marshal.PtrToStringAnsi(subPtr);
                Marshal.FreeHGlobal(strPtr);
                return result;
            }
        }

        public void CopyParamFrom(Out123 other) {
            Out123.Errors error = Out123NativeMethods.ParamFrom(handle, other.handle);
            if (error != Out123.Errors.OK)
                throw new Out123.ErrorException(error);
        }

        public IEnumerable<Out123.Driver> Drivers() {
            IntPtr namesPtr = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>());
            IntPtr descsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>());

            Out123.Errors error = Out123NativeMethods.Drivers(handle, namesPtr, descsPtr);
            Console.WriteLine("1: " + handle.ToInt64());
            if (error != Out123.Errors.OK) {
                Marshal.FreeHGlobal(namesPtr);
                Marshal.FreeHGlobal(descsPtr);
                Console.WriteLine(error);
                Console.WriteLine("[" + LastErrorString() + "]");
                //throw new Out123.ErrorException(this);
                yield break;
            }

            IntPtr namesPtrDeref = Marshal.ReadIntPtr(namesPtr);
            IntPtr descsPtrDeref = Marshal.ReadIntPtr(descsPtr);
            int offset = 0;

            while (true) {
                IntPtr namePtr = Marshal.ReadIntPtr(namesPtrDeref, offset);
                IntPtr descPtr = Marshal.ReadIntPtr(descsPtrDeref, offset);
                if (namePtr == IntPtr.Zero) {
                    yield break;
                }

                String name = Marshal.PtrToStringAnsi(namePtr);
                String desc = Marshal.PtrToStringAnsi(descPtr);
                Out123.Driver driver = new Out123.Driver(name, desc);

                yield return driver;
                offset += Marshal.SizeOf<IntPtr>();
            }
        }

        public void Open(string driver = null, string device = null) {
            IntPtr driverPtr = IntPtr.Zero;
            IntPtr devicePtr = IntPtr.Zero;
            if (driver != null)
                driverPtr = Marshal.StringToHGlobalAnsi(driver);
            if (device != null)
                devicePtr = Marshal.StringToHGlobalAnsi(device);

            Out123.Errors error = Out123NativeMethods.Open(handle, driverPtr, devicePtr);
            if (driverPtr != IntPtr.Zero)
                Marshal.FreeHGlobal(driverPtr);
            if (devicePtr != IntPtr.Zero)
                Marshal.FreeHGlobal(devicePtr);

            if (error != Out123.Errors.OK)
                throw new Out123.ErrorException(this);
        }

        public void DriverInfo() {
            IntPtr driverPtr = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>());
            IntPtr devicePtr = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>());

            Out123.Errors error = Out123NativeMethods.DriverInfo(handle, driverPtr, devicePtr);
            Console.WriteLine(error);
            Console.WriteLine(LastErrorCode());
            Console.WriteLine(LastErrorString());

            if (error != Out123.Errors.OK)
                throw new Out123.ErrorException(error);
            /*
            IntPtr driver = Marshal.ReadIntPtr(driverPtr);
            IntPtr device = Marshal.ReadIntPtr(devicePtr);

            Console.WriteLine(driver.ToString());
            Console.WriteLine(device.ToString());
            */
        }

        public void Start(long rate, int channels, int encoding) {
            Out123.Errors error = Out123NativeMethods.Start(handle, new IntPtr(rate), channels, encoding);
            if (error != Out123.Errors.OK)
                throw new Out123.ErrorException(this);
        }

        public ulong Play(byte[] buffer) {
            IntPtr bufferPtr = Marshal.AllocHGlobal(Marshal.SizeOf<byte>() * buffer.Length);
            Marshal.Copy(buffer, 0, bufferPtr, buffer.Length);
            UIntPtr size = Out123NativeMethods.Play(handle, bufferPtr, new UIntPtr((ulong)buffer.Length));

            Marshal.FreeHGlobal(bufferPtr);
            return size.ToUInt64();
        }


    }
}
