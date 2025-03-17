using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;  //It provides built-in methods like - MailMessage, smtpClient, etc.
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;

namespace Mailer
{
    public class MailSender
    {
        public static void SendMail(string fromAddress, string toAddress, string subject, string body, string footer, bool isBodyHtml, string attachmentPath = null)
        {
            if (!IsValidEmail(fromAddress) || !IsValidEmail(toAddress))
                throw new ArgumentException("Invalid email address format.");

            StringBuilder strBody = new StringBuilder(body).Append(footer);

            using (MailMessage mailer = new MailMessage(fromAddress, toAddress))
            {
                mailer.IsBodyHtml = isBodyHtml;
                mailer.Subject = subject;
                mailer.Body = strBody.ToString();

                if (!string.IsNullOrEmpty(attachmentPath))
                {
                    if (System.IO.File.Exists(attachmentPath))
                    {
                        Attachment attachment = new Attachment(attachmentPath);
                        mailer.Attachments.Add(attachment);
                    }
                    else
                    {
                        throw new System.IO.FileNotFoundException("Attachment file not found: " + attachmentPath);
                    }
                }

                using (SmtpClient client = new SmtpClient())
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailGroupId"], ConfigurationManager.AppSettings["EmailPassword"]);
                    client.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                    client.Host = ConfigurationManager.AppSettings["Host"];
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.EnableSsl = true;
                    client.Timeout = 30000;


                    // Hardcoded credentials for testing
                    /*
                    client.Credentials = new NetworkCredential("sachinkumar778866@gmail.com", "lhqf aavy kaln lexi");
                    client.Port = 587;
                    client.Host = "smtp.gmail.com";
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.EnableSsl = true;
                    client.Timeout = 30000;
                    */

                    try
                    {
                        client.Send(mailer);
                    }
                    catch (SmtpException smtpEx)
                    {
                        // Log smtpEx.Message if needed
                        throw new ApplicationException("Error sending email.", smtpEx);
                    }
                }
            }
        }



        private static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
        }



        public static string ValidatePath(params string[] paths)
        {
            string filePath = System.IO.Path.Combine(paths);
            for (int i = 0; i < paths.Length - 1; i++)
            {
                paths[i] = System.Web.HttpUtility.UrlEncode(paths[i]);
                if (paths[i].IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > -1)
                {
                    throw new System.IO.FileNotFoundException("FileName not valid");
                }
            }
            return filePath;
        }
    }
}
