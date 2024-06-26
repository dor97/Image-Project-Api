using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System.Text;

namespace projectServer
{
    public class CustomConsoleFormatter : ConsoleFormatter, IDisposable
    {
        private readonly IDisposable _optionsReloadToken;
        private CustomConsoleFormatterOptions _formatterOptions;

        public CustomConsoleFormatter(IOptionsMonitor<CustomConsoleFormatterOptions> options)
            : base("custom")
        {
            _optionsReloadToken = options.OnChange(ReloadLoggerOptions);
            _formatterOptions = options.CurrentValue;
        }

        private void ReloadLoggerOptions(CustomConsoleFormatterOptions options)
        {
            _formatterOptions = options;
        }

        public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, TextWriter textWriter)
        {
            var logLevel = logEntry.LogLevel;
            var category = logEntry.Category;
            var message = logEntry.Formatter(logEntry.State, logEntry.Exception);

            if (!string.IsNullOrEmpty(message))
            {
                var builder = new StringBuilder();

                if (!string.IsNullOrEmpty(_formatterOptions.TimestampFormat))
                {
                    builder.Append($"[{DateTime.Now.ToString(_formatterOptions.TimestampFormat)}] ");
                }

                builder.Append($"[{logLevel}] ");
                builder.Append($"[{category}] ");
                builder.Append(_formatterOptions.CustomPrefix);
                builder.Append(message);                

                if (logEntry.Exception != null)
                {
                    builder.AppendLine();
                    builder.Append(logEntry.Exception);
                }

                textWriter.WriteLine(builder.ToString());
            }
        }

        public void Dispose()
        {
            _optionsReloadToken?.Dispose();
        }
    }
}
