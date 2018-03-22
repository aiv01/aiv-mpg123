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
            DONE = -12,
            NEW_FORMAT = -11,
            NEED_MORE = -10,
            ERR = -1,
            OK = 0,
            BAD_OUTFORMAT = 1,
            BAD_CHANNEL = 2,
            BAD_RATE = 3,
            ERR_16TO8TABLE = 4,
            BAD_PARAM = 5,
            BAD_BUFFER = 6,
            OUT_OF_MEM = 7,
            NOT_INITIALIZED,
            BAD_DECODER = 9,
            BAD_HANDLE = 10,
            NO_BUFFERS = 11,
            BAD_RVA = 12,
            NO_GAPLESS = 13,
            NO_SPACE = 14,
            BAD_TYPES = 15,
            BAD_BAND = 16,
            ERR_NULL = 17,
            ERR_READER = 18,
            NO_SEEK_FROM_END = 19,
            BAD_WHENCE = 20,
            NO_TIMEOUT = 21,
            BAD_FILE = 22,
            NO_SEEK = 23,
            NO_READER = 24,
            BAD_PARS = 25,
            BAD_INDEX_PAR = 26,
            OUT_OF_SYNC = 27,
            RESYNC_FAIL = 28,
            NO_8BIT = 29,
            BAD_ALIGN = 30,
            NULL_BUFFER = 31,
            NO_RELSEEK = 32,
            NULL_POINTER = 33,
            BAD_KEY = 34,
            NO_INDEX = 35,
            INDEX_FAIL = 36,
            BAD_DECODER_SETUP = 37,
            MISSING_FEATURE = 38,
            BAD_VALUE = 39,
            LSEEK_FAILED = 40,
            BAD_CUSTOM_IO = 41,
            LFS_OVERFLOW = 42,
            INT_OVERFLOW = 43
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

        public static string StrError(IntPtr handle)
        {
            IntPtr error = NativeMethods.NativeMpg123StrError(handle);
            if (error == IntPtr.Zero)
            {
                return null;
            }
            string errorMessage = Marshal.PtrToStringAnsi(error);
            return errorMessage;
        }
        public static Errors ErrorCode(IntPtr handle)
        {
            Errors error = NativeMethods.NativeMpg123ErrorCode(handle);
            return error;
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

        public void Open(string path)
        {
            IntPtr pathPtr = IntPtr.Zero;

            if (path != null)
            {
                pathPtr = Marshal.StringToHGlobalAnsi(path);
            }

            Errors error = NativeMethods.NativeMpg123Open(handle, pathPtr);

            if (error != Errors.OK)
                throw new ErrorException((Errors)error);
        }

        public void Close()
        {
            Errors error = NativeMethods.NativeMpg123Close(handle);
            if (error != Errors.OK)
                throw new ErrorException((Errors)error);
        }

        //public void Read(byte[] buffer, ulong offset)
        //{
        //    NativeMethods.NativeMpg123Read(handle, )
        //}

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
    }
}