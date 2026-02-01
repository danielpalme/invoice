namespace Palmmedia.InvoiceSample.Orders;

public class Order
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public required string OrderNumber { get; set; }

    public required string CustomerNumber { get; set; }

    public required string? CustomerReference { get; set; }

    public required string CustomerName { get; set; }

    public required string Address { get; set; }

    public required string ZipCode { get; set; }

    public required string City { get; set; }

    public required string Country { get; set; }

    public required string Email { get; set; }

    public IReadOnlyCollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public decimal NetPriceTotal => this.OrderItems.Sum(x => x.NetPriceTotal);

    public decimal Tax
    {
        get
        {
            decimal tax = 0;
            foreach (var vatGroup in this.OrderItems.GroupBy(o => o.VatRate))
            {
                tax += Math.Round(vatGroup.Sum(v => v.NetPrice) * vatGroup.Key, 2, MidpointRounding.AwayFromZero);
            }

            return tax;
        }
    }

    public decimal GrossPriceTotal => this.NetPriceTotal + this.Tax;
}
