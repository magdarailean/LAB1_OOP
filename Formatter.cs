using System;

class Formatter
{
    public static string Format = "%n: %m";

    public string FormatLine(string name, LogLevel msgLevel, string message)
    {
        string output = "";
        string time = DateTime.Now.ToString("HH:mm:ss");
        string randomMessage = "";

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
        return output;
    }
    
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