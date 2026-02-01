using System.Globalization;
using QRCoder;

namespace Palmmedia.InvoiceSample.Orders;

public class GiroCodeGenerator
{
    public static byte[] GenerateGiroCode(OrderSettings orderSettings, decimal amount, string subject)
    {
        string text = string.Format(
            @"BCD
001
1
SCT
{0}
{1}
{2}
EUR{3}


{4}
",
            orderSettings.Bic,
            orderSettings.BankAccountOwner,
            orderSettings.Iban,
            amount.ToString("0.00", CultureInfo.InvariantCulture),
            subject);

        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q))
        using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
        {
            return qrCode.GetGraphic(20);
        }
    }
}