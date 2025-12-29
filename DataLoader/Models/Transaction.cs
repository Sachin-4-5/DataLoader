// POCO Class
// Represents one row from xml

namespace DataLoader.Models
{
    public class Transaction
    {
        public int Trade_Num { get; set; }
        public DateTime Trade_Date { get; set; }
        public string Trade_Currency { get; set; } = string.Empty;
        public decimal Trade_Price { get; set; }
        public DateTime Trade_Maturity { get; set; }
        public string Trade_Status { get; set; } = string.Empty;
    }
}