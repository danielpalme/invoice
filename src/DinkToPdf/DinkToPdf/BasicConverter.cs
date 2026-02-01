using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using DinkToPdf.Contracts;
using DinkToPdf.EventDefinitions;

namespace DinkToPdf;

public class BasicConverter : IConverter
{
	public readonly ITools Tools;

	public IDocument ProcessingDocument { get; private set; }

	public event EventHandler<PhaseChangedArgs> PhaseChanged;

	public event EventHandler<ProgressChangedArgs> ProgressChanged;

	public event EventHandler<FinishedArgs> Finished;

	public event EventHandler<ErrorArgs> Error;

	public event EventHandler<WarningArgs> Warning;

	public BasicConverter(ITools tools)
	{
		Tools = tools;
	}

	public virtual byte[] Convert(IDocument document)
	{
		if (document.GetObjects().Count() == 0)
		{
			throw new ArgumentException("No objects is defined in document that was passed. At least one object must be defined.");
		}
		ProcessingDocument = document;
		byte[] result = new byte[0];
		Tools.Load();
		IntPtr converter = CreateConverter(document);
		Tools.SetPhaseChangedCallback(converter, OnPhaseChanged);
		Tools.SetProgressChangedCallback(converter, OnProgressChanged);
		Tools.SetFinishedCallback(converter, OnFinished);
		Tools.SetWarningCallback(converter, OnWarning);
		Tools.SetErrorCallback(converter, OnError);
		if (Tools.DoConversion(converter))
		{
			result = Tools.GetConversionResult(converter);
		}
		Tools.DestroyConverter(converter);
		return result;
	}

	private void OnPhaseChanged(IntPtr converter)
	{
		int currentPhase = Tools.GetCurrentPhase(converter);
		PhaseChangedArgs e = new PhaseChangedArgs
		{
			Document = ProcessingDocument,
			PhaseCount = Tools.GetPhaseCount(converter),
			CurrentPhase = currentPhase,
			Description = Tools.GetPhaseDescription(converter, currentPhase)
		};
		this.PhaseChanged?.Invoke(this, e);
	}

	private void OnProgressChanged(IntPtr converter)
	{
		ProgressChangedArgs e = new ProgressChangedArgs
		{
			Document = ProcessingDocument,
			Description = Tools.GetProgressString(converter)
		};
		this.ProgressChanged?.Invoke(this, e);
	}

	private void OnFinished(IntPtr converter, int success)
	{
		FinishedArgs e = new FinishedArgs
		{
			Document = ProcessingDocument,
			Success = (success == 1)
		};
		this.Finished?.Invoke(this, e);
	}

	private void OnError(IntPtr converter, string message)
	{
		ErrorArgs e = new ErrorArgs
		{
			Document = ProcessingDocument,
			Message = message
		};
		this.Error?.Invoke(this, e);
	}

	private void OnWarning(IntPtr converter, string message)
	{
		WarningArgs e = new WarningArgs
		{
			Document = ProcessingDocument,
			Message = message
		};
		this.Warning?.Invoke(this, e);
	}

	private IntPtr CreateConverter(IDocument document)
	{
		IntPtr zero = IntPtr.Zero;
		IntPtr intPtr = Tools.CreateGlobalSettings();
		ApplyConfig(intPtr, document, isGlobal: true);
		zero = Tools.CreateConverter(intPtr);
		foreach (IObject @object in document.GetObjects())
		{
			if (@object != null)
			{
				IntPtr intPtr2 = Tools.CreateObjectSettings();
				ApplyConfig(intPtr2, @object, isGlobal: false);
				Tools.AddObject(zero, intPtr2, @object.GetContent());
			}
		}
		return zero;
	}

	private void ApplyConfig(IntPtr config, ISettings settings, bool isGlobal)
	{
		if (settings == null)
		{
			return;
		}
		BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
		PropertyInfo[] properties = settings.GetType().GetProperties(bindingAttr);
		foreach (PropertyInfo obj in properties)
		{
			Attribute[] array = (Attribute[])obj.GetCustomAttributes();
			object value = obj.GetValue(settings);
			if (value != null)
			{
				if (array.Length != 0 && array[0] is WkHtmlAttribute)
				{
					WkHtmlAttribute wkHtmlAttribute = array[0] as WkHtmlAttribute;
					Apply(config, wkHtmlAttribute.Name, value, isGlobal);
				}
				else if (value is ISettings)
				{
					ApplyConfig(config, value as ISettings, isGlobal);
				}
			}
		}
	}

	private void Apply(IntPtr config, string name, object value, bool isGlobal)
	{
		Type type = value.GetType();
		Func<IntPtr, string, string, int> func = ((!isGlobal) ? new Func<IntPtr, string, string, int>(Tools.SetObjectSetting) : new Func<IntPtr, string, string, int>(Tools.SetGlobalSetting));
		if (typeof(bool) == type)
		{
			func(config, name, ((bool)value) ? "true" : "false");
			return;
		}
		if (typeof(double) == type)
		{
			func(config, name, ((double)value).ToString("0.##", CultureInfo.InvariantCulture));
			return;
		}
		if (typeof(Dictionary<string, string>).IsAssignableFrom(type))
		{
			Dictionary<string, string> obj = (Dictionary<string, string>)value;
			int num = 0;
			{
				foreach (KeyValuePair<string, string> item in obj)
				{
					if (item.Key != null && item.Value != null)
					{
						func(config, name + ".append", null);
						func(config, $"{name}[{num}]", item.Key + "\n" + item.Value);
						num++;
					}
				}
				return;
			}
		}
		func(config, name, value.ToString());
	}
}
