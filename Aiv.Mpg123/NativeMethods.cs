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

        [DllImport(LibraryName, EntryPoint = "mpg123_decoders", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr NativeMpg123Decoders();

        [DllImport(LibraryName, EntryPoint = "mpg123_new", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr NativeMpg123New(IntPtr decoder, ref int error);

        [DllImport(LibraryName, EntryPoint = "mpg123_delete", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void NativeMpg123Delete(IntPtr handle);


        //OUTPUT
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
    }
}
