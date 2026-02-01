using System.Reflection;
using System.Runtime.InteropServices;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace Palmmedia.InvoiceSample.Pdf;

public class PdfGenerator : IPdfGenerator
{
    private readonly IConverter converter;

    public PdfGenerator(IConverter converter)
    {
        this.converter = converter;
    }

    public async Task<byte[]> GeneratePdfFromHtmlAsync(string html)
    {
        await DownloadNativeDependencyForPdfGenerationAsync();

        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings(0, 0, 0, 0)
                },
            Objects =
                {
                    new ObjectSettings()
                    {
                        PagesCount = true,
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" },
                        FooterSettings = { FontSize = 9, Center = string.Empty }
                    }
                }
        };

        byte[] pdf = this.converter.Convert(doc);

        return pdf;
    }

    private static async Task DownloadNativeDependencyForPdfGenerationAsync()
    {
        string? file = null;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            file = "libwkhtmltox.dll";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            file = "libwkhtmltox.so";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            file = "libwkhtmltox.dylib";
        }

        if (file == null)
        {
            throw new InvalidOperationException("Not supported OS");
        }

        string targetFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, file);
        if (!File.Exists(targetFile))
        {
            string? url = null;
            if (Environment.Is64BitProcess)
            {
                url = "https://github.com/rdvojmoc/DinkToPdf/raw/master/v0.12.4/64%20bit/" + file;
            }
            else if (RuntimeInformation.OSArchitecture == Architecture.X86)
            {
                url = "https://github.com/rdvojmoc/DinkToPdf/raw/master/v0.12.4/32%20bit/" + file;
            }

            if (url == null)
            {
                throw new InvalidOperationException("Not supported OSArchitecture");
            }

            using (var client = new HttpClient())
            {
                using (var stream = await client.GetStreamAsync(url))
                using (var fs = new FileStream(targetFile, FileMode.Create))
                {
                    await stream.CopyToAsync(fs);
                }
            }
        }

        new CustomAssemblyLoadContext().LoadUnmanagedLibrary(targetFile);
    }
}