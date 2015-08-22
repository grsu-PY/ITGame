using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace ITGame.Infrastructure.Logging
{
    public class Logger : ILogger
    {
        private LogWriter DefaultLogger => _defaulLogger ?? (_defaulLogger = CreateDefaultEventLogWriter());

        private LogWriter LogWriterInstance
        {
            get
            {
                if (_logWriter == null) Configure();
                return _logWriter ?? DefaultLogger;
            }
        }

        private readonly string _configFilePath;
        private LogWriter _logWriter;
        private LogWriter _defaulLogger;

        /// <summary>
        /// Initializes a new logger instance using the current executing assembly's configuration
        /// </summary>
        public Logger()
        {
            _configFilePath = null;
        }

        /// <summary>
        /// Initializes a new logger instance using configuraction file specified by the <paramref name="configFilePath"/>
        /// </summary>
        /// <param name="configFilePath">A path to the configuration file that contains Logging Application Block configuration section</param>
        public Logger(string configFilePath)
        {
            _configFilePath = configFilePath;
        }

        public void LogStart(string message = null)
        {
            WriteLogInternal($"Log Start. {message ?? string.Empty}", TraceEventType.Start);
        }

        public void LogStop(string message = null)
        {
            WriteLogInternal($"Log Stop. {message ?? string.Empty}", TraceEventType.Stop);
        }

        public void LogVerbose(string message)
        {
            WriteLogInternal(message, TraceEventType.Verbose);
        }

        private void WriteLogInternal(string message, TraceEventType severity)
        {
            var logEntry = new LogEntry()
            {
                Message = message,
                Severity = severity
            };

            LogWriterInstance.Write(logEntry);

            Console.WriteLine("[{0:G} {1}] {2}", DateTime.UtcNow, severity, message);
        }

        public void Log(string message)
        {
            WriteLogInternal(message, TraceEventType.Information);
        }

        public void LogWarning(string message)
        {
            WriteLogInternal(message, TraceEventType.Warning);
        }

        public void LogError(string message)
        {
            WriteLogInternal(message, TraceEventType.Error);
        }

        public void LogError(Exception exception)
        {
            LogError(null, exception);
        }

        public void LogError(string message, Exception exception)
        {
            message = $"{message ?? "Exception"}. {exception.Message}\n{exception.StackTrace}";

            LogError(message);
        }

        public void Configure()
        {
            try
            {
                var configuration = string.IsNullOrWhiteSpace(this._configFilePath)
                    ? (IConfigurationSource)new FileConfigurationSource(this._configFilePath)
                    : (IConfigurationSource)new SystemConfigurationSource();

                var logWriterFactory = new LogWriterFactory(configuration);
                _logWriter = logWriterFactory.Create();
                IsConfigured = true;
            }
            catch (Exception e)
            {
                IsConfigured = false;
                Console.WriteLine("Logger configuring has been failed");
                Console.WriteLine(e);

                DefaultLogger.Write(new LogEntry
                {
                    Message = "Logger configuration has been failed",
                    Severity = TraceEventType.Error
                });
            }
        }

        public bool IsConfigured { get; private set; }

        private LogWriter CreateDefaultEventLogWriter()
        {
            var loggingConfigBuilder = new ConfigurationSourceBuilder();
            loggingConfigBuilder.ConfigureLogging()
                .WithOptions
                .DoNotRevertImpersonation()
                .LogToCategoryNamed("DefaultEventLog")
                .WithOptions.SetAsDefaultCategory()
                .SendTo.EventLog("Default EventLog Listner")
                .Filter(SourceLevels.Error)
                .FormatWith(new FormatterBuilder()
                    .TextFormatterNamed("Default Text Formatter")
                    .UsingTemplate("[{timestamp(local:g)} {severity}] {message}"))
                .ToLog("Application");

            return loggingConfigBuilder.Get<LoggingSettings>(LoggingSettings.SectionName).BuildLogWriter();
        }
    }
}