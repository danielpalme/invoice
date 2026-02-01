namespace Palmmedia.InvoiceSample.Orders;

public class OrderItem
{
    public int Quantity { get; set; } = 1;

    public required string ArticleNumber { get; set; }

    public required string Description { get; set; }

    public required decimal NetPrice { get; set; }

    public required decimal VatRate { get; set; }

    public decimal NetPriceTotal => this.Quantity * this.NetPrice;

    public decimal Tax => Math.Round(this.NetPriceTotal * this.VatRate, 2, MidpointRounding.AwayFromZero);

    public decimal GrossPrice => this.NetPrice + Math.Round(this.NetPrice * this.VatRate, 2, MidpointRounding.AwayFromZero);

    public decimal GrossPriceTotal => this.NetPriceTotal + this.Tax;
}