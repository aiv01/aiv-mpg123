using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Aiv.Mpg123
{
    static class NativeMethods
    {
        internal const string LibraryName = "libmpg123-0.dll";

        [DllImport(LibraryName, EntryPoint = "mpg123_init", CallingConvention = CallingConvention.Cdecl)]
        // aspetta che ti passo un valore 32bit e castamelo ad Mpg123.Errors
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Mpg123.Errors NativeMpg123Init();

        [DllImport(LibraryName, EntryPoint = "mpg123_plain_strerror", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr NativeMpg123PlainStrError([MarshalAs(UnmanagedType.I4)] Mpg123.Errors error);

        [DllImport(LibraryName, EntryPoint = "mpg123_strerror", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr NativeMpg123StrError(IntPtr handle);

        [DllImport(LibraryName, EntryPoint = "mpg123_errcode", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int NativeMpg123ErrorCode(IntPtr handle);

        [DllImport(LibraryName, EntryPoint = "mpg123_decoders", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr NativeMpg123Decoders();

        [DllImport(LibraryName, EntryPoint = "mpg123_new", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr NativeMpg123New(IntPtr decoder, ref int error);

        [DllImport(LibraryName, EntryPoint = "mpg123_delete", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void NativeMpg123Delete(IntPtr handle);


    }
}
