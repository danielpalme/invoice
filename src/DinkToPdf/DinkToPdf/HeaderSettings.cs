using DinkToPdf.Contracts;

namespace DinkToPdf;

public class HeaderSettings : ISettings
{
	[WkHtml("header.fontSize")]
	public int? FontSize { get; set; }

	[WkHtml("header.fontName")]
	public string FontName { get; set; }

	[WkHtml("header.left")]
	public string Left { get; set; }

	[WkHtml("header.center")]
	public string Center { get; set; }

	[WkHtml("header.right")]
	public string Right { get; set; }

	[WkHtml("header.line")]
	public bool? Line { get; set; }

	[WkHtml("header.spacing")]
	public double? Spacing { get; set; }

	[WkHtml("header.htmlUrl")]
	public string HtmUrl { get; set; }
}
