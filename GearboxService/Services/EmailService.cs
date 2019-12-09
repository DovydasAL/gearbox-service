using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using GearboxService.Models;
using Microsoft.Extensions.Configuration;

namespace GearboxService.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public void SendEmail(List<RedeemResponse> responses)
        {
            if (Boolean.Parse(_config["SendEmails"]))
            {
                MailMessage message = new MailMessage();  
                SmtpClient smtp = new SmtpClient();  
                message.From = new MailAddress(_config["LoggingFromEmail"]);  
                message.To.Add(new MailAddress(_config["LoggingToEmail"]));  
                message.Subject = "Shift Redemption Service: " + DateTime.Now.ToLongTimeString();  
                message.IsBodyHtml = true;
                message.Body = BuildBody(responses);  
                smtp.Port = Int32.Parse(_config["SMTPPort"]);  
                smtp.Host = _config["SMTPAddress"];
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;  
                smtp.Credentials = new NetworkCredential(_config["LoggingFromEmail"], _config["LoggingFromEmailPassword"]);  
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;  
                smtp.Send(message); 
            }
        }

        private string BuildBody(List<RedeemResponse> responses)
        {
            var body = new StringBuilder();
            body.Append("<h2>Shift Redemption Service</h2>");
            body.Append("<p>Here is how your last redemption went:</p><ul>");
            foreach (var response in responses)
            {
                body.Append($"<li>{response.Code}: {response.Content}</li>");
            }

            body.Append("</ul>");
            return body.ToString();
        }
    }
}