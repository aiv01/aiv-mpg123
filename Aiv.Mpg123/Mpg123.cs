using System;
using System.Collections.Generic;
using System.IO;
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
        public enum ChannelCount
        {
            NONE, MONO, STEREO, BOTH
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


        public static IEnumerable<string> SupportedDecoders
        {
            get
            {
                IntPtr sDecodersPtr = NativeMethods.NativeMpg123SupportedDecoders();
                int offset = 0;
                while (true)
                {
                    IntPtr sDecoderPtr = Marshal.ReadIntPtr(sDecodersPtr, offset);
                    if (sDecoderPtr == IntPtr.Zero)
                    {
                        yield break;
                    }
                    yield return Marshal.PtrToStringAnsi(sDecoderPtr);
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
            Errors error = NativeMethods.NativeMpg123GetFormat(this.handle, ref tempPtr, ref channels, ref encoding);
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

        #region SEEK_POSITION

        public long Tell()
        {
            return (long)NativeMethods.NativeMpg123Tell(handle);
        }

        public long TellFrame()
        {
            return (long)NativeMethods.NativeMpg123TellFrame(handle);
        }

        public long TellStream()
        {
            return (long)NativeMethods.NativeMpg123TellStream(handle);
        }

        public long Seek(long offSample, SeekOrigin whence)
        {
            long offset = (long)NativeMethods.NativeMpg123Seek(handle, (IntPtr)offSample, whence);
            return offset >= 0 ? offset : throw new ErrorException((Errors)offset);
        }

        public long SeekFrame(long frameOff, SeekOrigin whence)
        {
            long offset = (long)NativeMethods.NativeMpg123SeekFrame(handle, (IntPtr)frameOff, whence);
            return offset >= 0 ? offset : throw new ErrorException((Errors)offset);
        }

        public long TimeFrame(double sec)
        {
            long frameOffset = (long)NativeMethods.NativeMpg123TimeFrame(handle, sec);
            return frameOffset >= 0 ? frameOffset : throw new ErrorException((Errors)frameOffset);
        }

        #endregion

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

        private void SetDecoder(string decoder)
        {
            IntPtr decoderPtr = IntPtr.Zero;

            decoderPtr = Marshal.StringToHGlobalAnsi(decoder);
            NativeMethods.NativeMpg123Decoder(handle, decoderPtr);
        }

        private string GetDecoder()
        {
            IntPtr decoderPtr = IntPtr.Zero;

            decoderPtr = NativeMethods.NativeMpg123CurrentDecoder(handle);
            return Marshal.PtrToStringAnsi(decoderPtr);
        }

        private string _decoder;
        public string Decoder
        {
            get
            {
                _decoder = GetDecoder();
                return _decoder;

            }
            set
            {
                _decoder = value;
                SetDecoder(_decoder);
            }
        }
    }
}