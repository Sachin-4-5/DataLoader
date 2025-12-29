using DataLoader.Logger;

namespace DataLoader.Archive
{
    public class ArchiveProcess
    {
        public void ArchiveFile(string sourceFilePath, string archiveDirectory)
        {
            try
            {
                if (!Directory.Exists(archiveDirectory))
                {
                    Directory.CreateDirectory(archiveDirectory);
                    ErrorLogger.Log(nameof(ArchiveProcess), "ArchiveFile", $"Archive directory created: {archiveDirectory}", ErrorLogger.LogLevel.INFO);
                }

                string fileName = Path.GetFileName(sourceFilePath);
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string archivedFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{timestamp}{Path.GetExtension(fileName)}";
                string archivedFilePath = Path.Combine(archiveDirectory, archivedFileName);
                File.Move(sourceFilePath, archivedFilePath);
                ErrorLogger.Log(nameof(ArchiveProcess), "ArchiveFile", $"File archived successfully: {archivedFilePath}", ErrorLogger.LogLevel.INFO);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(nameof(ArchiveProcess), "ArchiveFile", "Failed to archive input file", ErrorLogger.LogLevel.ERROR, ex);
                throw;
            }
        }
    }
}