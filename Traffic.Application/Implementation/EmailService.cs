using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Traffic.Utilities.Constants;
using Traffic.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Traffic.Utilities.Enums;
using Traffic.Application.Models.SendEmail;
using System.Net.Mail;
using System.Net;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mime;

namespace Traffic.Application.Interfaces
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public  Task SendEmail1(string email, string subject, string message)
        {
            SmtpClient client = new SmtpClient(_configuration["MailSettings:Server"])
            {
                UseDefaultCredentials = false,
                Port = int.Parse(_configuration["MailSettings:Port"]),
                EnableSsl = bool.Parse(_configuration["MailSettings:EnableSsl"]),
                Credentials = new NetworkCredential(_configuration["MailSettings:UserName"], _configuration["MailSettings:Password"])
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["MailSettings:FromEmail"], _configuration["MailSettings:FromName"]),
            };
            mailMessage.To.Add(email);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            client.Send(mailMessage);
            return Task.CompletedTask;
        }
        public async Task<Response> SendEmail2(string email, string subject, string message)
        {

            ////var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            var client = new SendGridClient("");
            var from = new EmailAddress("traffic@noreply.com", "Traffic project");
            ////var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress(email, "Traffic User");
            ////var plainTextContent = "and easy to do anywhere, even with C#";
            ////var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject,"", message);
            var response = await client.SendEmailAsync(msg);
            return response;
        }

        public Task SendEmail(string email, string subject, string message)
        {
            using (MailMessage mailMsg = new MailMessage())
            {
                
                mailMsg.To.Add(new MailAddress(email, "Traffic User"));
                mailMsg.From = new MailAddress("hanhan.nguyengia@gmail.com", "Admin Traffic project");
                mailMsg.Subject = subject;
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(message, null, MediaTypeNames.Text.Html));
                using (SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", 587))
                {
                    smtpClient.Credentials = new NetworkCredential("apikey", "");
                    smtpClient.Send(mailMsg);
                }
            }
            return Task.CompletedTask;
        }
      
    }
}
