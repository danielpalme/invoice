using System.Collections.Generic;
using DinkToPdf.Contracts;

namespace DinkToPdf;

public class HtmlToPdfDocument : IDocument, ISettings
{
	private GlobalSettings globalSettings = new GlobalSettings();

	public List<ObjectSettings> Objects { get; private set; }

	public GlobalSettings GlobalSettings
	{
		get
		{
			return globalSettings;
		}
		set
		{
			globalSettings = value;
		}
	}

	public HtmlToPdfDocument()
	{
		Objects = new List<ObjectSettings>();
	}

	public IEnumerable<IObject> GetObjects()
	{
		return Objects.ToArray();
	}
}
