using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Aiv.Mpg123
{
    static class NativeMethods
    {
        internal const string LibraryName = "libmpg123-0.dll";

        [DllImport(LibraryName, EntryPoint = "mpg123_init", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Mpg123.Errors NativeMpg123Init();

        [DllImport(LibraryName, EntryPoint = "mpg123_plain_strerror", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr NativeMpg123PlainStrError([MarshalAs(UnmanagedType.I4)] Mpg123.Errors error);

        [DllImport(LibraryName, EntryPoint = "mpg123_strerror", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr NativeMpg123StrError(IntPtr handle);

        [DllImport(LibraryName, EntryPoint = "mpg123_errcode", CallingConvention = CallingConvention.Cdecl)]
        internal extern static Mpg123.Errors NativeMpg123ErrorCode(IntPtr handle);

        [DllImport(LibraryName, EntryPoint = "mpg123_decoders", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr NativeMpg123Decoders();

        [DllImport(LibraryName, EntryPoint = "mpg123_supported_decoders", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr NativeMpg123SupportedDecoders();

        [DllImport(LibraryName, EntryPoint = "mpg123_decoder", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr NativeMpg123Decoder(IntPtr handle, IntPtr decoder);

        [DllImport(LibraryName, EntryPoint = "mpg123_current_decoder", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr NativeMpg123CurrentDecoder(IntPtr handle);

        [DllImport(LibraryName, EntryPoint = "mpg123_new", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr NativeMpg123New(IntPtr decoder, ref int error);

        [DllImport(LibraryName, EntryPoint = "mpg123_delete", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void NativeMpg123Delete(IntPtr handle);

        [DllImport(LibraryName, EntryPoint = "mpg123_open", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Mpg123.Errors NativeMpg123Open(IntPtr handle, IntPtr path);

        [DllImport(LibraryName, EntryPoint = "mpg123_close", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Mpg123.Errors NativeMpg123Close(IntPtr handle);

        [DllImport(LibraryName, EntryPoint = "mpg123_read", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Mpg123.Errors NativeMpg123Read(IntPtr handle, IntPtr outMemory, UIntPtr outMemSize, UIntPtr done);

        [DllImport(LibraryName, EntryPoint = "mpg123_param", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int NativeMpg123SetParam(IntPtr handle, [MarshalAs(UnmanagedType.I4)] Mpg123.Mpg123Params type, IntPtr value, double fvalue);

        [DllImport(LibraryName, EntryPoint = "mpg123_getparam", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int NativeMpg123GetParam(IntPtr handle, [MarshalAs(UnmanagedType.I4)] Mpg123.Mpg123Params type, ref IntPtr value, ref double fvalue);

        [DllImport(LibraryName, EntryPoint = "mpg123_rates", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void NativeMpg123Rates(ref IntPtr list, ref UIntPtr number);

        [DllImport(LibraryName, EntryPoint = "mpg123_encodings", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void NativeMpg123Encodings(ref IntPtr list, ref UIntPtr number);

        [DllImport(LibraryName, EntryPoint = "mpg123_encsize", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int NativeMpg123EncodingsSize(int encoding);

        [DllImport(LibraryName, EntryPoint = "mpg123_format_none", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Mpg123.Errors NativeMpg123FormatNone(IntPtr handle);

        [DllImport(LibraryName, EntryPoint = "mpg123_format_all", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Mpg123.Errors NativeMpg123FormatAll(IntPtr handle);

        [DllImport(LibraryName, EntryPoint = "mpg123_format", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Mpg123.Errors NativeMpg123Format(IntPtr handle, IntPtr rate, int channels, int encodings);

        [DllImport(LibraryName, EntryPoint = "mpg123_format_support", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.I4)]
        internal extern static Mpg123.ChannelCount NativeMpg123FormatSupport(IntPtr handle, IntPtr rate, int encoding);

        [DllImport(LibraryName, EntryPoint = "mpg123_getformat", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Mpg123.Errors NativeMpg123GetFormat(IntPtr handle, ref IntPtr rate, ref int channels, ref int encoding);

        [DllImport(LibraryName, EntryPoint = "mpg123_getformat2", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Mpg123.Errors NativeMpg123GetFormat2(IntPtr handle, ref IntPtr rate, ref int channels, ref int encoding, int clearFlags);

        [DllImport(LibraryName, EntryPoint = "mpg123_feature", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int NativeMpg123Feature([MarshalAs(UnmanagedType.I4)] Mpg123.Mpg123FeatureSet key);
    }
}