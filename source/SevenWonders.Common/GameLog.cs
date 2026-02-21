using Serilog;
using Serilog.Core;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace SevenWonders.Common
{
    public static class GameLog
    {
        static GameLog()
        {
            Log.Logger = Logger.None;
        }

        public static void Info(string message, [CallerMemberName]string methodName = "")
        {
            Log.Information($"[{methodName}] {message}");
        }

        public static void Debug(string message, [CallerMemberName] string methodName = "")
        {
            Log.Debug($"[{methodName}] {message}");
        }

        public static void Error(string message, [CallerMemberName] string methodName = "")
        {
            Log.Error($"[{methodName}] {message}");
        }

        public static void Fatal(string message, [CallerMemberName] string methodName = "")
        {
            Log.Fatal($"[{methodName}] {message}");
        }

        public static void Warning(string message, [CallerMemberName] string methodName = "")
        {
            Log.Warning($"[{methodName}] {message}");
        }

        public static void Verbose(string message, [CallerMemberName] string methodName = "")
        {
            Log.Verbose($"[{methodName}] {message}");
        }

        public static void InitializeFileLogger()
        {
            var logFileName = ConfigurationManager.AppSettings["logFileName"];

            if (logFileName is null || string.IsNullOrWhiteSpace(logFileName))
            {
                throw new InvalidOperationException("Log file name is null!");
            }

            var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", logFileName);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 50, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }
    }
}
