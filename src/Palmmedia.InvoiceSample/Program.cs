using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Palmmedia.InvoiceSample.Orders;
using Palmmedia.InvoiceSample.Pdf;
using s2industries.ZUGFeRD;
using s2industries.ZUGFeRD.PDF;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("log.txt")
    .CreateLogger();

var serviceCollection = new ServiceCollection();
ConfigureServices(serviceCollection);

var serviceProvider = serviceCollection.BuildServiceProvider();

var orderRenderer = serviceProvider.GetRequiredService<IOrderRenderer>();
var orderSettings = serviceProvider.GetRequiredService<IOptions<OrderSettings>>().Value;

var order = new Order()
{
    OrderNumber = "123",
    CustomerNumber = "DE965599654",
    CustomerReference = "987654321",
    CustomerName = "Some customer",
    Address = "Some street 123",
    ZipCode = "12345",
    City = "Some city",
    Country = "Germany",
    Email = "test@kunde.de",
    OrderItems = [
        new OrderItem()
        {
            ArticleNumber = "A123456",
            Description = "Some product",
            NetPrice = 100,
            VatRate = 0.07m,
        },
        new OrderItem()
        {
            ArticleNumber = "B123456",
            Description = "Some other product",
            NetPrice = 100,
            VatRate = 0.19m,
        }
    ]
};

byte[] pdf = await orderRenderer.CreatePdf(order);
File.WriteAllBytes("Order.pdf", pdf);

InvoiceDescriptor desc = InvoiceDescriptor.CreateInvoice(order.OrderNumber, order.CreatedAt, CurrencyCodes.EUR, "GE2020211-471102");
desc.BusinessProcess = "urn:fdc:peppol.eu:2017:poacc:billing:01:1.0";
desc.Name = "WARENRECHNUNG";
desc.ReferenceOrderNo = order.CustomerReference;
desc.AddNote($"Rechnung gemäß Bestellung Nr. {order.OrderNumber} vom {order.CreatedAt.ToShortDateString()}");
desc.SetBuyer(order.CustomerName, order.ZipCode, order.City, order.Address, CountryCodes.DE, null, new GlobalID(GlobalIDSchemeIdentifiers.GLN, "4000001123452"));
desc.AddBuyerTaxRegistration("DE234567890", TaxRegistrationSchemeID.VA);
desc.SetBuyerContact("Hans Muster");
desc.SetSeller("Lieferant GmbH", "80333", "München", "Lieferantenstraße 20", CountryCodes.DE, null, new GlobalID(GlobalIDSchemeIdentifiers.GLN, "4000001123452"));
desc.SetSellerContact("Frau Lieferant", null, "seller@test.de", "+49123456789");
desc.AddSellerTaxRegistration("201/113/40209", TaxRegistrationSchemeID.FC);
desc.AddSellerTaxRegistration("DE123456789", TaxRegistrationSchemeID.VA);
desc.SetBuyerOrderReferenceDocument("2013-471331", order.CreatedAt);
desc.SetDeliveryNoteReferenceDocument("2013-51111", order.CreatedAt);
desc.ActualDeliveryDate = order.CreatedAt;
desc.SetTotals(order.NetPriceTotal, null, null, order.NetPriceTotal, order.Tax, order.GrossPriceTotal, null, order.GrossPriceTotal, null);

foreach (var vatGroup in order.OrderItems.GroupBy(o => o.VatRate))
{
    desc.AddApplicableTradeTax(vatGroup.Sum(v => v.NetPriceTotal), vatGroup.Key * 100, vatGroup.Sum(v => v.Tax), TaxTypes.VAT, TaxCategoryCodes.S);
}

// desc.AddLogisticsServiceCharge(5.80m, "Versandkosten", TaxTypes.VAT, TaxCategoryCodes.S, 7m);
desc.AddTradePaymentTerms($"Zahlbar innerhalb 30 Tagen netto bis {order.CreatedAt.AddDays(30).ToShortDateString()}", order.CreatedAt.AddDays(30));

desc.SetPaymentMeans(PaymentMeansTypeCodes.SEPACreditTransfer, "Zahlung per Überweisung");
desc.AddCreditorFinancialAccount(orderSettings.Iban, orderSettings.Bic, bankName: orderSettings.Bank);

desc.SetSellerElectronicAddress("DE123456789", ElectronicAddressSchemeIdentifiers.GermanyVatNumber);
desc.SetBuyerElectronicAddress("LU987654321", ElectronicAddressSchemeIdentifiers.LuxemburgVatNumber);

foreach (var item in order.OrderItems)
{
    var tradeLineItem = desc.AddTradeLineItem(
        item.ArticleNumber,
        item.NetPrice,
        QuantityCodes.H87,
        item.Description,
        item.Quantity,
        item.GrossPrice,
        taxType: TaxTypes.VAT,
        categoryCode: TaxCategoryCodes.S,
        taxPercent: item.Tax);

    tradeLineItem.BilledQuantity = item.Quantity;
}

desc.Save("zugferd.xml", ZUGFeRDVersion.Version1, Profile.Basic);
desc.Save("xrechnung.xml", ZUGFeRDVersion.Version23, Profile.XRechnung);

await InvoicePdfProcessor.SaveToPdfAsync("zugferd-invoice.pdf", ZUGFeRDVersion.Version23, Profile.Comfort, ZUGFeRDFormats.CII, "Order.pdf", desc);

static void ConfigureServices(IServiceCollection services)
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    var configuration = builder.Build();

    services.AddLogging(configure => configure
        .AddConsole()
        .AddSerilog());

    services.AddSingleton<IConfiguration>(configuration);
    services.Configure<OrderSettings>(configuration.GetSection("OrderSettings"));

    services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
    services.AddSingleton<IPdfGenerator, PdfGenerator>();
    services.AddSingleton<IOrderRenderer, OrderRenderer>();
}