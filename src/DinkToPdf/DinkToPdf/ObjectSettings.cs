using System.Text;
using DinkToPdf.Contracts;

namespace DinkToPdf;

public class ObjectSettings : IObject, ISettings
{
	private WebSettings webSettings = new WebSettings();

	private HeaderSettings headerSettings = new HeaderSettings();

	private FooterSettings footerSettings = new FooterSettings();

	private LoadSettings loadSettings = new LoadSettings();

	[WkHtml("page")]
	public string Page { get; set; }

	[WkHtml("useExternalLinks")]
	public bool? UseExternalLinks { get; set; }

	[WkHtml("useLocalLinks")]
	public bool? UseLocalLinks { get; set; }

	[WkHtml("produceForms")]
	public bool? ProduceForms { get; set; }

	[WkHtml("includeInOutline")]
	public bool? IncludeInOutline { get; set; }

	[WkHtml("pagesCount")]
	public bool? PagesCount { get; set; }

	public string HtmlContent { get; set; }

	public WebSettings WebSettings
	{
		get
		{
			return webSettings;
		}
		set
		{
			webSettings = value;
		}
	}

	public HeaderSettings HeaderSettings
	{
		get
		{
			return headerSettings;
		}
		set
		{
			headerSettings = value;
		}
	}

	public FooterSettings FooterSettings
	{
		get
		{
			return footerSettings;
		}
		set
		{
			footerSettings = value;
		}
	}

	public LoadSettings LoadSettings
	{
		get
		{
			return loadSettings;
		}
		set
		{
			loadSettings = value;
		}
	}

	public byte[] GetContent()
	{
		if (HtmlContent == null)
		{
			return new byte[0];
		}
		return Encoding.UTF8.GetBytes(HtmlContent);
	}
}
