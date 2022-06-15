using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace AIStudio.WpfApi
{
    public class ColorConsoleLoggerConfiguration
    {
        public int EventId { get; set; }

        public Dictionary<LogLevel, ConsoleColor> LogLevels { get; set; } = new()
        {
            [LogLevel.Error] = ConsoleColor.Red,
            [LogLevel.Warning] = ConsoleColor.Yellow,           
            [LogLevel.Information] = ConsoleColor.Green,
            [LogLevel.Debug] = ConsoleColor.Gray,
        };
    }
}