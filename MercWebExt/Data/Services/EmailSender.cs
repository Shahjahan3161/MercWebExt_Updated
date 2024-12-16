using System;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using MercWebExt.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using static MercWebExt.Services.EmailSender;


namespace MercWebExt.Services
{
    public interface IEmailSender
    {
        void SendEmail(string from, string email, string subject, string message);

        void SendInduction(string from, string email, string subject, string emailName, string message, byte[] file);

        void SendToDriver(string from, string email, string subject, string message, byte[] qrcode);

        void SendToVisitor(string from, string email, string subject, string message, byte[] qrcode);

        void SendApplication(string from, string email, string subject, string emailName, string message, IFormFile file);

        void EmailWithFile(string from, string email, string subject, string message, MemoryStream file, string fileName);
        //Task<IActionResult> SendInvoiceEmail(string from, string email, string subject, string message, byte[] qrcode);
    }

    public class EmailSender : IEmailSender
    {

        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<EmailSettings> emailSettings, ILogger<EmailSender> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public EmailSettings _emailSettings { get; }

        //[HttpPost]
        //public async Task<IActionResult> SendInvoiceEmail(string from, string email, string subject, string message, byte[] pdfBytes)
        //{
        //    try
        //    {
        //        var apiKey = "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"; // Replace with your SendGrid API key
        //        var client = new SendGridClient(apiKey);
        //        var fromEmail = new EmailAddress("contact@appvizta.com", "Mercorella Group");
        //        var to = new EmailAddress(email, "Recipient Name");
        //        var plainTextContent = "Please find attached your invoice.";
        //        var htmlContent = message;

        //        // Create the email
        //        var msg = MailHelper.CreateSingleEmail(fromEmail, to, subject, plainTextContent, htmlContent);

        //        // Attach the PDF file
        //        msg.AddAttachment("InductionCertificate.pdf", Convert.ToBase64String(pdfBytes));

        //        // Send the email
        //        var response = await client.SendEmailAsync(msg);

        //        if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Accepted)
        //        {
        //            return new JsonResult("Invoice sent successfully!");
        //        }
        //        else
        //        {
        //            return new JsonResult(new { message = "Failed to send invoice." });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log any exceptions
        //        _logger.LogError($"An error occurred: {ex.Message}");
        //        return new JsonResult("Error");
        //    }
        //}


        public void SendEmail(string from, string email, string subject, string message)
        {
            Execute(from, email, subject, message);
            return;
        }

        public void SendInduction(string from, string email, string subject, string emailName, string message, byte[] file)
        {
            try
            {
                string toEmail = string.IsNullOrEmpty(email)
                                 ? _emailSettings.ToEmail
                                 : email;
                string fromEmail = string.IsNullOrEmpty(from)
                                 ? _emailSettings.FromEmail
                                 : from;

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(fromEmail, emailName)
                };
                mail.To.Add(new MailAddress(toEmail));

                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = false;
                mail.Priority = MailPriority.Normal;
                //mail.Attachments.Add(new Attachment(new MemoryStream(file), subject + "_Induction.pdf"));

                using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            catch (SmtpFailedRecipientException SX)
            {
                _logger.LogError(SX.StackTrace);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
            }

            return;
        }

        public void SendToDriver(string from, string email, string subject, string message, byte[] file)
        {
            try
            {
                string fromEmail = string.IsNullOrEmpty(from)
                                 ? _emailSettings.UsernameEmail
                                 : from;

                string emailMerc = "email@mercorella.com.au";
                //string ccEmail = "Jodie.Watson@mercorella.com.au";
                //string ccEmail = "mercorella.cloud@gmail.com";

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(fromEmail, "Mercorella Group")
                };

                foreach (var address in email.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(address);
                }

                //mail.CC.Add(new MailAddress(ccEmail));
                mail.Bcc.Add(new MailAddress(emailMerc));

                _logger.LogError(mail.To.ToString());

                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = message;
                mail.Priority = MailPriority.Normal;
                //mail.Attachments.Add(new Attachment(new MemoryStream(file), subject + ".pdf"));

                using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    //smtp.EnableSsl = false;
                    smtp.Send(mail);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
            }

