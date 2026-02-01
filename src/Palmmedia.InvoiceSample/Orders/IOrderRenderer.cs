namespace Palmmedia.InvoiceSample.Orders;

public interface IOrderRenderer
{
    Task<byte[]> CreatePdf(Order order);
}
