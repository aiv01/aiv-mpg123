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

        [DllImport(LibraryName, EntryPoint = "mpg123_new_string", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr NativeMpg123NewString(IntPtr val);

        [DllImport(LibraryName, EntryPoint = "mpg123_delete_string", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void NativeMpg123DeleteString(IntPtr sb);

        [DllImport(LibraryName, EntryPoint = "mpg123_init_string", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void NativeMpg123InitString(IntPtr sb);

        [DllImport(LibraryName, EntryPoint = "mpg123_free_string", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void NativeMpg123FreeString(IntPtr sb);

        [DllImport(LibraryName, EntryPoint = "mpg123_resize_string", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int NativeMpg123ResizeString(IntPtr sb, UIntPtr news);

        [DllImport(LibraryName, EntryPoint = "mpg123_grow_string", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int NativeMpg123GrowString(IntPtr sb, UIntPtr news);

        [DllImport(LibraryName, EntryPoint = "mpg123_copy_string", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int NativeMpg123CopyString(IntPtr from, IntPtr to);

        [DllImport(LibraryName, EntryPoint = "mpg123_add_string", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int NativeMpg123AddString(IntPtr sb, IntPtr stuff);

        [DllImport(LibraryName, EntryPoint = "mpg123_add_substring", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int NativeMpg123AddSubString(IntPtr sb, IntPtr stuff, UIntPtr from, UIntPtr count);

        [DllImport(LibraryName, EntryPoint = "mpg123_set_string", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int NativeMpg123SetString(IntPtr sb, IntPtr stuff);

        [DllImport(LibraryName, EntryPoint = "mpg123_set_substring", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int NativeMpg123SetSubString(IntPtr sb, IntPtr stuff, UIntPtr from, UIntPtr count);

        [DllImport(LibraryName, EntryPoint = "mpg123_strlen", CallingConvention = CallingConvention.Cdecl)]
        internal extern static UIntPtr NativeMpg123StrLen(IntPtr sb, int utf8);

        [DllImport(LibraryName, EntryPoint = "mpg123_chomp_string", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int NativeMpg123ChompString(IntPtr sb);


    }
}
