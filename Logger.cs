using System;

class Logger
{
    private readonly string name;
    private readonly Formatter formatter;
    private readonly ConsoleSink sink;

    public static LogLevel Level = LogLevel.INFO;

    public Logger(string name)
    {
        this.name = name;
        formatter = new Formatter();
        sink = new ConsoleSink();
    }

    public Logger(string name, Formatter formatter, ConsoleSink sink)
    {
        this.name = name;
        this.formatter = formatter;
        this.sink = sink;
    }

    public static void SetLevel(LogLevel newLevel)
    {
        Level = newLevel;
    }

    public void Log(LogLevel msgLevel, string message)
    {
        if (msgLevel < Level)
            return;

        string line = formatter.FormatLine(name, msgLevel, message);
        sink.Write(line);
    }
}