using System;
using System.Runtime.InteropServices;

namespace DinkToPdf;

public static class WkHtmlToXBindings
{
	private const string DLLNAME = "libwkhtmltox";

	private const CharSet CHARSET = CharSet.Unicode;

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int wkhtmltopdf_extended_qt();

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern IntPtr wkhtmltopdf_version();

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int wkhtmltopdf_init(int useGraphics);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int wkhtmltopdf_deinit();

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern IntPtr wkhtmltopdf_create_global_settings();

	[DllImport("libwkhtmltox", CharSet = CharSet.Unicode)]
	public static extern int wkhtmltopdf_set_global_setting(IntPtr settings, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);

	[DllImport("libwkhtmltox", CharSet = CharSet.Unicode)]
	public unsafe static extern int wkhtmltopdf_get_global_setting(IntPtr settings, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, byte* value, int valueSize);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int wkhtmltopdf_destroy_global_settings(IntPtr settings);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern IntPtr wkhtmltopdf_create_object_settings();

	[DllImport("libwkhtmltox", CharSet = CharSet.Unicode)]
	public static extern int wkhtmltopdf_set_object_setting(IntPtr settings, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);

	[DllImport("libwkhtmltox", CharSet = CharSet.Unicode)]
	public unsafe static extern int wkhtmltopdf_get_object_setting(IntPtr settings, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, byte* value, int valueSize);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int wkhtmltopdf_destroy_object_settings(IntPtr settings);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern IntPtr wkhtmltopdf_create_converter(IntPtr globalSettings);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern void wkhtmltopdf_add_object(IntPtr converter, IntPtr objectSettings, byte[] data);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern void wkhtmltopdf_add_object(IntPtr converter, IntPtr objectSettings, [MarshalAs(UnmanagedType.LPUTF8Str)] string data);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool wkhtmltopdf_convert(IntPtr converter);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern void wkhtmltopdf_destroy_converter(IntPtr converter);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int wkhtmltopdf_get_output(IntPtr converter, out IntPtr data);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int wkhtmltopdf_set_phase_changed_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] VoidCallback callback);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int wkhtmltopdf_set_progress_changed_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] VoidCallback callback);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int wkhtmltopdf_set_finished_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] IntCallback callback);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int wkhtmltopdf_set_warning_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] StringCallback callback);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int wkhtmltopdf_set_error_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] StringCallback callback);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int wkhtmltopdf_phase_count(IntPtr converter);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int wkhtmltopdf_current_phase(IntPtr converter);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern IntPtr wkhtmltopdf_phase_description(IntPtr converter, int phase);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern IntPtr wkhtmltopdf_progress_string(IntPtr converter);

	[DllImport("libwkhtmltox", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int wkhtmltopdf_http_error_code(IntPtr converter);
}
