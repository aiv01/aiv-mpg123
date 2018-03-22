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
        public enum ChannelCount
        {
            NONE, MONO, STEREO, BOTH
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

        /// <summary>
        /// An IEnumerable of supported Rates
        /// </summary>
        public static IEnumerable<long> Rates
        {
            get
            {
                IntPtr ratesPtr = IntPtr.Zero;
                UIntPtr number = UIntPtr.Zero;

                NativeMethods.NativeMpg123Rates(ref ratesPtr, ref number);
                int offset = 0;
                for (ulong i = 0; i < (ulong)number; i++)
                {
                    long value = (long)Marshal.ReadIntPtr(ratesPtr, offset);
                    yield return value;
                    offset += Marshal.SizeOf<IntPtr>();
                }
            }
        }
        /// <summary>
        /// An IEnumerable of supported audio encodings
        /// </summary>
        public static IEnumerable<int> Encodings
        {
            get
            {
                IntPtr ratesPtr = IntPtr.Zero;
                UIntPtr number = UIntPtr.Zero;

                NativeMethods.NativeMpg123Encodings(ref ratesPtr, ref number);
                int offset = 0;
                for (ulong i = 0; i < (ulong)number; i++)
                {
                    int value = (int)Marshal.ReadInt32(ratesPtr, offset);
                    yield return value;
                    offset += Marshal.SizeOf<IntPtr>();
                }
            }
        }
        /// <summary>
        /// Returns the size in bytes of one mono sample of the named encoding.
        /// </summary>
        /// <param name="encoding">The encoding value to analyze</param>
        /// <returns>positive size of encoding in bytes, 0 on invalid encoding</returns>
        public static int GetEncodingSize(int encoding)
        {
            return NativeMethods.NativeMpg123EncodingsSize(encoding);
        }
        /// <summary>
        /// Configure a mpg123 handle to accept no output format at all, use before specifying supported formats with mpg123_format
        /// </summary>
        /// <param name="handle">Can't be null</param>
        /// <returns>Returns OK on succes</returns>
        public Errors FormatNone()
        {
            Errors error = NativeMethods.NativeMpg123FormatNone(this.handle);
            if (error != Errors.OK)
                throw new ErrorException(error);
            return error;
        }
        /// <summary>
        /// Configure mpg123 handle to accept all formats (also any custom rate you may set) – this is default.
        /// </summary>
        /// <param name="handle">Can't be null</param>
        /// <returns>Returns OK on succes</returns>
        public Errors FormatAll()
        {
            Errors error = NativeMethods.NativeMpg123FormatAll(this.handle);
            if (error != Errors.OK)
                throw new ErrorException(error);
            return error;
        }
        /// <summary>
        /// Set the audio format support of a mpg123_handle in detail
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="channels"></param>
        /// <param name="encodings"></param>
        /// <returns></returns>
        public Errors Format(long rate, int channels, int encodings)
        {
            Errors error = NativeMethods.NativeMpg123Format(this.handle, (IntPtr)rate, channels, encodings);
            if (error != Errors.OK)
                throw new ErrorException(error);
            return error;
        }
        public ChannelCount IsFormatSupported(long rate, int encoding)
        {
            ChannelCount channels = NativeMethods.NativeMpg123FormatSupport(this.handle, (IntPtr)rate, encoding);
            return channels;
        }
        /// <summary>
        /// Get the current output format, written to reference passed
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="channels"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public Errors GetFormat(ref long rate, ref int channels, ref int encoding)
        {
            IntPtr tempPtr = IntPtr.Zero;
            Errors error = NativeMethods.NativeMpg123GetFormat(this.handle, ref tempPtr,ref channels,ref encoding);
            rate = (long)tempPtr;
            return error;
        }
        public Errors GetFormat(ref long rate, ref int channels, ref int encoding, bool clearFlags)
        {
            IntPtr tempPtr = IntPtr.Zero;
            int _clearFlags = clearFlags ? 0 : 1;
            Errors error = NativeMethods.NativeMpg123GetFormat2(this.handle, ref tempPtr, ref channels, ref encoding, _clearFlags);
            rate = (long)tempPtr;
            return error;
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
    }
}
