using Microsoft.Extensions.Logging.Console;

namespace projectServer
{
    public class CustomConsoleFormatterOptions : ConsoleFormatterOptions
    {
        public string TimestampFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";
        public Dictionary<LogLevel, ConsoleColor> LogLevelColors { get; set; } = new Dictionary<LogLevel, ConsoleColor>
        {
            [LogLevel.Trace] = ConsoleColor.Gray,
            [LogLevel.Debug] = ConsoleColor.Green,
            [LogLevel.Information] = ConsoleColor.White,
            [LogLevel.Warning] = ConsoleColor.Yellow,
            [LogLevel.Error] = ConsoleColor.Red,
            [LogLevel.Critical] = ConsoleColor.Magenta,
            [LogLevel.None] = ConsoleColor.White
        };
        public string CustomPrefix { get; set; } = "Log: ";
    }
}
