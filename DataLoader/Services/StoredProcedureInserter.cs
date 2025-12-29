using System.Data;
using DataLoader.Interfaces;
using DataLoader.Models;
using DataLoader.Templates;
using DataLoader.Logger;
using Microsoft.Data.SqlClient;

namespace DataLoader.Services
{
    public class StoredProcedureInserter : IDataInserter
    {
        public void Insert(Transaction transaction, DataLoadTemplate template, string connectionString, string sourceFileName)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                using var command = new SqlCommand(template.StoredProcedureName, connection);
                command.CommandType = CommandType.StoredProcedure;
                foreach (var column in template.Columns)
                {
                    var value = GetPropertyValue(transaction, column.Name);
                    command.Parameters.AddWithValue("@" + column.ParamName, value ?? DBNull.Value);
                }

                // Optional: pass filename to SP
                if (template.FileNameAsParam)
                {
                    command.Parameters.AddWithValue("@FileName", sourceFileName);
                }
                connection.Open();
                command.ExecuteNonQuery();
                ErrorLogger.Log(nameof(StoredProcedureInserter), nameof(Insert), $"Inserted Trade_Num={transaction.Trade_Num}", ErrorLogger.LogLevel.INFO);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(nameof(StoredProcedureInserter), nameof(Insert), "Database insertion failed", ErrorLogger.LogLevel.ERROR, ex);
                throw;
            }
        }

        private object? GetPropertyValue(Transaction transaction, string propertyName)
        {
            return transaction
                .GetType()
                .GetProperty(propertyName)
                ?.GetValue(transaction);
        }
    }
}