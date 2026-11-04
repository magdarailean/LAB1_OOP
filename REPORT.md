# Laboratory Report - LAB1_OOP

**Student:** Railean Magdalena

**Language and runtime:** C#, .NET 8

## Logger

A class is a description of objects with the same data and behavior. `new Logger("First")` constructs one Logger object; `new Logger("Second")` constructs another.

Fields (attributes):

- `private readonly string name`: the individual object's name. Private prevents outside code from accessing it directly. Readonly means it can be assigned at declaration or in the constructor, but cannot be reassigned afterward.
- `public static LogLevel Level`: the minimum level to print, initially INFO.
- `public static string Format`: the output pattern, initially `[%t] [%l] %n: %m`.

The constructor `Logger(string name)` stores the supplied name. `this.name = name` means assign the parameter on the right to this object's field on the left. A constructor has the class's name and no return type.

Methods:

- `SetLevel(LogLevel newLevel)`: static, changes the shared threshold.
- `SetFormat(string newFormat)`: static, changes the shared pattern.
- `Log(LogLevel msgLevel, string message)`: instance method, filters the message, scans the format with a loop, replaces recognized tokens, and prints the result. It needs the object's name.
- `GetRandomMessage(LogLevel level)`: private static helper for the optional %r token. If statements select an array of phrases, then Random.Shared.Next chooses an index. It uses the message level, not the threshold.

The tokens are %t (time), %m (message), %n (name), %l (level), and bonus %r (random phrase). The loop only reads the original format, so tokens inside an inserted name or message are not replaced again. Unknown tokens stay unchanged. Each entry chooses at most one random phrase.

## ParenthesesValidator

Its field `private readonly Logger logger = new Logger("Parentheses")` gives each validator its own logger. The field is private because other classes do not need direct access to it. There is no explicitly written constructor: C# supplies a parameterless constructor, used by `new ParenthesesValidator()`, and the field initializer creates its logger.

Its public instance method `bool IsValid(string input)` returns true or false. `balance` and `valid` are local variables, not object attributes; they start fresh for each call.

An opening parenthesis increases balance and logs TRACE. A closing parenthesis decreases balance and logs an INFO matching pair, unless balance is already zero: then it logs WARN, marks the input invalid, and stops. A nonzero balance at the end also means invalid. Invalid input produces ERROR with the original input. Every call logs the final Boolean result at INFO. Level filtering determines which events are visible.

For `(())`, balance is 0, 1, 2, 1, 0, so it is valid. For `(()`, balance ends at 1, so it is invalid. For `)(`, the first closing parenthesis has no match, so equal total counts alone are insufficient. Empty input is valid. Other characters are ignored; null input is unsupported.

## Program and LogLevel

Program has no fields or explicit constructor. Its static Main method is the application's entry point; the runtime calls it without creating a Program object. Main creates two loggers and a validator, shows logging at each threshold and with different formats, then compares nine actual validation results to an array of expected results. It uses loops and if statements. A failure increments a counter and sets the process exit code to 1.

LogLevel is an enum, not a class. It names four values: TRACE = 0, INFO = 1, WARN = 2, ERROR = 3. Their order makes the threshold comparison possible: if the message level is lower than Level, Log returns without printing.

## Static, instance, and encapsulation

Static members belong to the class and are shared. Call `Logger.SetLevel(LogLevel.ERROR)` or read `Logger.Level`. Changing Level or Format affects both existing and future logger objects.

Instance members belong to an individual object. `first.Log(...)` uses first's name; `second.Log(...)` uses second's name. Each validator also owns a logger object. This is composition: one object contains and uses another.

Encapsulation groups data and methods in a class and limits direct access. Logger hides name and GetRandomMessage; ParenthesesValidator hides logger and exposes IsValid. The required public static configuration fields remain directly accessible; the setter methods offer a clear way to change them, but do not enforce access restrictions.

## Verification

On 4 November 2026, using SDK 8.0.424:

- dotnet build succeeded with zero warnings and zero errors.
- dotnet run exited with code 0.
- All nine expected results matched: () true; )((())) false; ( false; (())((()())()) true; empty true; (() false; ()) false; )( false; ((())) true.
- Console examples showed all four thresholds, distinct logger names, shared formatting changes, literal message tokens, and level-specific bonus phrases.

Only parentheses results are checked automatically using simple comparisons. Logger examples are visible console demonstrations, without delegates, console-output capture, or a testing framework.
