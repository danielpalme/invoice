using DinkToPdf.Contracts;

namespace DinkToPdf;

public class GlobalSettings : ISettings
{
	private MarginSettings margins = new MarginSettings();

	[WkHtml("orientation")]
	public Orientation? Orientation { get; set; }

	[WkHtml("colorMode")]
	public ColorMode? ColorMode { get; set; }

	[WkHtml("useCompression")]
	public bool? UseCompression { get; set; }

	[WkHtml("dpi")]
	public int? DPI { get; set; }

	[WkHtml("pageOffset")]
	public int? PageOffset { get; set; }

	[WkHtml("copies")]
	public int? Copies { get; set; }

	[WkHtml("collate")]
	public bool? Collate { get; set; }

	[WkHtml("outline")]
	public bool? Outline { get; set; }

	[WkHtml("outlineDepth")]
	public int? OutlineDepth { get; set; }

	[WkHtml("dumpOutline")]
	public string DumpOutline { get; set; }

	[WkHtml("out")]
	public string Out { get; set; }

	[WkHtml("documentTitle")]
	public string DocumentTitle { get; set; }

	[WkHtml("imageDPI")]
	public int? ImageDPI { get; set; }

	[WkHtml("imageQuality")]
	public int? ImageQuality { get; set; }

	[WkHtml("load.cookieJar")]
	public string CookieJar { get; set; }

	public PechkinPaperSize PaperSize { get; set; }

	[WkHtml("size.height")]
	private string PaperHeight
	{
		get
		{
			if (PaperSize != null)
			{
				return PaperSize.Height;
			}
			return null;
		}
	}

	[WkHtml("size.width")]
	private string PaperWidth
	{
		get
		{
			if (PaperSize != null)
			{
				return PaperSize.Width;
			}
			return null;
		}
	}

	public MarginSettings Margins
	{
		get
		{
			return margins;
		}
		set
		{
			margins = value;
		}
	}

	[WkHtml("margin.left")]
	private string MarginLeft => margins.GetMarginValue(margins.Left);

	[WkHtml("margin.right")]
	private string MarginRight => margins.GetMarginValue(margins.Right);

	[WkHtml("margin.top")]
	private string MarginTop => margins.GetMarginValue(margins.Top);

	[WkHtml("margin.bottom")]
	private string MarginBottom => margins.GetMarginValue(margins.Bottom);

	[WkHtml("viewportSize")]
	public string ViewportSize { get; set; }
}
