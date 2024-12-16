using DinkToPdf;
using DinkToPdf.Contracts;
using iTextSharp.text.pdf.qrcode;
using MercWebExt.Data.Services;
using MercWebExt.Models.DataBase;
using MercWebExt.Models.ViewModels;
using MercWebExt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 // Updated namespace
using QRCoder;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;



namespace MercWebExt.Controllers
{
    public class InductionController : Controller
    {
        private IEmailSender _emailSender;
        private readonly PdfService _pdfService;
        private readonly DatabaseContext _dbContext;
        private readonly IContextService _context;
        private readonly ILogger<InductionController> _logger;

        [Obsolete]
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private IConverter _converter;


        // [Obsolete]
        public InductionController(IEmailSender emailSender, ILogger<InductionController> logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConverter converter, 
            IContextService context, PdfService pdfService, DatabaseContext databaseContext)
        {
            _emailSender = emailSender;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _converter = converter;
            _context = context;
            _pdfService = pdfService;   
            _dbContext = databaseContext;
        }

        public IActionResult Induction()
        {
            return View();
        }

        // GET: Induction
        public IActionResult VisitorInductionTest()
        {
            ViewInduction viewModel = new ViewInduction();

            viewModel.InductionQuestions = new List<InductionQuestion>();

            viewModel.InductionQuestions = _context.GetDataBaseContext().InductionQuestion.Where(w => w.IsActivated.Equals(true) && w.Category.Equals("Visitor")).OrderBy(o => o.Qnumber).ToList();
            viewModel.InductionQuestions.AddRange(_context.GetDataBaseContext().InductionQuestion.Where(w => w.IsActivated.Equals(true) && w.Category.Equals("ForkLift")).OrderBy(o => o.Qnumber).ToList());
            viewModel.InductionQuestions.AddRange(_context.GetDataBaseContext().InductionQuestion.Where(w => w.IsActivated.Equals(true) && w.Category.Equals("Response")).OrderBy(o => o.Qnumber).ToList());

            return View(viewModel);
        }

        // GET: Induction
        public IActionResult VisitorInduction()
        {
            ViewInduction viewModel = new ViewInduction();

            viewModel.InductionQuestions = new List<InductionQuestion>();

            viewModel.InductionQuestions = _context.GetDataBaseContext().InductionQuestion.Where(w => w.IsActivated.Equals(true) && w.Category.Equals("Visitor")).OrderBy(o => o.Qnumber).ToList();
            viewModel.InductionQuestions.AddRange(_context.GetDataBaseContext().InductionQuestion.Where(w => w.IsActivated.Equals(true) && w.Category.Equals("ForkLift")).OrderBy(o => o.Qnumber).ToList());
            viewModel.InductionQuestions.AddRange(_context.GetDataBaseContext().InductionQuestion.Where(w => w.IsActivated.Equals(true) && w.Category.Equals("Response")).OrderBy(o => o.Qnumber).ToList());

            return View(viewModel);
        }

