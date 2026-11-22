class ParenthesesValidator
{
    private readonly Logger logger = new Logger("Parentheses");
    
    public bool IsValid(string input)
    {
        int balance = 0;
        bool valid = true;

        for (int i = 0; i < input.Length; i++)
        {
            char character = input[i];
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
                    // int remainder = input.Length - i;
                    // Console.WriteLine($"Remainder:{remainder}");
                        
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
