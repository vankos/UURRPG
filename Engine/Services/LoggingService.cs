using System;
using System.IO;

namespace Engine.Services
{
    public static class LoggingService
    {
        private const string LOG_FILE_DIRECTORY = "Logs";

        static LoggingService()
        {
            string logDir = Path.Combine(Environment.CurrentDirectory, LOG_FILE_DIRECTORY);
            Directory.CreateDirectory(logDir);
        }

        public static void Log(Exception exception, bool isInnerException = false)
        {
            using (StreamWriter sw = new StreamWriter(LogFileName, true))
            {
                sw.Write(new string('\n', isInnerException ? 1 : 2));
                sw.WriteLine(isInnerException ? "INNER EXCEPTION" : $"EXCEPTION:{DateTime.Now}");
                sw.WriteLine(new string(isInnerException ? '-' : '=', 40));
                sw.WriteLine(exception.Message);
                sw.WriteLine(exception.StackTrace);
            }

            if (exception.InnerException != null)
                Log(exception.InnerException, true);
        }

        private static string LogFileName => Path.Combine(Environment.CurrentDirectory, LOG_FILE_DIRECTORY, $"UURRPG_{DateTime.Today:ddMMyyyy}.log");
    }
}
