using System.Collections.Generic;
using DinkToPdf.Contracts;

namespace DinkToPdf;

public class LoadSettings : ISettings
{
	[WkHtml("load.username")]
	public string Username { get; set; }

	[WkHtml("load.password")]
	public string Password { get; set; }

	[WkHtml("load.jsdelay")]
	public int? JSDelay { get; set; }

	[WkHtml("load.zoomFactor")]
	public double? ZoomFactor { get; set; }

	[WkHtml("load.blockLocalFileAccess")]
	public bool? BlockLocalFileAccess { get; set; }

	[WkHtml("load.stopSlowScript")]
	public bool? StopSlowScript { get; set; }

	[WkHtml("load.debugJavascript")]
	public bool? DebugJavascript { get; set; }

	[WkHtml("load.loadErrorHandling")]
	public ContentErrorHandling? LoadErrorHandling { get; set; }

	[WkHtml("load.proxy")]
	public string Proxy { get; set; }

	[WkHtml("load.customHeaders")]
	public Dictionary<string, string> CustomHeaders { get; set; }

	[WkHtml("load.repeatCustomHeaders")]
	public bool? RepeatCustomHeaders { get; set; }

	[WkHtml("load.cookies")]
	public Dictionary<string, string> Cookies { get; set; }
}
