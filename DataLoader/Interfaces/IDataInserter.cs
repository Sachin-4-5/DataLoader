using DataLoader.Models;
using DataLoader.Templates;

namespace DataLoader.Interfaces
{
    public interface IDataInserter
    {
        void Insert(Transaction transaction, DataLoadTemplate template, string connectionString, string sourceFileName);
    }
}