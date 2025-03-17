using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mailer;
using System.IO;
using ErrorLogger;


namespace DataLoader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[] { @"D:\Projects\Console Applications (Legacy)\ProdApps\Templates\Transaction_file_loader.xml" };
            }


            var methodName = MethodBase.GetCurrentMethod().Name;
            var className = MethodBase.GetCurrentMethod().DeclaringType.Name;


            try
            {
                Console.WriteLine("********** Execution Started, Please Wait.. **********");
                ErrorLog.WriteErrorLog(className, methodName, "Execution Started", ErrorLog.Loglevel.INFO.ToString());
                Console.WriteLine("START TIME: " + DateTime.Now.ToLongTimeString() + Environment.NewLine);

                foreach (string strPath in args)
                {
                    if (strPath.EndsWith(".xml") && File.Exists(strPath))
                    {
                        PreProcessor objExport = new PreProcessor();
                        objExport.Import(strPath);
                    }
                    else
                    {
                        Console.WriteLine($"Skipping invalid or non-XML file: {strPath}");
                    }
                }

                Console.WriteLine("EXECUTION SUCCESSFULLY COMPLETED." + Environment.NewLine);
                ErrorLog.WriteErrorLog(className, methodName, "Execution Successfully completed.", ErrorLog.Loglevel.INFO.ToString());

                Console.WriteLine("END TIME: " + DateTime.Now.ToLongTimeString());
                Console.WriteLine("******************************************************");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message + ex.StackTrace);
                ErrorLog.WriteErrorLog(className, methodName, "Execution failed: " + ex.Message, ErrorLog.Loglevel.ERROR.ToString());
                Environment.Exit(1);
            }

            Environment.Exit(0);
        }
    }
}