        // GET: Induction
        public IActionResult DriverInduction()
        {
            ViewInduction viewModel = new ViewInduction();

            viewModel.InductionQuestions = new List<InductionQuestion>();
            viewModel.InductionQuestions = _context.GetDataBaseContext().InductionQuestion.Where(w => w.IsActivated.Equals(true) && w.Category.Equals("Driver")).OrderBy(o => o.Qnumber).ToList();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CheckInductionAnswers([FromBody] List<AnswerViewModel> selectedAnswers)
        {
            // Fetch correct answers from the database based on the question IDs provided
            var correctAnswers = _context.GetDataBaseContext().InductionQuestion
                                         .Where(q => selectedAnswers.Select(s => s.QuestionId).Contains(q.Qid))
                                         .ToList();

            bool isCorrect = true;

            foreach (var answer in selectedAnswers)
            {
                var correctAnswer = correctAnswers.FirstOrDefault(q => q.Qid == answer.QuestionId);
                if (correctAnswer != null)
                {
                    var mappedAnswer = MapAnswer(answer.Answer);

                    // Compare the submitted answer with the correct answer from the database
                    if (mappedAnswer != correctAnswer.isCorrectAnswer.ToString())
                    {
                        isCorrect = false;
                        break;
                    }
                }
            }

            return Json(new { isCorrect });
        }

        // Helper method to map front-end answers to database values (True, False, etc.)
        private string MapAnswer(string answer)
        {
            switch (answer.ToUpper())
            {
                case "TRUE":
                    return "1";
                case "FALSE":
                    return "2";
                case "NONE":
                    return "3";
                default:
                    return "0";
            }
        }

      
        public class AnswerViewModel
        {
            public int QuestionId { get; set; }
            public string Answer { get; set; }
        }


        // GET: Induction Report List
        [Authorize(Roles = "Site Manager, Admin")]
        public IActionResult InductionList()
        {
            List<InductionInduction> inductionList = new List<InductionInduction>();
            
            inductionList = _context.GetDataBaseContext().InductionInduction.ToList();
            
            ViewReportInduction viewModel = new ViewReportInduction(inductionList);

            return View(viewModel);
        }

        // GET: Induction Report List
        [Authorize(Roles = "Site Manager, Admin")]
        public IActionResult InductionUpdate()
        {
            List<InductionInduction> inductionList = new List<InductionInduction>();

            inductionList = _context.GetDataBaseContext().InductionInduction.ToList();

            ViewReportInduction viewModel = new ViewReportInduction(inductionList);

            return View(viewModel);
        }

        // POST : Select Specific Date
        [HttpPost]
        public void UpdateInduction(string paraString)
        {
            int userId = int.Parse(paraString.Split("_")[0]);
            string company = paraString.Split("_")[1];

            if(_context.GetDataBaseContext().InductionInduction.Any(a=>a.Iid.Equals(userId)))
            {
                InductionInduction induction = _context.GetDataBaseContext().InductionInduction.First(f=>f.Iid.Equals(userId));

                induction.Company = company;

                _context.GetDataBaseContext().InductionInduction.Update(induction);
                _logger.LogError("Saved : " + _context.GetDataBaseContext().SaveChanges());
            }
            
            _logger.LogError("Get Data : " +userId + "," + company);

            return;
        }


     

        // POST : Select Specific Date
        [HttpPost]
        public IActionResult ReportDriverList(ViewReportInduction postData)
        {
            ViewReportInduction returnModel = new ViewReportInduction();

            DateTime selectDate = postData.selectDate;

            returnModel.driverList = _context.GetDataBaseContext().InductionInduction.Where(w => w.DateCreated.Date.Equals(selectDate.Date)).ToList();

            return View("InductionList", returnModel);
        }


        // POST : Edit Linked Menu by Json Data
        [HttpPost]
        public IActionResult CheckInduction(InductionInduction induction)
        {

            if (!string.IsNullOrEmpty(induction.DriverEmail))
            {
                _logger.LogError("Induction Category : [" + induction.Category + "] Email Address : " + induction.DriverEmail);

                if (induction.Category.Equals("Driver"))
                {
                    SendInductionAsync(induction);
                }
                else if (induction.Category.Equals("Visitor"))
                {
                    SendVisitorInduction(induction);
                }

                return new JsonResult("Okay");
            }
            else
            {
                return new JsonResult("Wrong Email");
            }
        }
        // POST : Edit Linked Menu by Json Data
        [HttpPost]
        public IActionResult ExternalInduction(InductionInduction induction)
        {
            // Save induction to DB
            _context.GetDataBaseContext().InductionInduction.Add(induction);
            _context.GetDataBaseContext().SaveChanges();

            // logging and message
            _logger.LogInformation("Save Induction, " + DateTime.Now.ToShortDateString() + " by " + induction.LastName + " " + induction.FirstName);

            return Json("Sucess");
        }
        // POST : Edit Linked Menu by Json Data
        [HttpPost]
        public IActionResult IsRegistered(string plate)
        {
            // Save induction to DB
            bool result = _context.GetDataBaseContext().InductionInduction.Any(a=>a.RegoNumber.Equals(plate));

            return Json(result);
        }
        // POST : Edit Linked Menu by Json Data
        [HttpPost]
        public IActionResult FindUserByPlate(string plate)
        {
            // Any QR Details
            string result = "Not registerd user";

            if(_context.GetDataBaseContext().InductionInduction.Any(a => a.RegoNumber.Equals(plate)))
            {
                result = _context.GetDataBaseContext().InductionInduction.Where(w=>w.RegoNumber.Equals(plate)).First().UserId.ToString();
            }

            return Json(result);
        }
        // VOID : Send Email
        public async Task SendInductionAsync(InductionInduction induction)
        {
            // Save induction to DB
            DateTime timeSync = DateTime.Now;
            induction.DateCreated = timeSync;

            _context.GetDataBaseContext().InductionInduction.Add(induction);
            _context.GetDataBaseContext().SaveChanges();

            if (_context.GetDataBaseContext().InductionInduction.Any(x => x.DateCreated.Equals(timeSync)))
            {
                // Get induction from DB and Send Email
                var driverInduction = _context.GetDataBaseContext().InductionInduction
                    .First(w => w.DateCreated.Equals(timeSync));

                string driverEmail = driverInduction.DriverEmail;
                string from = "ext@mercorella.com.au";
                string emailName = driverInduction.FirstName + " " + driverInduction.LastName;
                string subject = emailName + " STG" + driverInduction.Iid + " Card#" + induction.UserId;

                // Generate PDF HTML content
                //string pdfHtmlContent = PDFgenerator.MakePDF(driverInduction, null); // Assuming qrCode is null for now, replace it with actual qrCode if needed
                string pdfHtmlContent = "sdsdsf";
                // Generate PDF from HTML
               // var pdfBytes = await _pdfService.GeneratePdfFromHtmlAsync(pdfHtmlContent);

                // Email Message
                string emailMessage = "<b>St George Produce Driver Site Induction </b><br /><br />" +
                    "This is to certify that <b>" + emailName + "</b> has successfully completed the St George Produce Driver Site Induction.<br /><br />" +
                    "Certificate #:<b> STG" + driverInduction.Iid + "</b><br />" +
                    "Card #:<b>" + induction.UserId + "</b><br />" +
                    "Date Issued: <b>" + DateTime.Now.ToString("dd MMM yyyy") + "</b><br /><br /><br />" +
                    "Note:<br />" +
                    "&#8226;You must present this QR pass once you reach the gate (print out or this email on your phone).<br />" +
                    "&#8226;You will be given a red card on your first visit for future site entry.<br />" +
                    "&#8226;You must carry the attached QR pass with you at all times while onsite.<br />" +
                    "&#8226;You must work in a safe manner at all times.<br />" +
                    "&#8226;You must wear HiViz clothing and Approved Safety Footwear when onsite.<br />" +
                    "&#8226;Make sure you know what to do and where to go in case of an emergency evacuation.<br />" +
                    "&#8226;Stay near your vehicle during loading/unloading.<br /><br />" +
                    "<b>[THIS IS AN AUTOMATED MESSAGE - PLEASE DO NOT REPLY DIRECTLY TO THIS EMAIL]</b>";

                // Send the email
                await SendInvoiceEmail(from, driverEmail, subject, emailMessage);

                // logging and message
                _logger.LogInformation("Sent Induction at " + DateTime.Now.ToShortDateString() + " by " + induction.DriverEmail);
            }
            else
            {
                // logging and message
                _logger.LogError("Cannot send Email.");
            }
        }


        [HttpPost]
        public async Task<IActionResult> SendInvoiceEmail(string from, string email, string subject, string message)
        {
            try
            {
                var apiKey = "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"; // Replace with your SendGrid API key
                var client = new SendGridClient(apiKey);
                var fromEmail = new EmailAddress("contact@appvizta.com", "Mercorella Group");
                var to = new EmailAddress(email, "Recipient Name");
                var plainTextContent = "Please find attached your invoice.";
                var htmlContent = message;

                // Create the email
                var msg = MailHelper.CreateSingleEmail(fromEmail, to, subject, plainTextContent, htmlContent);

                // Attach the PDF file
                //msg.AddAttachment("InductionCertificate.pdf", Convert.ToBase64String(pdfBytes));

                // Send the email
                var response = await client.SendEmailAsync(msg);

                if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    return new JsonResult("Invoice sent successfully!");
                }
                else
                {
                    return new JsonResult(new { message = "Failed to send invoice." });
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions
                _logger.LogError($"An error occurred: {ex.Message}");
                return new JsonResult("Error");
            }
        }


        // VOID : Send Email
        public void SendVisitorInduction(InductionInduction induction)
        {
            // Save induction to DB
            DateTime timeSync = DateTime.Now;

            InductionInduction newInduction = new InductionInduction();
            newInduction = GetNewInduction(induction, timeSync);

            _context.GetDataBaseContext().InductionInduction.Add(newInduction);
            _context.GetDataBaseContext().SaveChanges();

            if(_context.GetDataBaseContext().InductionInduction.Any(x=>x.DateCreated.Equals(timeSync)))
            {
                // Get induction from DB and Send Email
                InductionInduction visitorInduction = new InductionInduction();
                visitorInduction = _context.GetDataBaseContext().InductionInduction.Where(w=>w.DateCreated.Equals(timeSync)).First();

                string driverEmail = visitorInduction.DriverEmail;
                string from = "Ext@mercorella.com.au";
                //string emailMerc = "email@mercorella.com.au";
                string emailName = visitorInduction.FirstName + " " + visitorInduction.LastName;
               // byte[] qrCode = QrGenerator(visitorInduction.UserId);
                //Email Title
                string subject = emailName + " STG" + visitorInduction.Iid + " Card#" + visitorInduction.UserId;

             //   var file = CreatePDF(visitorInduction, qrCode);

                //Email Message
                string emailMessage = "<b>Visitor/Contractor Induction </b><br /><br />" +
                               "This is to certified that <b>" + emailName + "</b> has successfully completed the Visitor/Contractor Induction.<br /><br />" +
                               "Email :" + visitorInduction.DriverEmail + "<br />" +
                               "Phone #:" + visitorInduction.DriverMobile + "<br />" +
                               "Company :" + visitorInduction.Company + "<br />";

                if (!string.IsNullOrEmpty(visitorInduction.ForkliftNo))
                {
                    emailMessage = emailMessage + "High Risk Licence #: " + visitorInduction.ForkliftNo + "<br />";
                }

                if (!string.IsNullOrEmpty(visitorInduction.Comments))
                {
                    emailMessage = emailMessage + "Comments : " + visitorInduction.Comments + "<br />";
                }

                emailMessage = emailMessage + "<br />Certificate #:<b> STG" + visitorInduction.Iid + "</b><br />" +
                               "Card #:<b>" + visitorInduction.UserId + "</b><br />" +
                               "Issued Date : <b>" + DateTime.Now.ToString("dd MMM yyyy") + "</b><br /><br /><br />" +
                               "Note:<br />" +
                               "You must show this email once you reach the gate (print out or this email on your phone).<br />" +
                               "You will be given a red card on your first visit for future site entry.<br />" +
                               "You must carry the attached QR pass with you at all times while onsite.<br />" +
                               "You must work in a safe manner at all times.<br />" +
                               "You must wear HiViz clothing and Approved Safety Footwear when onsite.<br />" +
                               "Make sure you know what to do and where to go in case of an emergency evacuation.<br />" +
                               "Stay near your vehicle during loading/unloading.<br /><br />" +
                               "<b>[THIS IS AN AUTOMATED MESSAGE - PLEASE DO NOT REPLY DIRECTLY TO THIS EMAIL]</b>";

                //SendToVisitor(from, to, subject, message, file)
                //   _emailSender.SendToVisitor(from, driverEmail, subject, emailMessage, file);
                 SendInvoiceEmail(from, driverEmail, subject, emailMessage);

                // logging and message
                _logger.LogInformation("Sent Induction at  " + DateTime.Now.ToShortDateString() + " by " + visitorInduction.DriverEmail);
            }
            else
            {
                // logging and message
                _logger.LogError("Cannot send Email.");
            }
        }
        //public byte[] QrGenerator(string qrText)
        //{
        //    QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //    QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
        //    // Adjusted to pass both QRCodeData and size to the QRCode constructor
        //    QRCode qrCode = new QRCode(qrCodeData, 20); // Assuming 20 is the desired size in pixels
        //                                                // Use Generate method instead of GetGraphic
        //    Bitmap qrCodeImage = qrCode.Generate();

        //    return BitmapToBytes(qrCodeImage);
        //}


        //public static Byte[] BitmapToBytes(Bitmap img)
        //{
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        img.Save(stream, System.DrawingCore.Imaging.ImageFormat.Png);
        //        return stream.ToArray();
        //    }

        //}
        public byte[] CreatePDF(InductionInduction induction, byte[] qrCode)
        {
            //Generate PDF 
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Card #"+induction.UserId+", "+induction.FirstName +" " + induction.LastName
                // Save file to Local Hard disk drive
                // Out = @"D:\PDF\Report.pdf"
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = PDFgenerator.MakePDF(induction, qrCode),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/css/", "style-pdf.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Left = "Date : [date]", Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Mercorella Group" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);

            //return Byte[]
            return file;

            //return File(file, "application/pdf");

        }
        public InductionInduction GetNewInduction(InductionInduction baseInduction, DateTime timeSync)
        {
            InductionInduction newInduction = new InductionInduction();
            newInduction.DateCreated = timeSync;
            newInduction.LastName = baseInduction.LastName;
            newInduction.FirstName = baseInduction.FirstName;
            newInduction.DriverEmail = baseInduction.DriverEmail;

            if(!string.IsNullOrEmpty(baseInduction.DriverMobile))
            {
                newInduction.DriverMobile = baseInduction.DriverMobile;
            }

            if (!string.IsNullOrEmpty(baseInduction.RegoNumber))
            {
                newInduction.RegoNumber = baseInduction.RegoNumber;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr1))
            {
                newInduction.Qr1 = baseInduction.Qr1;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr2))
            {
                newInduction.Qr2 = baseInduction.Qr2;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr3))
            {
                newInduction.Qr3 = baseInduction.Qr3;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr4))
            {
                newInduction.Qr4 = baseInduction.Qr4;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr5))
            {
                newInduction.Qr5 = baseInduction.Qr5;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr6))
            {
                newInduction.Qr6 = baseInduction.Qr6;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr7))
            {
                newInduction.Qr7 = baseInduction.Qr7;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr8))
            {
                newInduction.Qr8 = baseInduction.Qr8;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr9))
            {
                newInduction.Qr9 = baseInduction.Qr9;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr10))
            {
                newInduction.Qr11 = baseInduction.Qr11;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr12))
            {
                newInduction.Qr12 = baseInduction.Qr12;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr13))
            {
                newInduction.Qr13 = baseInduction.Qr13;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr14))
            {
                newInduction.Qr14 = baseInduction.Qr14;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr15))
            {
                newInduction.Qr15 = baseInduction.Qr15;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr16))
            {
                newInduction.Qr16 = baseInduction.Qr16;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr17))
            {
                newInduction.Qr17 = baseInduction.Qr17;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr18))
            {
                newInduction.Qr18 = baseInduction.Qr18;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr19))
            {
                newInduction.Qr19 = baseInduction.Qr19;
            }

            if (!string.IsNullOrEmpty(baseInduction.Qr20))
            {
                newInduction.Qr20 = baseInduction.Qr20;
            }

            if (!string.IsNullOrEmpty(baseInduction.Fr1))
            {
                newInduction.Fr1 = baseInduction.Fr1;
            }

            if (!string.IsNullOrEmpty(baseInduction.Fr2))
            {
                newInduction.Fr2 = baseInduction.Fr2;
            }

            if (!string.IsNullOrEmpty(baseInduction.Fr3))
            {
                newInduction.Fr3 = baseInduction.Fr3;
            }

            if (!string.IsNullOrEmpty(baseInduction.Fr4))
            {
                newInduction.Fr4 = baseInduction.Fr4;
            }

            if (!string.IsNullOrEmpty(baseInduction.Fr5))
            {
                newInduction.Fr5 = baseInduction.Fr5;
            }

            if (!string.IsNullOrEmpty(baseInduction.Fr6))
            {
                newInduction.Fr6 = baseInduction.Fr6;
            }

            if (!string.IsNullOrEmpty(baseInduction.Fr7))
            {
                newInduction.Fr7 = baseInduction.Fr7;
            }

            if (!string.IsNullOrEmpty(baseInduction.Fr8))
            {
                newInduction.Fr8 = baseInduction.Fr8;
            }

            if (!string.IsNullOrEmpty(baseInduction.UserId))
            {
                newInduction.UserId = baseInduction.UserId;
            }

            if (!string.IsNullOrEmpty(baseInduction.ForkliftNo))
            {
                newInduction.ForkliftNo = baseInduction.ForkliftNo;
            }

            if (!string.IsNullOrEmpty(baseInduction.Comments))
            {
                newInduction.Comments = baseInduction.Comments;
            }

            if (!string.IsNullOrEmpty(baseInduction.Category))
            {
                newInduction.Category = baseInduction.Category;
            }

            if (!string.IsNullOrEmpty(baseInduction.Company))
            {
                newInduction.Company = baseInduction.Company;
            }

            if (!string.IsNullOrEmpty(baseInduction.Response1))
            {
                newInduction.Response1 = baseInduction.Response1;
            }

            if (!string.IsNullOrEmpty(baseInduction.Response2))
            {
                newInduction.Response2 = baseInduction.Response2;
            }

            return newInduction;
        }

    }
}
