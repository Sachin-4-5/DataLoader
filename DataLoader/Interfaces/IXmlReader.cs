// Loose coupling
// Future - ready(CSV / JSON reader later)

using DataLoader.Models;
namespace DataLoader.Interfaces
{
    public interface IXmlReader
    {
        List<Transaction> Read(string filePath, string iterationXPath);
    }
}