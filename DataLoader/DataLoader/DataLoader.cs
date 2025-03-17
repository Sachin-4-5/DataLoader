using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Mailer;


namespace DataLoader
{
    public class DataLoader
    {
        string strConnection = ConfigurationManager.ConnectionStrings["TransactionDB"].ConnectionString;    //Set your actual connection string here

        public DataLoader()
        {
            //
        }

        public void XmlFileLoader(DataLoadTemplate objDataLoadTemplate)
        {
            string filePath = objDataLoadTemplate.Url;
            XmlDocument doc1 = new XmlDocument();
            doc1.Load(filePath);    //Load the XML document

            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Ignore    //Ensures DTDs are ignored
            };

            DataSet ds = new DataSet();
            using (XmlReader readerFeed = XmlReader.Create(filePath, settings))
            {
                ds.ReadXml(readerFeed);
            }

            foreach (DataTable table in ds.Tables)
            {
                Console.WriteLine("Table Name: " + table.TableName);
            }

            // Assuming the table with TRADE data is named "TRADE"
            if (ds.Tables.Contains("TRADE"))
            {
                InsertDataUsingStoredProcedure(ds.Tables["TRADE"], objDataLoadTemplate.SPName, filePath);
                SendSuccessEmail(filePath);
            }
            else
            {
                Console.WriteLine("No TRADE data found in the XML.");
            }
            ArchiveProcess.Archive(filePath);
        }


        private void InsertDataUsingStoredProcedure(DataTable tradeTable, string storedProcedureName, string filePath)
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                connection.Open();
                foreach (DataRow row in tradeTable.Rows)
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Map the DataRow fields to the stored procedure parameters
                        command.Parameters.AddWithValue("@TD_NUM", row["TD_NUM"]);
                        command.Parameters.AddWithValue("@TRD_TRADE_DATE", row["TRD_TRADE_DATE"]);
                        command.Parameters.AddWithValue("@TRD_CURRENCY", row["TRD_CURRENCY"]);
                        command.Parameters.AddWithValue("@TRD_PRICE", row["TRD_PRICE"]);
                        command.Parameters.AddWithValue("@TRD_PRINCIPAL", Convert.ToDecimal(row["TRD_PRINCIPAL"]));
                        command.Parameters.AddWithValue("@TRD_SETTLE_DATE", row["TRD_SETTLE_DATE"]);
                        command.Parameters.AddWithValue("@TRD_STATUS", row["TRD_STATUS"]);
                        command.Parameters.AddWithValue("@COUNTERPARTY_CODE", row["COUNTERPARTY_CODE"]);
                        command.Parameters.AddWithValue("@CUSIP", row["CUSIP"]);
                        command.Parameters.AddWithValue("@FUND", row["FUND"]);
                        command.Parameters.AddWithValue("@INVNUM", row["INVNUM"]);
                        command.Parameters.AddWithValue("@MATURITY", row["MATURITY"]);
                        command.Parameters.AddWithValue("@TICKER", row["TICKER"]);
                        command.Parameters.AddWithValue("@TOUCH_COUNT", row["TOUCH_COUNT"]);
                        command.Parameters.AddWithValue("@TRAN_TYPE", row["TRAN_TYPE"]);

                        // Additional parameters
                        //command.Parameters.AddWithValue("@FILE_DATE", row["FILE_DATE"]); // Assuming this is part of your XML data
                        //command.Parameters.AddWithValue("@ReInsert", DBNull.Value); // Set it to NULL by default

                        // Execute the command
                        command.ExecuteNonQuery();
                    }
                }

                // Archive the XML file after successful data load and delete from original path location.
                ArchiveProcess.Archive(filePath);
                Console.WriteLine($"File '{filePath}' has been successfully archived.");
            }
        }


        private void SendSuccessEmail(string filePath)
        {
            string fromAddress = ConfigurationManager.AppSettings["FromEmail"];
            string toAddress = ConfigurationManager.AppSettings["ToEmail"];
            string subject = "Data Load Successful";
            string body = $"The XML file '{filePath}' has been successfully loaded into the database.";
            string footer = "<br><br>Thank you,<br>Your Team"; // Add any footer content if needed
            bool isBodyHtml = true;

            try
            {
                MailSender.SendMail(fromAddress, toAddress, subject, body, footer, isBodyHtml);
                Console.WriteLine("Success email sent.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }
}
