using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Mailer;
using ErrorLogger;

namespace DataLoader
{
    public class PreProcessor
    {
        private static readonly string MailBody = ConfigurationManager.AppSettings["MailBody"];
        private static readonly string ExeLogPath = string.Format(ConfigurationManager.AppSettings["ExeLogPath"], DateTime.Now.ToString("yyyyMMdd"));
        private static readonly string ServerName = ConfigurationManager.AppSettings["ServerName"];
        private static readonly string AlertMailId = ConfigurationManager.AppSettings["AlertMailId"];
        private static readonly string AlertMailSubject = ConfigurationManager.AppSettings["MailSubject"];
        private static readonly string FromMail = "sachinkumarrana1005@gmail.com";

        private DataLoadInfo objDLResult = new DataLoadInfo();

        public void Import(string strTemplatePath)
        {
            LogInfo("Template loading started.");

            try
            {
                DataLoadTemplate objDataLoadTemplate = new DataLoadTemplate(strTemplatePath);
                DataLoader objDataLoader = new DataLoader();
                ProcessXmlFile(objDataLoader, objDataLoadTemplate);
            }
            catch (Exception ex)
            {
                HandleError("Error occurred during XML import", ex);
            }
        }

        private void ProcessXmlFile(DataLoader objDataLoader, DataLoadTemplate objDataLoadTemplate)
        {
            try
            {
                objDataLoader.XmlFileLoader(objDataLoadTemplate);
                LogInfo("XML file processing completed successfully.");
            }
            catch (Exception ex)
            {
                HandleError("Failed to process XML file.", ex);
            }
        }

        private void HandleError(string message, Exception ex)
        {
            LogError(message, ex);
            SendAlertEmail(AlertMailId, AlertMailSubject + " - Failure", ex);
        }

        private void LogInfo(string message) =>
            ErrorLog.WriteErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, message, ErrorLog.Loglevel.INFO.ToString());

        private void LogError(string message, Exception ex) =>
            ErrorLog.WriteErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, $"{message}: {ex.Message}", ErrorLog.Loglevel.ERROR.ToString());

        private void SendAlertEmail(string toEmail, string subject, Exception ex)
        {
            string body = $"{MailBody}</br>Error: {ex.Message}</br>Check log file for more info: {ExeLogPath}";
            for (int attempt = 0; attempt < 2; attempt++)
            {
                try
                {
                    Mailer.MailSender.SendMail(FromMail, toEmail, subject, body, "</br></br>Thanks,</br>TFA FIS", true);
                    break;
                }
                catch (Exception emailEx)
                {
                    LogError("Failed to send alert email", emailEx);
                }
            }
        }
    }
}
