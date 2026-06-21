using System.Text;
using Microsoft.Extensions.Options;
using Palmmedia.InvoiceSample.Pdf;

namespace Palmmedia.InvoiceSample.Orders;

public class OrderRenderer(
    IPdfGenerator pdfGenerator,
    IOptions<OrderSettings> optionsAccessor)
    : IOrderRenderer
{
    private readonly IPdfGenerator pdfGenerator = pdfGenerator;

    private readonly OrderSettings orderSettings = optionsAccessor.Value;

    public async Task<byte[]> CreatePdf(Order order)
    {
        StringBuilder sb = new();

        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("<style type=\"text/css\">");
        sb.AppendLine($"body {{ font-family: 'Arial', sans-serif; padding: 2cm; font-size: .8rem; }}");
        sb.AppendLine("table { width: 100%; margin-bottom: 20px; border-spacing: 0; border-collapse: collapse; }");
        sb.AppendLine("th { text-align: left; font-size: .8rem; }");
        sb.AppendLine("th, td { margin:0; padding: 5px; vertical-align: top; font-size: .8rem; }");
        sb.AppendLine(".table-bordered th, .table-bordered td { border: 1px solid #5A5A5A; padding: 7px; }");
        sb.AppendLine(".noborder { border: none !important; }");
        sb.AppendLine(".text-center { text-align: center; }");
        sb.AppendLine(".text-end { text-align: right; }");
        sb.AppendLine(".nowrap { white-space: nowrap; }");
        sb.AppendLine("</style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");

        sb.AppendLine($"<br/><br/><br/>{order.CustomerName}<br />{order.Address}<br />{order.ZipCode} {order.City}<br />{order.Country}");

        sb.AppendLine("<br/><br/><br/><br/><br/><br/>");
        sb.AppendLine($"<div class=\"text-end\">{order.CreatedAt.ToShortDateString()}</div>");
        sb.AppendLine("<br/>");
        sb.AppendLine($"<div><b>Order number: {order.OrderNumber}</b></div>");
        sb.AppendLine("<br/>");
        sb.AppendLine($"<div><b>Customer reference</b>: {order.CustomerReference ?? "-"}</div>");

        sb.AppendLine("<br/><br/>");

        sb.AppendLine("<table class=\"table table-bordered\">");
        sb.AppendLine("<thead>");
        sb.AppendLine("<tr>");
        sb.AppendLine("<th>Article number</th>");
        sb.AppendLine("<th>Description</th>");
        sb.AppendLine("<th class=\"text-end\">Quantity</th>");
        sb.AppendLine($"<th class=\"text-end\">Price</th>");
        sb.AppendLine("</tr>");
        sb.AppendLine("</thead>");
        sb.AppendLine("<tbody>");

        foreach (var orderItem in order.OrderItems!)
        {
            sb.AppendLine("<tr>");
            sb.AppendLine($"<td>{orderItem.ArticleNumber}</td>");
            sb.AppendLine($"<td>{orderItem.Description}</td>");
            sb.AppendLine($"<td class=\"text-end\">{orderItem.Quantity}</td>");
            sb.AppendLine($"<td class=\"text-end nowrap\">{orderItem.NetPrice.ToString("f2")} €</td>");
            sb.AppendLine("</tr>");
        }

        sb.AppendLine("</tbody>");

        sb.AppendLine("<tfoot>");
        sb.AppendLine("<tr>");
        sb.AppendLine("<td colspan=\"2\" class=\"noborder\"></td>");
        sb.AppendLine($"<td class=\"text-end noborder\">Total costs net</td>");
        sb.AppendLine($"<td class=\"text-end nowrap\">{order.NetPriceTotal.ToString("f2")} €</td>");
        sb.AppendLine("</tr>");

        foreach (var vatGroup in order.OrderItems.GroupBy(o => o.VatRate))
        {
            sb.AppendLine("<tr>");
            sb.AppendLine("<td colspan=\"2\" class=\"noborder\"></td>");
            sb.AppendLine($"<td class=\"text-end noborder\">{string.Format("plus {0}% VAT", vatGroup.Key * 100)}</td>");
            sb.AppendLine($"<td class=\"text-end nowrap\">{vatGroup.Sum(v => v.Tax).ToString("f2")} €</td>");
            sb.AppendLine("</tr>");
        }

        sb.AppendLine("<tr>");
        sb.AppendLine("<td colspan=\"2\" class=\"noborder\"></td>");
        sb.AppendLine($"<th class=\"text-end noborder\">Total costs gross</th>");
        sb.AppendLine($"<th class=\"text-end nowrap\">{order.GrossPriceTotal.ToString("f2")} €</th>");
        sb.AppendLine("</tr>");

        sb.AppendLine("</tfoot>");
        sb.AppendLine("</table>");

        sb.AppendLine("<br/><br/>");

        string subject = $"Order_{order.OrderNumber}";
        byte[] giroCodePng = GiroCodeGenerator.GenerateGiroCode(
            this.orderSettings,
            order.GrossPriceTotal,
            subject);

        sb.AppendLine("<table>");
        sb.AppendLine("<tr>");
        sb.AppendLine("<td>");
        sb.AppendLine("<br/>");
        sb.AppendLine($"<div><b>Recipient</b>: {this.orderSettings.BankAccountOwner}</div>");
        sb.AppendLine($"<div><b>IBAN</b>: {this.orderSettings.Iban}</div>");
        sb.AppendLine($"<div><b>BIC</b>: {this.orderSettings.Bic}</div>");
        sb.AppendLine($"<br/>");
        sb.AppendLine($"<div><b>Subject</b>: {subject}</div>");
        sb.AppendLine("</td>");
        sb.AppendLine($"<td class=\"text-center\" style=\"width: 150px;\"><img src=\"data:image/png;base64,{Convert.ToBase64String(giroCodePng)}\" width=\"150\" /><br/>GiroCode</td>");
        sb.AppendLine("</tr>");
        sb.AppendLine("</table>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        var result = await this.pdfGenerator.GeneratePdfFromHtmlAsync(sb.ToString());
        return result;
    }
}
