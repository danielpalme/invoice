using System.Globalization;

namespace DinkToPdf;

public class MarginSettings
{
	public Unit Unit { get; set; }

	public double? Top { get; set; }

	public double? Bottom { get; set; }

	public double? Left { get; set; }

	public double? Right { get; set; }

	public MarginSettings()
	{
		Unit = Unit.Millimeters;
	}

	public MarginSettings(double top, double right, double bottom, double left)
		: this()
	{
		Top = top;
		Bottom = bottom;
		Left = left;
		Right = right;
	}

	public string GetMarginValue(double? value)
	{
		if (!value.HasValue)
		{
			return null;
		}
		string text = "in";
		return string.Concat(str1: Unit switch
		{
			Unit.Inches => "in", 
			Unit.Millimeters => "mm", 
			Unit.Centimeters => "cm", 
			_ => "in", 
		}, str0: value.Value.ToString("0.##", CultureInfo.InvariantCulture));
	}
}