            return;
        }

        public void SendToVisitor(string from, string email, string subject, string message, byte[] file)
        {
            try
            {
                string fromEmail = string.IsNullOrEmpty(from)
                                 ? _emailSettings.UsernameEmail
                                 : from;

                string emailMerc = "email@mercorella.com.au";
                string ccEmail = "Jodie.Watson@mercorella.com.au";
                //string ccEmail = "mercorella.cloud@gmail.com";

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(fromEmail, "Mercorella Group")
                };

                foreach (var address in email.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(address);
                }

                mail.CC.Add(new MailAddress(ccEmail));
                mail.Bcc.Add(new MailAddress(emailMerc));

                _logger.LogError(mail.To.ToString());

                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = message;
                mail.Priority = MailPriority.Normal;
                //mail.Attachments.Add(new Attachment(new MemoryStream(file), subject + ".pdf"));

                using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    //smtp.EnableSsl = false;
                    smtp.Send(mail);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
            }

            return;
        }

        public void SendApplication(string from, string email, string subject, string emailName, string message, IFormFile file)
        {
            emailApplication(from, email, subject, emailName, message, file);
            return;
        }

        public void Execute(string from, string email, string subject, string message)
        {
            try
            {
                string toEmail = string.IsNullOrEmpty(email)
                                 ? _emailSettings.ToEmail
                                 : email;
                string fromEmail = string.IsNullOrEmpty(from)
                                 ? _emailSettings.FromEmail
                                 : email;

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(fromEmail, "Mercorella Group")
                };
                mail.To.Add(new MailAddress(toEmail));

                //string ccEmail = "Jodie.Watson@mercorella.com.au";
                //mail.CC.Add(new MailAddress(_emailSettings.CcEmail));

                if (subject.Contains("Enquire"))
                {
                    string bccEmail = "Allon.Song@mercorella.com.au";
                    mail.Bcc.Add(new MailAddress(bccEmail));
                }
                
                string ccEmail = "MMerc@mercorella.com.au";
                mail.CC.Add(new MailAddress(ccEmail));

                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.Normal;

                using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    smtp.EnableSsl =true;
                    smtp.Send(mail);
                }
            }
            catch (SmtpFailedRecipientException SX)
            {
                _logger.LogError(SX.StackTrace);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
            }
        }


        public void emailApplication(string from, string email, string subject, string emailName, string message, IFormFile file)
        {
            try
            {   
                string toEmail = string.IsNullOrEmpty(email)
                                 ? _emailSettings.ToEmail
                                 : email;

                //string emailMerc = "email@mercorella.com.au";
                string emailLoc = "loctrinh@mercorella.com.au";

                string fromEmail = string.IsNullOrEmpty(from)
                                 ? _emailSettings.FromEmail
                                 : from;

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(fromEmail, emailName)
                };
                mail.To.Add(new MailAddress(toEmail));
                mail.Bcc.Add(new MailAddress(emailLoc));

                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = false;
                mail.Priority = MailPriority.Normal;

                //mail.Attachments.Add(new Attachment(file.OpenReadStream(), file.FileName));

                using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }

            }
            catch (SmtpFailedRecipientException SX)
            {
                _logger.LogError(SX.StackTrace);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
            }
        }

        public void EmailWithFile(string from, string to, string subject, string message, MemoryStream file, string fileName)
        {
            try
            {
                string fromEmail = string.IsNullOrEmpty(from)
                                 ? _emailSettings.UsernameEmail
                                 : from;

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(fromEmail, "Mercorella Group")
                };

                foreach (var address in to.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(address);
                }

                string emailMerc = "email@mercorella.com.au";
                string ccEmail = "Jodie.Watson@mercorella.com.au";

                _logger.LogError(mail.To.ToString());

                //mail.CC.Add(new MailAddress(_emailSettings.CcEmail));

                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.Normal;
                //mail.Attachments.Add(new Attachment(file, fileName));

                using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    //smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
            }
        }

       

        //End Methods
    }
}
