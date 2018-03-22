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
        #region metadata_pic_handling
        const int ID3 = 0x3;
        const int NEW_ID3 = 0x1;
        const int ICY = 0xC;
        const int NEW_ICY = 0x4;
        /// <summary>
        /// The picture type values from ID3v2.
        /// </summary>
        public enum PictureType
        {
            OTHER = 0,
            ICON = 1,
            OTHER_ICON = 2,
            FRONT_COVER = 3,
            BACK_COVER = 4,
            LEAFLET = 5,
            MEDIA = 6,
            LEAD = 7,
            ARTIST = 8,
            CONDUCTOR = 9,
            ORCHESTRA = 10,
            COMPOSER = 11,
            LYRICIST = 12,
            LOCATION = 13,
            RECORDING = 14,
            PERFORMANCE = 15,
            VIDEO = 16,
            FISH = 17,
            ILLUSTRATION = 18,
            ARTIST_LOGO = 19,
            PUBLISHER_LOGO = 20
        }

        /// <summary>
        /// Sub data structure for ID3v2, for storing picture data including comment. This is for the ID3v2 APIC field. You should consult the ID3v2 specification for the use of the APIC field ("frames" in ID3v2 documentation, I use "fields" here to separate from MPEG frames).
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        struct Picture
        {
            PictureType type;
            String description;
            String mimeType;
            /// <summary>
            /// Size in bytes
            /// </summary>
            UIntPtr size;
            /// <summary>
            /// Pointer to the image data
            /// </summary>
            IntPtr data;
        };

        /// <summary>
        /// Data structure for storing strings in a safer way than a standard C-String. Can also hold a number of null-terminated strings.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        struct SafeString
        {
            /// <summary>
            /// Pointer to string data
            /// </summary>
            IntPtr ptr;
            /// <summary>
            /// Raw number of bytes allocated
            /// </summary>
            UIntPtr size;
            /// <summary>
            /// Number of used bytes (including closing zero byte)
            /// </summary>
            UIntPtr fill;
        };

        /// <summary>
        /// Allocate and intialize a new string.
        /// </summary>
        /// <param name="value">Optional initial string value (can be null)</param>
        /// <returns></returns>
        public static IntPtr NewString(IntPtr value)
        {
            IntPtr str = NativeMethods.NativeMpg123NewString(value);
            return str;
        }

        /// <summary>
        /// Free memory of contents and the string structure itself.
        /// </summary>
        /// <param name="stringPtr">string handle</param>
        public static void DeleteString(IntPtr stringPtr)
        {
            NativeMethods.NativeMpg123DeleteString(stringPtr);
        }

        /// <summary>
        /// Initialize an existing mpg123_string structure to {NULL, 0, 0}.
        /// </summary>
        /// <param name="stringPtr">string handle (address of existing structure on your side)</param>
        public static void InitString(IntPtr stringPtr)
        {
            NativeMethods.NativeMpg123InitString(stringPtr);
        }

        /// <summary>
        /// Free-up memory of the contents of a mpg123_string (not the struct itself).
        /// </summary>
        /// <param name="stringPtr">string handle</param>
        public static void FreeString(IntPtr stringPtr)
        {
            NativeMethods.NativeMpg123FreeString(stringPtr);
        }

        /// <summary>
        /// Change the size of a mpg123_string
        /// Can throw an exception if fails to resize the string
        /// </summary>
        /// <param name="stringPtr">string handle</param>
        /// <param name="newSize">new size in bytes</param>
        public static void ResizeString(IntPtr stringPtr, UIntPtr newSize)
        {
            int error = NativeMethods.NativeMpg123ResizeString(stringPtr, newSize);
            if (error == 0)
                throw new Exception($"Failed to resize string at {stringPtr.ToString("X")} by {newSize}");
        }

        /// <summary>
        /// Increase size of a mpg123_string if necessary (it may stay larger).
        /// Can throw an exception if fails to resize the string
        /// </summary>
        /// <param name="stringPtr">string handle</param>
        /// <param name="newSize">new minimum size</param>
        public static void GrowString(IntPtr stringPtr, UIntPtr newSize)
        {
            int error = NativeMethods.NativeMpg123GrowString(stringPtr, newSize);
            if (error == 0)
                throw new Exception($"Failed to grow string at {stringPtr.ToString("X")} by {newSize}");
        }

        /// <summary>
        /// Copy the contents of one mpg123_string string to another
        /// Can throw an exception if fails to copy the string
        /// </summary>
        /// <param name="from">string handle</param>
        /// <param name="to">string handle</param>
        public static void CopyString(IntPtr from, IntPtr to)
        {
            int error = NativeMethods.NativeMpg123CopyString(from, to);
            if (error == 0)
                throw new Exception($"Failed to copy string at {from.ToString("X")} to {to.ToString("X")}");
        }

        /// <summary>
        /// Append a C-String to an mpg123_string
        /// Can throw an exception if fails to append the string
        /// </summary>
        /// <param name="sb">string handle</param>
        /// <param name="stringPtr">content to add</param>
        public static void AddString(IntPtr sb, IntPtr stringPtr)
        {
            int error = NativeMethods.NativeMpg123AddString(sb, stringPtr);
            if (error == 0)
                throw new Exception($"Failed to add string at {stringPtr.ToString("X")} to {sb.ToString("X")}");
        }

        /// <summary>
        /// Append a C-substring to an mpg123 string
        /// Can throw an exception if fails to append the substring
        /// </summary>
        /// <param name="sb">string handle</param>
        /// <param name="stringPtr">content to add</param>
        /// <param name="from">offset to copy from</param>
        /// <param name="count">number of characters to copy (a null-byte is always appended)</param>
        public static void AddSubString(IntPtr sb, IntPtr stringPtr, long from, long count)
        {
            int error = NativeMethods.NativeMpg123AddSubString(sb, stringPtr, (UIntPtr)from, (UIntPtr)count);
            if (error == 0)
                throw new Exception($"Failed to add substring at {stringPtr.ToString("X")} to {sb.ToString("X")}");
        }

        /// <summary>
        /// Set a C-String to an mpg123_string
        /// Can throw an exception if fails to set the string
        /// </summary>
        /// <param name="sb">string handle</param>
        /// <param name="stringPtr">string to set</param>
        public static void SetString(IntPtr sb, IntPtr stringPtr)
        {
            int error = NativeMethods.NativeMpg123SetString(sb, stringPtr);
            if (error == 0)
                throw new Exception($"Failed to set string at {stringPtr.ToString("X")} to {sb.ToString("X")}");
        }

        /// <summary>
        /// Set a C-substring to an mpg123 string
        /// Can throw an exception if fails to set the substring
        /// </summary>
        /// <param name="sb">string handle</param>
        /// <param name="stringPtr">content to add</param>
        /// <param name="from">offset to copy from</param>
        /// <param name="count">number of characters to copy (a null-byte is always appended)</param>
        public static void SetSubString(IntPtr sb, IntPtr stringPtr, long from, long count)
        {
            int error = NativeMethods.NativeMpg123SetSubString(sb, stringPtr, (UIntPtr)from, (UIntPtr)count);
            if (error == 0)
                throw new Exception($"Failed to set substring at {stringPtr.ToString("X")} to {sb.ToString("X")}");
        }

        /// <summary>
        /// Count characters in a mpg123 string (non-null bytes or UTF-8 characters).
        /// </summary>
        /// <param name="sb">string handle</param>
        /// <param name="utf8">a flag to tell if the string is in utf8 encoding</param>
        /// <returns>character count</returns>
        public static UIntPtr StrLen(IntPtr sb, bool utf8)
        {
            return NativeMethods.NativeMpg123StrLen(sb, Convert.ToInt32(utf8));
        }

        /// <summary>
        /// Remove trailing \r and \n, if present
        /// Can throw an exception if fails
        /// </summary>
        /// <param name="sb">string handle</param>
        public static void ChompString(IntPtr sb, IntPtr stringPtr)
        {
            int error = NativeMethods.NativeMpg123SetString(sb, stringPtr);
            if (error == 0)
                throw new Exception($"Failed to chomp string at {stringPtr.ToString("X")}");
        }

        #endregion

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