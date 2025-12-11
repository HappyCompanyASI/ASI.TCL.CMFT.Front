using System;
using Serilog;
using Serilog.Events;

namespace ASI.TCL.CMFT.WPF.Logger
{
    public class SerilogService : ILogService
    {
        public SerilogService()
        {
            var outputTemplate =
                "================================={NewLine}" +
                "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}]{NewLine}" +
                "================================={NewLine}" +
                "{Message:lj}{NewLine}{Exception}{NewLine}";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                    .WriteTo.File(
                        "Logs\\info-.txt",
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 30,
                        outputTemplate: outputTemplate
                    ))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
                    .WriteTo.File(
                        "Logs\\warn-.txt",
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 30,
                        outputTemplate: outputTemplate
                    ))

                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                    .WriteTo.File(
                        "Logs\\error-.txt",
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 30,
                        outputTemplate: outputTemplate
                    ))

                .CreateLogger();

        }

        public void Info(string message)
        {
            Log.Information(message);
        }

        public void Warn(string message)
        {
            Log.Warning(message);
        }

        public void Error(string message, Exception ex = null)
        {
            if (ex != null)
                Log.Error(ex, message);
            else
                Log.Error(message);
        }
    }
}
