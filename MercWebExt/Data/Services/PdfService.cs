namespace MercWebExt.Data.Services;
using System.IO;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;

public class PdfService
{
    private readonly IConverter _converter;

    public PdfService(IConverter converter)
    {
        _converter = converter;
    }

    public async Task<byte[]> GeneratePdfFromHtmlAsync(string htmlContent)
    {
        var globalSettings = new GlobalSettings
        {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A4,
            Margins = new MarginSettings { Top = 10 }
        };

        var objectSettings = new ObjectSettings
        {
            PagesCount = true,
            HtmlContent = htmlContent,
            WebSettings = { DefaultEncoding = "utf-8" }
        };

        var pdfDocument = new HtmlToPdfDocument
        {
            GlobalSettings = globalSettings,
            Objects = { objectSettings }
        };

        return await Task.Run(() => _converter.Convert(pdfDocument));
    }
}

