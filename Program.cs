using System;
using System.IO;

class Program
{
    static void Main()
    {
        Logger.SetLevel(LogLevel.INFO);
        Logger.SetFormat("[%t] [%l] %n: %m");
        ParenthesesValidator validator = new ParenthesesValidator();
        (string Input, bool Expected)[] tests =
        {
            ("()", true), (")((()))", false), ("(", false),
            ("(())((()())())", true), ("", true), ("(()", false),
            ("())", false), (")(", false), ("((()))", true)
        };

        int failures = 0;
        Console.WriteLine("Parentheses validation tests:");
        foreach (var test in tests)
        {
            bool result = validator.IsValid(test.Input);
            bool passed = result == test.Expected;
            if (!passed)
            {
                failures++;
            }
            Console.WriteLine($"{(passed ? "PASS" : "FAIL")}: \"{test.Input}\" | Expected: {test.Expected} | Actual: {result}\n");
        }

        failures += TestLogger();
        failures += TestFormatting();
        DemonstrateLogger(validator);
        Console.WriteLine(failures == 0 ? "ALL TESTS PASSED" : $"FAILED: {failures} test(s)");
        Environment.ExitCode = failures == 0 ? 0 : 1;
    }

    private static int TestLogger()
    {
        int failures = 0;
        Logger first = new Logger("First");
        Logger second = new Logger("Second");
        TextWriter originalOutput = Console.Out;

        // Capture output to check both visible and filtered messages.
        foreach (LogLevel threshold in Enum.GetValues<LogLevel>())
        {
            using StringWriter captured = new StringWriter();
            Logger.SetLevel(threshold);
            Logger.SetFormat("%n|%l|%m");
            string expected = "";
            try
            {
                Console.SetOut(captured);
                foreach (LogLevel level in Enum.GetValues<LogLevel>())
                {
                    first.Log(level, "hello");
                    second.Log(level, "hello");
                    if (level >= threshold)
                    {
                        expected += $"First|{level}|hello{Environment.NewLine}";
                        expected += $"Second|{level}|hello{Environment.NewLine}";
                    }
                }
            }
            finally
            {
                Console.SetOut(originalOutput);
            }

            bool passed = captured.ToString() == expected
                && Logger.Level == threshold && Logger.Format == "%n|%l|%m";
            Console.WriteLine($"{(passed ? "PASS" : "FAIL")}: Logger at {threshold}: filtering, shared settings, instance names");
            if (!passed)
            {
                failures++;
            }
        }
        return failures;
    }

    private static int TestFormatting()
    {
        int failures = 0;
        Logger.SetLevel(LogLevel.TRACE);
        Logger logger = new Logger("Service%m%r");
        Logger.SetFormat("[%l] %n: %m | %n | %x | 100%!");
        failures += CheckLog("Literal names/messages, repeated tokens, and unchanged text",
            () => logger.Log(LogLevel.INFO, "Keep %t %m %n %l %r"),
            output => output == "[INFO] Service%m%r: Keep %t %m %n %l %r | Service%m%r | %x | 100%!");

        Logger.SetFormat("%t");
        failures += CheckLog("Time-only format",
            () => logger.Log(LogLevel.INFO, "unused"),
            output => DateTime.TryParseExact(output, "HH:mm:ss",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out _));

        (LogLevel Level, string[] Phrases)[] randomCases =
        {
            (LogLevel.TRACE, new[] { "Checking input", "Processing character" }),
            (LogLevel.INFO, new[] { "Operation completed", "Everything looks good", "Here is your result" }),
            (LogLevel.WARN, new[] { "Check this situation", "Something unexpected happened" }),
            (LogLevel.ERROR, new[] { "Operation failed", "Invalid input detected" })
        };
        Logger.SetFormat("%r|%r");
        foreach (var test in randomCases)
        {
            failures += CheckLog($"%r uses the {test.Level} phrase category",
                () => logger.Log(test.Level, "unused"),
                output => Array.Exists(test.Phrases, phrase => output == $"{phrase}|{phrase}"));
        }
        return failures;
    }

    private static int CheckLog(string description, Action logAction, Func<string, bool> check)
    {
        TextWriter originalOutput = Console.Out;
        using StringWriter captured = new StringWriter();
        try
        {
            Console.SetOut(captured);
            logAction();
        }
        finally
        {
            Console.SetOut(originalOutput);
        }

        bool passed = check(captured.ToString().TrimEnd('\r', '\n'));
        Console.WriteLine($"{(passed ? "PASS" : "FAIL")}: {description}");
        return passed ? 0 : 1;
    }

    private static void DemonstrateLogger(ParenthesesValidator validator)
    {
        Logger first = new Logger("First");
        Logger second = new Logger("Second");
        Logger.SetLevel(LogLevel.TRACE);
        Logger.SetFormat("[%t] [%l] %n: %m | Random: %r | literal text!");
        Console.WriteLine("\nAll levels and placeholders (including extra credit %r):");
        foreach (LogLevel level in Enum.GetValues<LogLevel>())
        {
            first.Log(level, "Demonstration message");
        }
        second.Log(LogLevel.INFO, "Same format, different logger name");
        validator.IsValid("()");

        Logger.SetLevel(LogLevel.ERROR);
        Logger.SetFormat("%n -> %l: %m");
        Console.WriteLine("\nERROR threshold and a different shared format:");
        first.Log(LogLevel.INFO, "This message is filtered out");
        first.Log(LogLevel.ERROR, "This error is visible");
        second.Log(LogLevel.ERROR, "The new settings apply to both objects");

        Logger.SetLevel(LogLevel.INFO);
        Logger.SetFormat("[%t] [%l] %n: %m");
    }
}
