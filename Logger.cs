using System;

class Logger
{
    private readonly string name;

    public static LogLevel Level = LogLevel.INFO;
    public static string Format = "[%t] [%l] %n: %m";

    public Logger(string name)
    {
        this.name = name;
    }

    public static void SetLevel(LogLevel newLevel)
    {
        Level = newLevel;
    }

    public static void SetFormat(string newFormat)
    {
        Format = newFormat;
    }

    public void Log(LogLevel msgLevel, string message)
    {
        // Do not print messages below the configured level.
        if (msgLevel < Level)
        {
            return;
        }
    }
}
