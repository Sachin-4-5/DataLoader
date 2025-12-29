namespace DataLoader.Logger
{
    public static class ErrorLogger
    {
        private static readonly object _lock = new();
        private static string _logFilePath = string.Empty;

        public static void Initialize(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public enum LogLevel
        {
            ERROR,
            INFO,
            DEBUG
        }

        public static void Log(string source, string method, string message, LogLevel level, Exception? exception = null)
        {
            if (string.IsNullOrEmpty(_logFilePath))
                return;

            try
            {
                lock (_lock)
                {
                    var filePath = string.Format(_logFilePath, DateTime.Now);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                    var log = $"[{level}] [{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                              $"Source: {source} :: {method}{Environment.NewLine}" +
                              $"Message: {message}{Environment.NewLine}";

                    if (exception != null)
                    {
                        log += $"Error: {exception.Message}{Environment.NewLine}" +
                               $"Stack: {exception.StackTrace}{Environment.NewLine}";
                    }
                    log += "------------------------------------------" + Environment.NewLine;
                    File.AppendAllText(filePath, log);
                }
            }
            catch
            {
                // swallow logging errors
            }
        }
    }
}
