using DinkToPdf.Contracts;

namespace DinkToPdf;

public class FooterSettings : ISettings
{
	[WkHtml("footer.fontSize")]
	public int? FontSize { get; set; }

	[WkHtml("footer.fontName")]
	public string FontName { get; set; }

	[WkHtml("footer.left")]
	public string Left { get; set; }

	[WkHtml("footer.center")]
	public string Center { get; set; }

	[WkHtml("footer.right")]
	public string Right { get; set; }

	[WkHtml("footer.line")]
	public bool? Line { get; set; }

	[WkHtml("footer.spacing")]
	public double? Spacing { get; set; }

	[WkHtml("footer.htmlUrl")]
	public string HtmUrl { get; set; }
}
