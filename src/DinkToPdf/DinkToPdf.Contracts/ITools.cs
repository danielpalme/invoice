using System;

namespace DinkToPdf.Contracts;

public interface ITools : IDisposable
{
	bool IsLoaded { get; }

	void Load();

	bool ExtendedQt();

	string GetLibraryVersion();

	IntPtr CreateGlobalSettings();

	int SetGlobalSetting(IntPtr settings, string name, string value);

	string GetGlobalSetting(IntPtr settings, string name);

	void DestroyGlobalSetting(IntPtr settings);

	IntPtr CreateObjectSettings();

	int SetObjectSetting(IntPtr settings, string name, string value);

	string GetObjectSetting(IntPtr settings, string name);

	void DestroyObjectSetting(IntPtr settings);

	IntPtr CreateConverter(IntPtr globalSettings);

	void AddObject(IntPtr converter, IntPtr objectSettings, byte[] data);

	void AddObject(IntPtr converter, IntPtr objectSettings, string data);

	bool DoConversion(IntPtr converter);

	void DestroyConverter(IntPtr converter);

	byte[] GetConversionResult(IntPtr converter);

	int SetPhaseChangedCallback(IntPtr converter, VoidCallback callback);

	int SetProgressChangedCallback(IntPtr converter, VoidCallback callback);

	int SetFinishedCallback(IntPtr converter, IntCallback callback);

	int SetWarningCallback(IntPtr converter, StringCallback callback);

	int SetErrorCallback(IntPtr converter, StringCallback callback);

	int GetPhaseCount(IntPtr converter);

	int GetCurrentPhase(IntPtr converter);

	string GetPhaseDescription(IntPtr converter, int phase);

	string GetProgressString(IntPtr converter);
}
