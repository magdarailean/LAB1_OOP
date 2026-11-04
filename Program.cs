using System;

class Program
{
    static void Main()
    {
        Logger first = new Logger("First");
        Logger second = new Logger("Second");
        LogLevel[] levels = { LogLevel.TRACE, LogLevel.INFO, LogLevel.WARN, LogLevel.ERROR };

        Console.WriteLine("Task 1: Logger examples");
        Logger.SetFormat("[%t] [%l] %n: %m | Bonus: %r");
        foreach (LogLevel threshold in levels)
        {
            Logger.SetLevel(threshold);
            Console.WriteLine($"\nThreshold: {threshold}. Only this level and higher should appear:");
            foreach (LogLevel level in levels)
            {
                first.Log(level, "Example message");
            }
            second.Log(LogLevel.ERROR, "Same shared settings, different instance name");
        }

        Logger.SetLevel(LogLevel.TRACE);
        Logger.SetFormat("%n -> %l: %m");
        first.Log(LogLevel.INFO, "The shared format has changed");
        second.Log(LogLevel.INFO, "The change applies to both objects");

        // Only this in Format are replaced; message text stays literal.
        Logger.SetFormat("%n | %n | %m | %x | 100%!");
        first.Log(LogLevel.INFO, "Keep %t %m %n %l %r as text");

        Console.WriteLine("\nTasks 2 and 3: Parentheses validation with logging");
        Logger.SetFormat("[%t] [%l] %n: %m");
        ParenthesesValidator validator = new ParenthesesValidator();
        string[] inputs = { "()", ")((()))", "(", "(())((()())())", "", "(()", "())", ")(", "((()))" };
        bool[] expectedResults = { true, false, false, true, true, false, false, false, true };
        int failures = 0;

        for (int i = 0; i < inputs.Length; i++)
        {
            bool result = validator.IsValid(inputs[i]);
            Console.WriteLine($"Input: \"{inputs[i]}\" | Expected: {expectedResults[i]} | Actual: {result}");
            if (result == expectedResults[i])
            {
                Console.WriteLine("PASS\n");
            }
            else
            {
                Console.WriteLine("FAIL\n");
                failures++;
            }
        }

        if (failures == 0)
        {
            Console.WriteLine("ALL 9 PARENTHESES TESTS PASSED");
        }
        else
        {
            Console.WriteLine($"FAILED: {failures} parentheses test(s)");
            Environment.ExitCode = 1;
        }
    }
}
