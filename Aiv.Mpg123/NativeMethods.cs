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
        internal extern static Mpg123.Errors NativeMpg123SetParam(IntPtr handle, [MarshalAs(UnmanagedType.I4)] Mpg123.Mpg123Params type, IntPtr value, double fvalue);

        [DllImport(LibraryName, EntryPoint = "mpg123_getparam", CallingConvention = CallingConvention.Cdecl)]
        internal extern static Mpg123.Errors NativeMpg123GetParam(IntPtr handle, [MarshalAs(UnmanagedType.I4)] Mpg123.Mpg123Params type, IntPtr value, IntPtr fvalue);

        //[DllImport(LibraryName, EntryPoint = "mpg123_feature", CallingConvention = CallingConvention.Cdecl)]
        //internal extern static void NativeMpg123Feature([MarshalAs(UnmanagedType.I4)] Mpg123.Mpg123FeatureSet key);
    }
}
