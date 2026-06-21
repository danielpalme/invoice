using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Palmmedia.InvoiceSample.Pdf
{
    internal class PuppeteerSharpPdfGenerator : IPdfGenerator
    {
        private static readonly string[] ChromePaths = [
            "/usr/bin/google-chrome",
            "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"];

        public async Task<byte[]> GeneratePdfFromHtmlAsync(string html)
        {
            using var browser = await Puppeteer.LaunchAsync(new()
            {
                Headless = true,
                ExecutablePath = GetChromePath(),
                Args = [
                    //"--no-sandbox", // Required when running Chromium as a non-root user inside Docker
                    //"--disable-setuid-sandbox", // Disables the setuid-specific sandbox mechanism
                    "--disable-dev-shm-usage", // Docker limits /dev/shm to 64 MB by default; this tells Chrome to use /tmp instead, preventing crashes
                    "--disable-gpu"
                ]
            });

            using var page = await browser.NewPageAsync();
            await page.SetContentAsync(html);
            byte[] pdf = await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                MarginOptions = new MarginOptions() { Top = "0", Right = "0", Bottom = "0", Left = "0" }
            });
            return pdf;
        }

        private static string GetChromePath()
        {
            foreach (var path in ChromePaths)
            {
                if (System.IO.File.Exists(path))
                {
                    return path;
                }
            }

            throw new FileNotFoundException("Google Chrome or Chromium executable not found.");
        }
    }
}
