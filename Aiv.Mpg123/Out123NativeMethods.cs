using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Aiv.Mpg123
{
    static class Out123NativeMethods
    {
        internal const string LibraryName = "libout123-0.dll";

        //API: out123_handle* out123_new(void)
        [DllImport(LibraryName, EntryPoint = "out123_new", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr New();

        //API: out123_del(out123_handle *ao)
        [DllImport(LibraryName, EntryPoint = "out123_del", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void Del(IntPtr handle);

        //API: const char* out123_strerror(out123_handle *ao)
        [DllImport(LibraryName, EntryPoint = "out123_strerror", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr StrError(IntPtr handle);

        //API: out123_errcode(out123_handle *ao)
        [DllImport(LibraryName, EntryPoint = "out123_errcode", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Out123.Errors ErrCode(IntPtr handle);

        //API: const char* out123_plain_strerror(int errcode)
        [DllImport(LibraryName, EntryPoint = "out123_plain_strerror", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr PlainStrError([MarshalAs(UnmanagedType.I4)] Out123.Errors error);

        //API: int out123_set_buffer(out123_handle *ao, size_t buffer_bytes)
        [DllImport(LibraryName, EntryPoint = "out123_set_buffer", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Out123.Errors SetBuffer(IntPtr handle, UIntPtr bufferSize);

        //API: int out123_param(out123_handle* ao, enum out123_parms code, long value, double fvalue, const char* svalue)
        [DllImport(LibraryName, EntryPoint = "out123_param", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Out123.Errors Param(IntPtr handle, [MarshalAs(UnmanagedType.I4)] Out123.Params code, IntPtr value, double fvalue, IntPtr svalue);

        //API: int out123_getparam (out123_handle *ao, enum out123_parms code, long *ret_value, double *ret_fvalue, char **ret_svalue)
        [DllImport(LibraryName, EntryPoint = "out123_getparam", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Out123.Errors GetParam(IntPtr handle, [MarshalAs(UnmanagedType.I4)] Out123.Params code, ref long value, ref double fvalue, IntPtr svalue);

        //API: int out123_param_from(out123_handle *ao, out123_handle *from_ao)
        [DllImport(LibraryName, EntryPoint = "out123_param_from", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Out123.Errors ParamFrom(IntPtr destHandle, IntPtr sourceHandle);

        //API: int out123_drivers(out123_handle *ao, char ***names, char ***descr)
        [DllImport(LibraryName, EntryPoint = "out123_drivers", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Out123.Errors Drivers(IntPtr handle, IntPtr names, IntPtr descrs);

        //API: int out123_driver_info(out123_handle* ao, char** driver, char** device)
        [DllImport(LibraryName, EntryPoint = "out123_driver_info", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Out123.Errors DriverInfo(IntPtr handle, IntPtr driver, IntPtr device);

        //API: int out123_open(out123_handle* ao, const char* driver, const char* device)
        [DllImport(LibraryName, EntryPoint = "out123_open", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Out123.Errors Open(IntPtr handle, IntPtr driver, IntPtr device);

        //API: int out123_start(out123_handle *ao, long rate, int channels, int encoding)
        [DllImport(LibraryName, EntryPoint = "out123_start", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Out123.Errors Start(IntPtr handle, IntPtr rate, int channels, int encoding);

        //API: size_t out123_play(out123_handle* ao, void* buffer, size_t bytes)
        [DllImport(LibraryName, EntryPoint = "out123_play", CallingConvention = CallingConvention.Cdecl)]
        internal extern static UIntPtr Play(IntPtr handle, IntPtr buffer, UIntPtr bytes);
    }
}
