using DinkToPdf.Contracts;

namespace DinkToPdf;

public class WebSettings : ISettings
{
	[WkHtml("web.background")]
	public bool? Background { get; set; }

	[WkHtml("web.loadImages")]
	public bool? LoadImages { get; set; }

	[WkHtml("web.enableJavascript")]
	public bool? EnableJavascript { get; set; }

	[WkHtml("web.enableIntelligentShrinking")]
	public bool? EnableIntelligentShrinking { get; set; }

	[WkHtml("web.minimumFontSize")]
	public int? MinimumFontSize { get; set; }

	[WkHtml("web.printMediaType")]
	public bool? PrintMediaType { get; set; }

	[WkHtml("web.defaultEncoding")]
	public string DefaultEncoding { get; set; }

	[WkHtml("web.userStyleSheet")]
	public string UserStyleSheet { get; set; }

	[WkHtml("web.enablePlugins")]
	public bool? enablePlugins { get; set; }
}
