namespace Palmmedia.InvoiceSample.Pdf;

public interface IPdfGenerator
{
    Task<byte[]> GeneratePdfFromHtmlAsync(string html);
}