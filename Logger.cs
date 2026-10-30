using System;
using System.Text;

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

        StringBuilder output = new StringBuilder();
        string time = DateTime.Now.ToString("HH:mm:ss");
        string? randomMessage = null;

        // Read only the format: inserted names and messages stay literal.
        for (int i = 0; i < Format.Length; i++)
        {
            if (Format[i] == '%' && i + 1 < Format.Length)
            {
                string? replacement = Format[i + 1] switch
                {
                    't' => time,
                    'm' => message,
                    'n' => name,
                    'l' => msgLevel.ToString(),
                    'r' => randomMessage ??= GetRandomMessage(msgLevel),
                    _ => null
                };

                if (replacement != null)
                {
                    output.Append(replacement);
                    i++;
                    continue;
                }
            }

            output.Append(Format[i]);
        }

        Console.WriteLine(output.ToString());
    }

    // Extra credit: random message for each log level.
    private static string GetRandomMessage(LogLevel level)
    {
        string[] messages = level switch
        {
            LogLevel.TRACE => new[]
            {
                "Checking input",
                "Processing character"
            },
            LogLevel.INFO => new[]
            {
                "Operation completed",
                "Everything looks good",
                "Here is your result"
            },
            LogLevel.WARN => new[]
            {
                "Check this situation",
                "Something unexpected happened"
            },
            LogLevel.ERROR => new[]
            {
                "Operation failed",
                "Invalid input detected"
            },
            _ => new[] { "Unknown level" }
        };

        return messages[Random.Shared.Next(messages.Length)];
    }
}
