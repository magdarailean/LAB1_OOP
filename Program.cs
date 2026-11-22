using System;

class Program
{
    static void Main()
    {   
        Formatter formatatorMesaje = new Formatter();

        Formatter.Format = "%n: %m";
        Console.WriteLine(formatatorMesaje.FormatLine("Parentheses", LogLevel.INFO, "Whatever %n"));

        Formatter.Format = "[%l] %m";
        Console.WriteLine(formatatorMesaje.FormatLine("Parentheses", LogLevel.INFO, "Why %n"));
        
        
        Logger.SetLevel(LogLevel.TRACE);
        Formatter.Format = "[%t] [%l] %n: %m";
        ParenthesesValidator validator = new ParenthesesValidator();
        Console.Write("The Parentheses: ");
        var input = Console.ReadLine();
        if (input != null)
            Console.WriteLine(validator.IsValid(input));
        
        
        Display dell = new Display("Dell", 3840, 2160, 163f);
        Display lg = new Display("LG", 2560, 1440, 108.8f);
        Display samsung = new Display("Samsung", 2560, 1440, 93f);

        Console.WriteLine(samsung.CompareWithMonitor(lg));
        Console.WriteLine(dell.CompareWithMonitor(samsung));
        
    }
}
