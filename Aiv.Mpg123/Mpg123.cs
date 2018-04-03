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
        public Errors FormatNone()
        {
            Errors error = NativeMethods.NativeMpg123FormatNone(this.handle);
            if (error != Errors.OK && error != Errors.NEW_FORMAT && error != Errors.NEED_MORE && error != Errors.DONE)
                throw new ErrorException(error);
            return error;
        }
        /// <summary>
        /// Configure mpg123 handle to accept all formats (also any custom rate you may set) – this is default.
        /// </summary>
        /// <param name="handle">Can't be null</param>
        public Errors FormatAll()
        {
            Errors error = NativeMethods.NativeMpg123FormatAll(this.handle);
            if (error != Errors.OK && error != Errors.NEW_FORMAT && error != Errors.NEED_MORE && error != Errors.DONE)
                throw new ErrorException(error);
            return error;
        }
        /// <summary>
        /// Set the audio format support of a mpg123_handle in detail
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="channels"></param>
        /// <param name="encodings"></param>
        public Errors SetFormat(long rate, int channels, int encodings)
        {
            Errors error = NativeMethods.NativeMpg123Format(this.handle, (IntPtr)rate, channels, encodings);
            if (error != Errors.OK && error != Errors.NEW_FORMAT && error != Errors.NEED_MORE && error != Errors.DONE)
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
        public Errors GetFormat(ref long rate, ref int channels, ref int encoding)
        {
            IntPtr tempPtr = IntPtr.Zero;
            Errors error = NativeMethods.NativeMpg123GetFormat(this.handle, ref tempPtr, ref channels, ref encoding);
            rate = (long)tempPtr;
            if (error != Errors.OK && error != Errors.NEW_FORMAT && error != Errors.NEED_MORE && error != Errors.DONE)
                throw new ErrorException(error);
            return error;
        }
        public Errors GetFormat(ref long rate, ref int channels, ref int encoding, bool clearFlags)
        {
            IntPtr tempPtr = IntPtr.Zero;
            int _clearFlags = clearFlags ? 0 : 1;
            Errors error = NativeMethods.NativeMpg123GetFormat2(this.handle, ref tempPtr, ref channels, ref encoding, _clearFlags);
            rate = (long)tempPtr;
            if (error != Errors.OK && error != Errors.NEW_FORMAT && error != Errors.NEED_MORE && error != Errors.DONE)
                throw new ErrorException(error);
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

        /// <summary>
        /// Open and prepare to decode the specified file.
        /// </summary>
        /// <param name="path">path of the file to open</param>
        /// <returns></returns>
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

        /// <summary>
        /// Closes the source, if libmpg123 opened it.
        /// </summary>
        /// <returns></returns>
        public void Close()
        {
            Errors error = NativeMethods.NativeMpg123Close(handle);
            if (error != Errors.OK)
                throw new ErrorException((Errors)error);
        }

        /// <summary>
        /// Read from stream and decode up to the length of outMemory bytes.
        /// </summary>
        /// <param name="outMemory">memory buffer to write to</param>
        /// <param name="done">integer to store the number of actually decoded bytes to</param>
        /// <returns></returns>
        public Errors Read(byte[] outMemory, ref uint done)
        {
            IntPtr outMemoryPtr = IntPtr.Zero;
            UIntPtr outMemSizePtr = new UIntPtr((uint)outMemory.Length);
            UIntPtr donePtr = new UIntPtr(done);

            outMemoryPtr = Marshal.AllocHGlobal(outMemory.Length);
            if (outMemoryPtr != IntPtr.Zero)
                Marshal.Copy(outMemory, 0, outMemoryPtr, outMemory.Length);

            Errors error = Errors.OK;
            error = NativeMethods.NativeMpg123Read(handle, outMemoryPtr, outMemSizePtr, ref donePtr);

            if (outMemoryPtr != IntPtr.Zero)
            {
                Marshal.Copy(outMemoryPtr, outMemory, 0, outMemory.Length);
                Marshal.FreeHGlobal(outMemoryPtr);
            }

            done = donePtr.ToUInt32();

            if (error != Errors.OK && error != Errors.NEW_FORMAT && error != Errors.NEED_MORE && error != Errors.DONE)
                throw new ErrorException((Errors)error);

            return error;
        }

        /// <summary>
        /// Decode next MPEG frame to internal buffer or read a frame and return after setting a new format.
        /// </summary>
        /// <param name="num">current frame offset gets stored there</param>
        /// <param name="audio">reference set to a buffer to read the decoded audio from.</param>
        /// <param name="bytes">number of output bytes ready in the buffer</param>
        /// <returns></returns>
        public Errors DecodeFrame(ref int num, ref byte[] audio, ref uint bytes)
        {
            IntPtr numPtr       = IntPtr.Zero;
            UIntPtr bytesPtr    = UIntPtr.Zero;
            IntPtr audioPtr     = IntPtr.Zero;

            numPtr = new IntPtr(num);
            bytesPtr = new UIntPtr(bytes);

            Errors error = Errors.OK;
            error = NativeMethods.NativeMpg123DecodeFrame(handle, ref numPtr, ref audioPtr, ref bytesPtr);

            num = numPtr.ToInt32();
            bytes = bytesPtr.ToUInt32();
            audio = new byte[bytes];

            if (audioPtr != IntPtr.Zero)
                Marshal.Copy(audioPtr, audio, 0, (int)bytes);

            if (error != Errors.OK && error != Errors.NEW_FORMAT && error != Errors.NEED_MORE && error != Errors.DONE)
                throw new ErrorException((Errors)error);

            return error;
        }

        /// <summary>
        /// Open a new bitstream and prepare for direct feeding This works together with Decode(); 
        /// you are responsible for reading and feeding the input bitstream.
        /// </summary>
        public void OpenFeed()
        {
            Errors error = Errors.OK;
            error = NativeMethods.NativeMpg123OpenFeed(handle);

            if (error != Errors.OK)
                throw new ErrorException(error);
        }

        /// <summary>
        ///Feed data for a stream that has been opened with OpenFeed(). 
        ///It's give and take: You provide the bytestream, mpg123 gives you the decoded samples.
        /// </summary>
        /// <param name="inBuff">input buffer</param>
        public Errors Feed(byte[] inBuff)
        {
            IntPtr inBuffPtr = IntPtr.Zero;
            UIntPtr sizePtr = UIntPtr.Zero;

            inBuffPtr = Marshal.AllocHGlobal(inBuff.Length);
            sizePtr = new UIntPtr((uint)inBuff.Length);

            if (inBuffPtr != IntPtr.Zero)
            {
                Marshal.Copy(inBuff, 0, inBuffPtr, inBuff.Length);
            }

            Errors error = Errors.OK;
            error = NativeMethods.NativeMpg123Feed(handle, inBuffPtr, sizePtr);

            if (inBuffPtr != IntPtr.Zero)
                Marshal.FreeHGlobal(inBuffPtr);

            if (error != Errors.OK)
                throw new ErrorException(error);

            return error;
        }

        /// <summary>
        ///Decode MPEG Audio from inMemory to outMemory. 
        /// </summary>
        /// <param name="inMemory">input buffer</param>
        /// <param name="inMemSize">number of input bytes</param>
        /// <param name="outMemory">output buffer</param>
        /// <param name="outMemSize">maximum number of output bytes</param>
        /// <param name="done">integer to store the number of actually decoded bytes to</param>
        public Errors Decode(byte[] inMemory, uint inMemSize, byte[] outMemory, uint outMemSize, ref uint done)
        {
            IntPtr inMemoryPtr          = IntPtr.Zero;
            UIntPtr inMemorySizePtr     = UIntPtr.Zero;
            IntPtr outMemoryPtr         = IntPtr.Zero;
            UIntPtr outMemorySizePtr    = UIntPtr.Zero;
            UIntPtr donePtr             = UIntPtr.Zero;

            inMemorySizePtr = new UIntPtr(inMemSize);
            outMemorySizePtr = new UIntPtr(outMemSize);
            donePtr = new UIntPtr(done);

            inMemoryPtr = Marshal.AllocHGlobal((int)inMemSize);
            outMemoryPtr = Marshal.AllocHGlobal((int)outMemSize);

            if (inMemoryPtr != IntPtr.Zero && inMemory != null)
                Marshal.Copy(inMemory, 0, inMemoryPtr, (int)inMemSize);

            Errors error = Errors.OK;
            error = NativeMethods.NativeMpg123Decode(handle, inMemoryPtr, inMemorySizePtr, outMemoryPtr, outMemorySizePtr, ref donePtr);

            done = donePtr.ToUInt32();

            if(inMemoryPtr != IntPtr.Zero)
                Marshal.FreeHGlobal(inMemoryPtr);

            if (outMemoryPtr != IntPtr.Zero)
            {
                if(outMemory != null)
                    Marshal.Copy(outMemoryPtr, outMemory, 0, (int)outMemSize);

                Marshal.FreeHGlobal(outMemoryPtr);
            }

            if (error != Errors.OK && error != Errors.NEW_FORMAT && error != Errors.NEED_MORE && error != Errors.DONE)
                throw new ErrorException(error);

            return error;
        }

        /// <summary>
        /// Get the input position (byte offset in stream) of the last parsed frame. 
        /// This can be used for external seek index building, for example. 
        /// It just returns the internally stored offset, regardless of validity – you ensure that a valid frame has been parsed before!
        /// </summary>
        /// <returns></returns>
        public int FramePos()
        {
            IntPtr ret = NativeMethods.NativeMpg123FramePos(handle);
            return ret.ToInt32();
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

        public void Eq(Channels channel, int band, double val)
        {
            Errors error = NativeMethods.NativeMpg123Eq(handle, channel, band, val);
            if (error != Errors.OK)
                throw new ErrorException(error);
        }

        public double GetEq(Channels channel, int band)
        {
            double ret = NativeMethods.NativeMpg123GetEq(handle, channel, band);
            return ret;
        }

        public void ResetEq()
        {
            Errors error = NativeMethods.NativeMpg123ResetEq(handle);
            if (error != Errors.OK)
                throw new ErrorException(error);
        }

        public void Volume(double vol)
        {
            Errors error = NativeMethods.NativeMpg123Volume(handle,vol);
            if (error != Errors.OK)
                throw new ErrorException(error);
        }

        public void VolumeChange(double change)
        {
            Errors error = NativeMethods.NativeMpg123VolumeChange(handle, change);
            if (error != Errors.OK)
                throw new ErrorException(error);
        }

        public void GetVolume(ref double base_, ref double really, ref double rva_db)
        {
            Errors error = NativeMethods.NativeMpg123GetVolume(handle, ref base_, ref really, ref rva_db);
            if (error != Errors.OK)
                throw new ErrorException(error);
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