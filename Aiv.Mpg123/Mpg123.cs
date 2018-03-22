using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace Aiv.Mpg123
{
    public class Mpg123 : IDisposable
    {
        public class ErrorException : Exception
        {
            public ErrorException(Errors error) : base(PlainStrError(error))
            {
            }
        }

        public enum Errors
        {
            OK = 0,
        }

        public enum Channels
        {
            LEFT = 1,
            RIGHT,
            LR
        }

        static private bool libraryInitialized;
        static public bool IsLibraryInitialized
        {
            get
            {
                return libraryInitialized;
            }
        }

        public static IEnumerable<string> Decoders
        {
            get
            {
                IntPtr decodersPtr = NativeMethods.NativeMpg123Decoders();
                int offset = 0;
                while (true)
                {
                    IntPtr decoderPtr = Marshal.ReadIntPtr(decodersPtr, offset);
                    if (decoderPtr == IntPtr.Zero)
                    {
                        yield break;
                    }
                    yield return Marshal.PtrToStringAnsi(decoderPtr);
                    offset += Marshal.SizeOf<IntPtr>();
                }
            }
        }

        public static string PlainStrError(Errors error)
        {
            IntPtr errorPtr = NativeMethods.NativeMpg123PlainStrError(error);
            if (errorPtr == IntPtr.Zero)
                return "unknown error";
            string errorMessage = Marshal.PtrToStringAnsi(errorPtr);
            return errorMessage;
        }

        static Mpg123()
        {
            Errors error = NativeMethods.NativeMpg123Init();
            if (error != Errors.OK)
                throw new ErrorException(error);
            libraryInitialized = true;
        }

        public bool HasValidHandle
        {
            get
            {
                return handle != IntPtr.Zero;
            }
        }

        protected IntPtr handle;

        public Mpg123(string decoder = null)
        {
            IntPtr decoderPtr = IntPtr.Zero;
            if (decoder != null)
            {
                decoderPtr = Marshal.StringToHGlobalAnsi(decoder);
            }
            int error = 0;
            handle = NativeMethods.NativeMpg123New(decoderPtr, ref error);
            if (decoderPtr != IntPtr.Zero)
                Marshal.FreeHGlobal(decoderPtr);
            if (handle == IntPtr.Zero)
                throw new ErrorException((Errors)error);
        }

        protected bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool isDisposing)
        {
            if (disposed)
                return;

            if (handle != IntPtr.Zero)
            {
                NativeMethods.NativeMpg123Delete(handle);
                handle = IntPtr.Zero;
            }

            if (isDisposing)
            {
                // cleanup dependancies
            }

            disposed = true;
        }

        ~Mpg123()
        {
            Dispose(false);
        }

        public bool Eq(Channels channel, int band, double val)
        {
            Errors error = NativeMethods.NativeMpg123Eq(handle, channel, band, val);
            if (error != Errors.OK)
                throw new ErrorException(error);

            return true;
        }

        public double GetEq(Channels channel, int band)
        {
            double ret = NativeMethods.NativeMpg123GetEq(handle, channel, band);
            return ret;
        }

        public bool ResetEq()
        {
            Errors error = NativeMethods.NativeMpg123ResetEq(handle);
            if (error != Errors.OK)
                throw new ErrorException(error);
            return true;
        }

        public bool Volume(double vol)
        {
            Errors error = NativeMethods.NativeMpg123Volume(handle,vol);
            if (error != Errors.OK)
                throw new ErrorException(error);
            return true;
        }

        public bool VolumeChange(double change)
        {
            Errors error = NativeMethods.NativeMpg123VolumeChange(handle, change);
            if (error != Errors.OK)
                throw new ErrorException(error);
            return true;
        }

        public bool GetVolume(ref double base_,ref double really,ref double rva_db)
        {
            Errors error = NativeMethods.NativeMpg123GetVolume(handle, ref base_, ref really, ref rva_db);
            if (error != Errors.OK)
                throw new ErrorException(error);
            return true;
        }
    }
}
