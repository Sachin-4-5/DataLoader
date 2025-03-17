using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mailer;
using ErrorLogger;

namespace DataLoader
{
    public class ArchiveProcess
    {
        static string strExeLogPath = string.Format(ConfigurationManager.AppSettings["ExeLogPath"].ToString(), DateTime.Today.ToString("yyyyMMdd"));
        static string strDataLoaderArchPath = ConfigurationManager.AppSettings["ArchivePath"].ToString();

        public static void Archive(string strFilePath)
        {
            try
            {
                // Log the beginning of the archiving process
                ErrorLog.WriteErrorLog(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "Archiving: " + strFilePath, ErrorLog.Loglevel.INFO.ToString());

                // Ensure the archive directory exists
                string archivePath = ArchivePath(strDataLoaderArchPath);
                if (!Directory.Exists(archivePath))
                {
                    Directory.CreateDirectory(archivePath);
                }

                // Define the destination path with a timestamp to avoid overwrites
                string destinationPath = ArchiveFile(strFilePath, archivePath);
                File.Copy(strFilePath, destinationPath, true);
                File.Delete(strFilePath);

                // Log completion
                Console.WriteLine($"Archived file to: {destinationPath}");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "Error Occurred: " + ex.Message, ErrorLog.Loglevel.ERROR.ToString());
            }
        }

        public static DataLoadInfo Archive(string strFilePath, string strArchivePath, DataLoadInfo dlInfo)
        {
            try
            {
                // Log the beginning of the archiving process
                ErrorLog.WriteErrorLog(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "Archiving: " + strFilePath, ErrorLog.Loglevel.INFO.ToString());

                // Ensure the archive directory exists
                string archivePath = ArchivePath(strArchivePath);
                if (!Directory.Exists(archivePath))
                {
                    Directory.CreateDirectory(archivePath);
                }

                // Define the destination path with a timestamp to avoid overwrites
                string destinationPath = ArchiveFile(strFilePath, archivePath);
                File.Copy(strFilePath, destinationPath, true);
                File.Delete(strFilePath);

                // Update DataLoadInfo with the archive path
                dlInfo.ArchivePath = archivePath;
                dlInfo.IsArchived = true;

                return dlInfo;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "Error Occurred: " + ex.Message, ErrorLog.Loglevel.ERROR.ToString());
                dlInfo.IsArchived = false;
                return dlInfo;
            }
        }

        private static string ArchivePath(string strPath)
        {
            return string.Format(strPath, DateTime.Today.ToString("yyyyMMdd"));
        }

        private static string ArchiveFile(string strInFilePath, string strArcPath)
        {
            // Appends only the current date to the file name
            return string.Format(strArcPath.TrimEnd('\\') + "\\" + Path.GetFileNameWithoutExtension(strInFilePath) + "_{0}" + Path.GetExtension(strInFilePath), DateTime.Now.ToString("yyyyMMdd"));


            // Appends the current timestamp to the archived file name
            //return Path.Combine(strArcPath, Path.GetFileNameWithoutExtension(strInFilePath) + $"_{DateTime.Now:HHmmss}" + Path.GetExtension(strInFilePath));
        }
    }
}
