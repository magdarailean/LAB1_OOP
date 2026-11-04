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

        string output = "";
        string time = DateTime.Now.ToString("HH:mm:ss");
        string randomMessage = "";

        // Read only the format: inserted names and messages stay literal.
        for (int i = 0; i < Format.Length; i++)
        {
            if (Format[i] == '%' && i + 1 < Format.Length)
            {
                string replacement = "";
                char token = Format[i + 1];
                if (token == 't')
                {
                    replacement = time;
                }
                else if (token == 'm')
                {
                    replacement = message;
                }
                else if (token == 'n')
                {
                    replacement = name;
                }
                else if (token == 'l')
                {
                    replacement = msgLevel.ToString();
                }
                else if (token == 'r')
                {
                    // Choose once so repeated %r tokens use the same phrase.
                    if (randomMessage == "")
                    {
                        randomMessage = GetRandomMessage(msgLevel);
                    }
                    replacement = randomMessage;
                }
                else
                {
                    output += Format[i];
                    continue;
                }

                output += replacement;
                i++;
                continue;
            }

            output += Format[i];
        }

        Console.WriteLine(output);
    }

    // random message for each log level.
    private static string GetRandomMessage(LogLevel level)
    {
        string[] messages;
        if (level == LogLevel.TRACE)
        {
            messages = new string[] { "Checking input", "Processing character" };
        }
        else if (level == LogLevel.INFO)
        {
            messages = new string[] { "Operation completed", "Everything looks good", "Here is your result" };
        }
        else if (level == LogLevel.WARN)
        {
            messages = new string[] { "Check this situation", "Something unexpected happened" };
        }
        else if (level == LogLevel.ERROR)
        {
            messages = new string[] { "Operation failed", "Invalid input detected" };
        }
        else
        {
            messages = new string[] { "Unknown level" };
        }

        return messages[Random.Shared.Next(messages.Length)];
    }
}
