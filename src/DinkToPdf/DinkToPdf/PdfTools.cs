using System;
using System.Runtime.InteropServices;
using System.Text;
using DinkToPdf.Contracts;

namespace DinkToPdf;

public sealed class PdfTools : ITools, IDisposable
{
	private bool disposedValue;

	public bool IsLoaded { get; private set; }

	public PdfTools()
	{
		IsLoaded = false;
	}

	public void Load()
	{
		if (!IsLoaded && WkHtmlToXBindings.wkhtmltopdf_init(0) == 1)
		{
			IsLoaded = true;
		}
	}

	public bool ExtendedQt()
	{
		return WkHtmlToXBindings.wkhtmltopdf_extended_qt() == 1;
	}

	public string GetLibraryVersion()
	{
		return Marshal.PtrToStringAnsi(WkHtmlToXBindings.wkhtmltopdf_version());
	}

	public IntPtr CreateGlobalSettings()
	{
		return WkHtmlToXBindings.wkhtmltopdf_create_global_settings();
	}

	public int SetGlobalSetting(IntPtr settings, string name, string value)
	{
		return WkHtmlToXBindings.wkhtmltopdf_set_global_setting(settings, name, value);
	}

	public unsafe string GetGlobalSetting(IntPtr settings, string name)
	{
		byte[] array = new byte[2048];
		fixed (byte* value = array)
		{
			WkHtmlToXBindings.wkhtmltopdf_get_global_setting(settings, name, value, array.Length);
		}
		return GetString(array);
	}

	public void DestroyGlobalSetting(IntPtr settings)
	{
		WkHtmlToXBindings.wkhtmltopdf_destroy_global_settings(settings);
	}

	public IntPtr CreateObjectSettings()
	{
		return WkHtmlToXBindings.wkhtmltopdf_create_object_settings();
	}

	public int SetObjectSetting(IntPtr settings, string name, string value)
	{
		return WkHtmlToXBindings.wkhtmltopdf_set_object_setting(settings, name, value);
	}

	public unsafe string GetObjectSetting(IntPtr settings, string name)
	{
		byte[] array = new byte[2048];
		fixed (byte* value = array)
		{
			WkHtmlToXBindings.wkhtmltopdf_get_object_setting(settings, name, value, array.Length);
		}
		return GetString(array);
	}

	public void DestroyObjectSetting(IntPtr settings)
	{
		WkHtmlToXBindings.wkhtmltopdf_destroy_object_settings(settings);
	}

	public IntPtr CreateConverter(IntPtr globalSettings)
	{
		return WkHtmlToXBindings.wkhtmltopdf_create_converter(globalSettings);
	}

	public void AddObject(IntPtr converter, IntPtr objectSettings, byte[] data)
	{
		WkHtmlToXBindings.wkhtmltopdf_add_object(converter, objectSettings, data);
	}

	public void AddObject(IntPtr converter, IntPtr objectSettings, string data)
	{
		WkHtmlToXBindings.wkhtmltopdf_add_object(converter, objectSettings, data);
	}

	public bool DoConversion(IntPtr converter)
	{
		return WkHtmlToXBindings.wkhtmltopdf_convert(converter);
	}

	public void DestroyConverter(IntPtr converter)
	{
		WkHtmlToXBindings.wkhtmltopdf_destroy_converter(converter);
	}

	public byte[] GetConversionResult(IntPtr converter)
	{
		IntPtr data;
		int num = WkHtmlToXBindings.wkhtmltopdf_get_output(converter, out data);
		byte[] array = new byte[num];
		Marshal.Copy(data, array, 0, num);
		return array;
	}

	public int SetPhaseChangedCallback(IntPtr converter, VoidCallback callback)
	{
		return WkHtmlToXBindings.wkhtmltopdf_set_phase_changed_callback(converter, callback);
	}

	public int SetProgressChangedCallback(IntPtr converter, VoidCallback callback)
	{
		return WkHtmlToXBindings.wkhtmltopdf_set_progress_changed_callback(converter, callback);
	}

	public int SetFinishedCallback(IntPtr converter, IntCallback callback)
	{
		return WkHtmlToXBindings.wkhtmltopdf_set_finished_callback(converter, callback);
	}

	public int SetWarningCallback(IntPtr converter, StringCallback callback)
	{
		return WkHtmlToXBindings.wkhtmltopdf_set_warning_callback(converter, callback);
	}

	public int SetErrorCallback(IntPtr converter, StringCallback callback)
	{
		return WkHtmlToXBindings.wkhtmltopdf_set_error_callback(converter, callback);
	}

	public int GetPhaseCount(IntPtr converter)
	{
		return WkHtmlToXBindings.wkhtmltopdf_phase_count(converter);
	}

	public int GetCurrentPhase(IntPtr converter)
	{
		return WkHtmlToXBindings.wkhtmltopdf_current_phase(converter);
	}

	public string GetPhaseDescription(IntPtr converter, int phase)
	{
		return Marshal.PtrToStringAnsi(WkHtmlToXBindings.wkhtmltopdf_phase_description(converter, phase));
	}

	public string GetProgressString(IntPtr converter)
	{
		return Marshal.PtrToStringAnsi(WkHtmlToXBindings.wkhtmltopdf_progress_string(converter));
	}

	private void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			WkHtmlToXBindings.wkhtmltopdf_deinit();
			disposedValue = true;
		}
	}

	~PdfTools()
	{
		Dispose(disposing: false);
	}

	void IDisposable.Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private string GetString(byte[] buffer)
	{
		int i;
		for (i = 0; i < buffer.Length && buffer[i] != 0; i++)
		{
		}
		return Encoding.UTF8.GetString(buffer, 0, i);
	}
}
