using MercWebExt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MercWebExt.Controllers
{
    public class PdfController : Controller
    {
        private readonly IContextService _context;
        private readonly ILogger<PdfController> _logger;

        public PdfController(ILogger<PdfController> logger, IContextService context)
        {
            _logger = logger;
            _context = context;
        }

        public void GeneratePdf()
        { 
             /*Generate PDF 
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = title
                // Save file to Local Hard disk drive
                // Out = @"D:\PDF\Report.pdf"
            };

            //List<InductionQuestion> headers = await _context.InductionQuestion.Where(w => w.IsActivated.Equals(true)).OrderBy(o => o.Qnumber).ToListAsync();
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.MakeInduction(headers, answer),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/css/", "style-pdf.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Left= "Date : [date]", Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Mercorella Group"}
            };
            
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);
            */
            
            //string subject = answer.RegoNumber + "_" + answer.LastName + " " + answer.FirstName;
            //string emailMessage = "Induction:  \n\nPlease find attached induction file.\n\nKind Regards\n\n" + answer.FirstName + " " + answer.LastName;
                          
            //SendInduction(from, to, subject, name, message, file)
            //await _emailSender.SendInductionAsync(from, to, subject, emailName, emailMessage, file);
                        
            // Return to Show Pdf file on web
            // return File(file, "application/pdf");
        }
    }

}