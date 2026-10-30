using System;

enum LogLevel
{
    TRACE,
    INFO,
    WARN,
    ERROR
}

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

        string output = Format
            .Replace("%t", DateTime.Now.ToString("HH:mm:ss"))
            .Replace("%l", msgLevel.ToString())
            .Replace("%n", name)
            .Replace("%r", GetRandomMessage(msgLevel))
            .Replace("%m", message);

        Console.WriteLine(output);
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

class ParenthesesValidator
{
    private readonly Logger logger = new Logger("Parentheses");

    public bool IsValid(string input)
    {
        int balance = 0;
        bool valid = true;

        foreach (char character in input)
        {
            if (character == '(')
            {
                balance++;
                logger.Log(LogLevel.TRACE,
                    "Opening parenthesis found");
            }
            else if (character == ')')
            {
                if (balance == 0)
                {
                    logger.Log(LogLevel.WARN,
                        "Closing parenthesis has no match");

                    valid = false;
                    break;
                }

                balance--;
                logger.Log(LogLevel.INFO,
                    "Matching pair found");
            }
        }

        if (balance != 0)
        {
            valid = false;
        }

        if (!valid)
        {
            logger.Log(LogLevel.ERROR,
                $"Invalid string: {input}. Parentheses are not balanced.");
        }

        logger.Log(LogLevel.INFO,
            $"Validation result for {input}: {valid}");

        return valid;
    }
}

class Program
{
    static void Main()
    {
        Logger.SetLevel(LogLevel.INFO);
        Logger.SetFormat("[%t] [%l] %n: %m");

        ParenthesesValidator validator =
            new ParenthesesValidator();

        // Required test cases from the laboratory.
        string[] inputs =
        {
            "()",
            ")((( )))".Replace(" ", ""),
            "(",
            "(())((()())())"
        };

        bool[] expected =
        {
            true,
            false,
            false,
            true
        };

        for (int i = 0; i < inputs.Length; i++)
        {
            bool result = validator.IsValid(inputs[i]);

            Console.WriteLine(
                $"Test: {inputs[i]} | Expected: {expected[i]} | Actual: {result}");

            Console.WriteLine(result == expected[i]
                ? "PASS"
                : "FAIL");

            Console.WriteLine();
        }

        // Demonstrate TRACE and the extra %r placeholder.
        Logger.SetLevel(LogLevel.TRACE);
        Logger.SetFormat("[%l] %n: %m | %r");

        Console.WriteLine("Extra credit demonstration:");
        validator.IsValid("()");
    }
}
