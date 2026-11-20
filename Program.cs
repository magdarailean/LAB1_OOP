using System;

class Program
{
    static void Main()
    {   Logger.SetLevel(LogLevel.TRACE);
        Logger.SetFormat("[%t] [%l] %n: %m");
        ParenthesesValidator validator = new ParenthesesValidator();
        Console.Write("The Parentheses: ");
        var input = Console.ReadLine();
        if (input != null)
            Console.WriteLine(validator.IsValid(input));
    }
}
