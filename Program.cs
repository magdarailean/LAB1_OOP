using System;

class Program
{
    static void Main()
    {
        // A1
        Formatter formatatorMesaje = new Formatter();

        Formatter.Format = "%n: %m";
        Console.WriteLine(formatatorMesaje.FormatLine("Parentheses", LogLevel.INFO, "Whatever %n"));

        Formatter.Format = "[%l] %m";
        Console.WriteLine(formatatorMesaje.FormatLine("Parentheses", LogLevel.INFO, "Why %n"));

        // A2
        Logger.SetLevel(LogLevel.TRACE);
        Formatter.Format = "[%t] [%l] %n: %m";

        ParenthesesValidator validator = new ParenthesesValidator();
        Console.Write("The Parentheses: ");
        var input = Console.ReadLine();

        if (input != null)
            Console.WriteLine(validator.IsValid(input));

        // T1.3
        Formatter.Format = "[%l] %n: %m";

        Formatter shopFormatter = new Formatter();
        ConsoleSink shopSink = new ConsoleSink();

        Logger shopLogger = new Logger("Shop", shopFormatter, shopSink);
        Logger managerLogger = new Logger("Manager", shopFormatter, shopSink);

        // T1.1
        Display dell = new Display("Dell", 3840, 2160, 163f);
        Display lg = new Display("LG", 2560, 1440, 108.8f);
        Display samsung = new Display("Samsung", 2560, 1440, 93f);

        shopLogger.Log(LogLevel.TRACE, $"Display {dell.Model} created");
        shopLogger.Log(LogLevel.TRACE, $"Display {lg.Model} created");
        shopLogger.Log(LogLevel.TRACE, $"Display {samsung.Model} created");

        // T1.2
        DisplayManager bob = new DisplayManager("Bob", managerLogger);
        DisplayManager alice = new DisplayManager("Alice", managerLogger);

        bob.AssignDisplay(dell);
        bob.AssignDisplay(samsung);

        alice.AssignDisplay(dell);
        alice.AssignDisplay(lg);

        bob.BuyDisplay(dell);
        alice.Assist();
    }
}