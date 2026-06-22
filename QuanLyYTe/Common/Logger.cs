using System;
using System.IO;

namespace QuanLyYTe.Common
{
    public static class Logger
    {
        private static readonly string LogDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static readonly string LogFile = Path.Combine(LogDir, "app_error.log");

        public static void LogError(Exception ex, string context = "")
        {
            try
            {
                if (!Directory.Exists(LogDir))
                {
                    Directory.CreateDirectory(LogDir);
                }

                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR\n";
                if (!string.IsNullOrEmpty(context))
                {
                    logEntry += $"Context: {context}\n";
                }
                logEntry += $"Message: {ex.Message}\n";
                logEntry += $"StackTrace: {ex.StackTrace}\n";
                
                if (ex.InnerException != null)
                {
                    logEntry += $"Inner Exception: {ex.InnerException.Message}\n";
                    logEntry += $"Inner StackTrace: {ex.InnerException.StackTrace}\n";
                }
                logEntry += new string('-', 80) + "\n";

                File.AppendAllText(LogFile, logEntry);
            }
            catch
            {
                // Suppress logging errors to prevent app crash
            }
        }
    }
}
