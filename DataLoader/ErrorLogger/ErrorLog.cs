using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ErrorLogger
{
    public class ErrorLog
    {
        //properties
        private static string strLogFilePath = ConfigurationManager.AppSettings["ExeLogPath"].ToString().Trim();
        private static StreamWriter sw = null;


        //Constructor
        public ErrorLog() 
        { 
        }


        public enum Loglevel
        {
            ERROR = 0,
            DEBUG = 1,
            INFO = 2,
            DATA = 3
        }


        public static bool WriteErrorLog(string strSource, string strMethodName, string strLogMessage, string strLogType, Exception objException)
        {
            bool bReturn = false;
            string strException = string.Empty;
            int iLogLevel = 0;

            try
            {
                ErrorLog objErrorLogger = new ErrorLog();
                lock (objErrorLogger)
                {
                    if (String.Equals(strLogType.Trim(), "Error", StringComparison.OrdinalIgnoreCase)) {
                        iLogLevel = (int)Loglevel.ERROR;
                    }
                    else if (String.Equals(strLogType.Trim(), "DEBUG", StringComparison.OrdinalIgnoreCase)) {
                        iLogLevel = (int)Loglevel.DEBUG;
                    }
                    else if (String.Equals(strLogType.Trim(), "INFO", StringComparison.OrdinalIgnoreCase)) {
                        iLogLevel = (int)Loglevel.INFO;
                    }
                    else if (String.Equals(strLogType.Trim(), "DATA", StringComparison.OrdinalIgnoreCase)) {
                        iLogLevel = (int)Loglevel.DATA;
                    }

                    if (iLogLevel <= 4)
                    {
                        string strPathName = GetLogFilePath();
                        using (FileStream fs = new FileStream(strPathName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            using (sw = new StreamWriter(fs))
                            {
                                sw.WriteLine("[" + strLogType.Trim() + "] [" + DateTime.Now.ToString("MM/dd/yyyy") + " "
                                    + DateTime.Now.ToLongTimeString() + "] Source      : "
                                    + strSource.Trim() + " :: " + strMethodName.Trim());
                                sw.WriteLine("Log Message: " + strLogMessage.Trim());
                                sw.WriteLine("Error: " + objException.Message.ToString().Trim());
                                sw.WriteLine("Stack Trace: " + objException.StackTrace.ToString().Trim());
                            }
                        }
                    }
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                bReturn = false;
                throw ex;
            }
            return bReturn;
        }


        public static bool WriteErrorLog(string strSource, string strMethodName, string strLogMessage, string strLogType)
        {
            bool bReturn = false;
            string strException = string.Empty;
            int iLogLevel = 0;

            try
            {
                ErrorLog objErrorLogger = new ErrorLog();
                lock (objErrorLogger)
                {
                    if (String.Equals(strLogType.Trim(), "Error", StringComparison.OrdinalIgnoreCase))
                    {
                        iLogLevel = (int)Loglevel.ERROR;
                    }
                    else if (String.Equals(strLogType.Trim(), "Error", StringComparison.OrdinalIgnoreCase))
                    {
                        iLogLevel = (int)Loglevel.DEBUG;
                    }
                    else if (String.Equals(strLogType.Trim(), "Error", StringComparison.OrdinalIgnoreCase))
                    {
                        iLogLevel = (int)Loglevel.INFO;
                    }
                    else if (String.Equals(strLogType.Trim(), "Error", StringComparison.OrdinalIgnoreCase))
                    {
                        iLogLevel = (int)Loglevel.DATA;
                    }

                    if (iLogLevel <= 4)
                    {
                        string strPathName = GetLogFilePath();
                        using (FileStream fs = new FileStream(strPathName, FileMode.Append, FileAccess.Write, FileShare.Read))
                        {
                            using (sw = new StreamWriter(fs))
                            {
                                sw.WriteLine("[" + strLogType.Trim() + "] [" + DateTime.Now.ToString("MM/dd/yyyy") + " "
                                    + DateTime.Now.ToLongTimeString() + "] Source      : "
                                    + strSource.Trim() + " :: " + strMethodName.Trim());
                                sw.WriteLine("Log Message: " + strLogMessage.Trim());
                                sw.WriteLine("^^-----------------------------------------------------------------^^");
                            }
                        }
                    }
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                bReturn = false;
                throw ex;
            }
            return bReturn;
        }



        /// <summary>
        /// Check the log file in application directory, if not exists create new one.
        /// </summary>
        /// <returns></returns>
        private static string GetLogFilePath()
        {
            try
            {
                //get the base directcory
                string baseDir = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.RelativeSearchPath;

                //search the file below the current directory
                string retFilePath = "";
                if (strLogFilePath == null || strLogFilePath.Equals(string.Empty))
                {
                    retFilePath = baseDir + "//" + DateTime.Now.ToShortDateString();
                }
                else
                {
                    retFilePath = string.Format(strLogFilePath, DateTime.Now);
                }

                //create the directory if not exists
                if (!Directory.Exists(Path.GetDirectoryName(retFilePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(retFilePath));
                }
                return retFilePath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
