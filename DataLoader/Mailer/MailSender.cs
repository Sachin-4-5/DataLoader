using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using DataLoader.Logger;

namespace DataLoader.Mailer
{
    public class MailSender
    {
        private readonly IConfiguration _configuration;
        public MailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMail(string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient
                {
                    Host = _configuration["Email:SmtpHost"],
                    Port = int.Parse(_configuration["Email:SmtpPort"]),
                    EnableSsl = bool.Parse(_configuration["Email:EnableSsl"]),
                    Credentials = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"])
                };

                var mail = new MailMessage
                {
                    From = new MailAddress(_configuration["Email:From"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };

                mail.To.Add(_configuration["Email:To"]);
                smtpClient.Send(mail);
                ErrorLogger.Log(nameof(MailSender), "SendMail", "Email sent successfully", ErrorLogger.LogLevel.INFO);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(nameof(MailSender), "SendMail", "Failed to send email", ErrorLogger.LogLevel.ERROR, ex);
            }
        }
    }
}