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

        CommandReader reader = new CommandReader(shopLogger);
        string[] files = { "shop-monday.txt", "shop-tuesday.txt", "shop-extra.txt" };

        foreach (string file in files)
        {
            Console.WriteLine($"\n{file}");

            string path = Path.Combine(AppContext.BaseDirectory, file);
            string[] lines = reader.Read(path);

            List<DisplayManager> managers = new List<DisplayManager>();
            List<Display> displays = new List<Display>();

            for (int i = 0; i < lines.Length; i++)
            {
                CommandParser.ParseCommand(lines[i], managers, displays,
                    shopLogger, managerLogger, i + 1);
            }
        }
    }
}