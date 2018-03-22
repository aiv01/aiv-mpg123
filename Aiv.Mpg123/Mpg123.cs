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

        public enum Mpg123Params
        {
            MPG123_VERBOSE = 0, MPG123_FLAGS,
            MPG123_ADD_FLAGS, MPG123_FORCE_RATE,
            MPG123_DOWN_SAMPLE, MPG123_RVA,
            MPG123_DOWNSPEED, MPG123_UPSPEED,
            MPG123_START_FRAME, MPG123_DECODE_FRAMES,
            MPG123_ICY_INTERVAL, MPG123_OUTSCALE,
            MPG123_TIMEOUT, MPG123_REMOVE_FLAGS,
            MPG123_RESYNC_LIMIT, MPG123_INDEX_SIZE,
            MPG123_PREFRAMES, MPG123_FEEDPOOL,
            MPG123_FEEDBUFFER, MPG123_FREEFORMAT_SIZE
        }

        public enum Mpg123FeatureSet
        {
            MPG123_FEATURE_ABI_UTF8OPEN = 0, MPG123_FEATURE_OUTPUT_8BIT,
            MPG123_FEATURE_OUTPUT_16BIT, MPG123_FEATURE_OUTPUT_32BIT,
            MPG123_FEATURE_INDEX, MPG123_FEATURE_PARSE_ID3V2,
            MPG123_FEATURE_DECODE_LAYER1, MPG123_FEATURE_DECODE_LAYER2,
            MPG123_FEATURE_DECODE_LAYER3, MPG123_FEATURE_DECODE_ACCURATE,
            MPG123_FEATURE_DECODE_DOWNSAMPLE, MPG123_FEATURE_DECODE_NTOM,
            MPG123_FEATURE_PARSE_ICY, MPG123_FEATURE_TIMEOUT_READ,
            MPG123_FEATURE_EQUALIZER, MPG123_FEATURE_MOREINFO
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

        public void SetParam([MarshalAs(UnmanagedType.I4)] Mpg123Params type, long value = 0, double fvalue = 0)
        {
            int setParam = NativeMethods.NativeMpg123SetParam(handle, type, (IntPtr)value, fvalue);

            if ((Errors)setParam != Errors.OK)
            {
                throw new ErrorException((Errors)setParam);
            }
        }

        public void GetParam([MarshalAs(UnmanagedType.I4)] Mpg123Params type, ref long value, ref double fValue)
        {
            IntPtr paramValue = IntPtr.Zero;
            double paramFValue = 0;

            int getParam = NativeMethods.NativeMpg123GetParam(handle, type, ref paramValue, ref paramFValue);

            if ((Errors)getParam != Errors.OK)
            {
                throw new ErrorException((Errors)getParam);
            }

            value = (long)paramValue;
            fValue = paramFValue;
        }

        public void Feature([MarshalAs(UnmanagedType.I4)] Mpg123FeatureSet key)
        {
            int feature = NativeMethods.NativeMpg123Feature(key);

            if (feature == 0)
            {
                throw new Exception("unimplemented functions");
            }
        }
    }
}
