using Serilog;
using Serilog.Events;

namespace Bank.Logging
{
    public sealed class Logger : Application.Interfaces.ILogger
    {
        private static readonly ILogger logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        private static readonly Lazy<Logger> loggerInstance = new Lazy<Logger>(() => new Logger());
        private const string ExceptionName = "Exception";
        private const string InnerExceptionName = "Inner Exception";
        private const string ExceptionMessageWithoutInnerException = "{0}{1}: {2}Message: {3}{4}StackTrace: {5}.";
        private const string ExceptionMessageWithInnerException = "{0}{1}{2}";

        public static Logger Instance
        {
            get { return loggerInstance.Value; }
        }

        public void Verbose(string message)
        {
            if (logger.IsEnabled(LogEventLevel.Verbose))
            {
                logger.Verbose(message);
            }
        }

        public void Verbose(string message, Exception exception)
        {
            if (logger.IsEnabled(LogEventLevel.Verbose))
            {
                logger.Verbose(exception, message);
            }
        }

        public void Debug(string message)
        {
            if (logger.IsEnabled(LogEventLevel.Debug))
            {
                logger.Debug(message);
            }
        }

        public void Debug(string message, Exception exception)
        {
            if (logger.IsEnabled(LogEventLevel.Debug))
            {
                logger.Debug(exception, message);
            }
        }

        public void Information(string message)
        {
            if (logger.IsEnabled(LogEventLevel.Information))
            {
                logger.Information(message);
            }
        }

        public void Information(string message, Exception exception)
        {
            if (logger.IsEnabled(LogEventLevel.Information))
            {
                logger.Information(exception, message);
            }
        }

        public void Warning(string message)
        {
            if (logger.IsEnabled(LogEventLevel.Warning))
            {
                logger.Warning(message);
            }
        }

        public void Warning(string message, Exception exception)
        {
            if (logger.IsEnabled(LogEventLevel.Warning))
            {
                logger.Warning(exception, message);
            }
        }

        public void Error(string message)
        {
            if (logger.IsEnabled(LogEventLevel.Error))
            {
                logger.Error(message);
            }
        }

        public void Error(string message, Exception exception)
        {
            if (logger.IsEnabled(LogEventLevel.Error))
            {
                logger.Error(exception, message);
            }
        }

        public void Error(Exception exception)
        {
            if (logger.IsEnabled(LogEventLevel.Error))
            {
                logger.Error(SerializeException(exception, ExceptionName));
            }
        }

        public void Fatal(string message)
        {
            if (logger.IsEnabled(LogEventLevel.Fatal))
            {
                logger.Fatal(message);
            }
        }

        public void Fatal(string message, Exception exception)
        {
            if (logger.IsEnabled(LogEventLevel.Fatal))
            {
                logger.Fatal(exception, message);
            }
        }

        public void Fatal(Exception exception)
        {
            if (logger.IsEnabled(LogEventLevel.Fatal))
            {
                logger.Fatal(SerializeException(exception, ExceptionName));
            }
        }

        public static string SerializeException(Exception exception)
        {
            return SerializeException(exception, string.Empty);
        }

        private static string SerializeException(Exception ex, string exceptionMessage)
        {
            var mesgAndStackTrace = string.Format(ExceptionMessageWithoutInnerException, Environment.NewLine,
                exceptionMessage, Environment.NewLine, ex.Message, Environment.NewLine, ex.StackTrace);

            if (ex.InnerException != null)
            {
                mesgAndStackTrace = string.Format(ExceptionMessageWithInnerException, mesgAndStackTrace,
                    Environment.NewLine,
                    SerializeException(ex.InnerException, InnerExceptionName));
            }

            return mesgAndStackTrace + Environment.NewLine;
        }
    }
}
