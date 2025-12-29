using System.Globalization;
using System.Xml;
using DataLoader.Interfaces;
using DataLoader.Logger;
using DataLoader.Models;

namespace DataLoader.Services
{
    public class XmlTransactionParser : IXmlReader
    {
        public List<Transaction> Read(string filePath, string iterationXPath)
        {
            var transactions = new List<Transaction>();
            try
            {
                ErrorLogger.Log(nameof(XmlTransactionParser), nameof(Read), $"Reading XML file: {filePath}", ErrorLogger.LogLevel.INFO);

                var doc = new XmlDocument();
                doc.Load(filePath);

                var xpath = "//" + iterationXPath.Replace("\\", "/");
                XmlNodeList? tradeNodes = doc.SelectNodes(xpath);
                if (tradeNodes == null || tradeNodes.Count == 0)
                {
                    ErrorLogger.Log(nameof(XmlTransactionParser), nameof(Read), "No TRADE nodes found.", ErrorLogger.LogLevel.INFO);
                    return transactions;
                }

                foreach (XmlNode node in tradeNodes)
                {
                    var transaction = new Transaction
                    {
                        Trade_Num = TryParseInt(node["Trade_Num"]?.InnerText),
                        Trade_Date = TryParseDate(node["Trade_Date"]?.InnerText),
                        Trade_Currency = node["Trade_Currency"]?.InnerText ?? string.Empty,
                        Trade_Price = TryParseDecimal(node["Trade_Price"]?.InnerText),
                        Trade_Maturity = TryParseDate(node["Trade_Maturity"]?.InnerText),
                        Trade_Status = node["Trade_Status"]?.InnerText ?? string.Empty
                    };
                    transactions.Add(transaction);
                }
                ErrorLogger.Log(nameof(XmlTransactionParser), nameof(Read), $"Total transactions parsed: {transactions.Count}", ErrorLogger.LogLevel.INFO);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(nameof(XmlTransactionParser), nameof(Read), "Error while parsing XML file.", ErrorLogger.LogLevel.ERROR, ex);
                throw;
            }
            return transactions;
        }

        
        // Helper Methods
        private static int TryParseInt(string? value) => int.TryParse(value, out var result) ? result : 0;
        private static decimal TryParseDecimal(string? value) => decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : 0m;
        private static DateTime TryParseDate(string? value)
            => DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : DateTime.MinValue;
    }
}