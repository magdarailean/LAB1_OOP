using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Logger.SetLevel(LogLevel.TRACE);

        /* A2
        Formatter.Format = "[%t] [%l] %n: %m";
        ParenthesesValidator validator = new ParenthesesValidator();
        Console.Write("The Parentheses: ");
        var input = Console.ReadLine();

        if (input != null)
            Console.WriteLine(validator.IsValid(input));
        */

        // T1.3
        Formatter.Format = "[%l] %n: %m";

        Formatter shopFormatter = new Formatter();
        ConsoleSink shopSink = new ConsoleSink();

        Logger shopLogger = new Logger("Shop", shopFormatter, shopSink);
        Logger managerLogger = new Logger("Manager", shopFormatter, shopSink);

        // T1.4
        CommandReader reader = new CommandReader(shopLogger);
        string path = Path.Combine(AppContext.BaseDirectory, "mondayshop.txt");
        string[] lines = reader.Read(path);

        // T1.5
        List<DisplayManager> managers = new List<DisplayManager>();
        List<Display> displays = new List<Display>();

        foreach (string line in lines)
        {
            CommandParser.ParseCommand(line, managers, displays, shopLogger, managerLogger);
        }
    }
}