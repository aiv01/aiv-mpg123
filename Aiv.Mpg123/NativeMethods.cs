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

        [return: MarshalAs(UnmanagedType.I4)]
        [DllImport(LibraryName, EntryPoint = "mpg123_eq", CallingConvention = CallingConvention.Cdecl)]
        internal extern static Mpg123.Errors NativeMpg123Eq(IntPtr handle, [MarshalAs(UnmanagedType.I4)] Mpg123.Channels channel, int band, double val);

        [DllImport(LibraryName, EntryPoint = "mpg123_geteq", CallingConvention = CallingConvention.Cdecl)]
        internal extern static double NativeMpg123GetEq(IntPtr handle, [MarshalAs(UnmanagedType.I4)] Mpg123.Channels channel, int band);

        [return: MarshalAs(UnmanagedType.I4)]
        [DllImport(LibraryName, EntryPoint = "mpg123_reset_eq", CallingConvention = CallingConvention.Cdecl)]
        internal extern static Mpg123.Errors NativeMpg123ResetEq(IntPtr handle);

        [return: MarshalAs(UnmanagedType.I4)]
        [DllImport(LibraryName, EntryPoint = "mpg123_volume", CallingConvention = CallingConvention.Cdecl)]
        internal extern static Mpg123.Errors NativeMpg123Volume(IntPtr handle, double vol);

        [return: MarshalAs(UnmanagedType.I4)]
        [DllImport(LibraryName, EntryPoint = "mpg123_volume_change", CallingConvention = CallingConvention.Cdecl)]
        internal extern static Mpg123.Errors NativeMpg123VolumeChange(IntPtr handle, double change);

        [return: MarshalAs(UnmanagedType.I4)]
        [DllImport(LibraryName, EntryPoint = "mpg123_getvolume", CallingConvention = CallingConvention.Cdecl)]
        internal extern static Mpg123.Errors NativeMpg123GetVolume(IntPtr handle, ref double base_, ref double really, ref double rva_db);
    }
}
