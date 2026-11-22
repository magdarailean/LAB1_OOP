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
    }
}
