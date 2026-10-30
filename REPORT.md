# Laboratory Report — LAB1_OOP

**Student:** Railean Magdalena

**Programming language:** C#

## 1. Program Structure

This C# console application targets .NET 10 and demonstrates object-oriented programming through a configurable logger and a parentheses validator. It uses standard .NET libraries without external packages.

The source is divided into four files: `LogLevel.cs` defines severity values, `Logger.cs` handles logging, `ParenthesesValidator.cs` checks input, and `Program.cs` contains the entry point, tests, and demonstrations. This separation keeps console presentation and testing apart from the validation algorithm.

## 2. Classes and Objects

`Logger` defines the state and behavior of a named logging source. Its constructor assigns a private readonly `name`, so each object retains its own identity:

```csharp
Logger first = new Logger("First");
Logger second = new Logger("Second");
```

Both objects use the same class, but `%n` prints a different name for each. `ParenthesesValidator` contains its own logger:

```csharp
private readonly Logger logger = new Logger("Parentheses");
```

This is composition: the validator uses a Logger object to report its work. `Program` creates a validator and calls `IsValid`. The validator stores a reference to its logger; balance and validity are local variables created for each call. No custom inheritance hierarchy is required.

## 3. Static and Instance Members

The logger name belongs to each instance, while configuration belongs to the class:

```csharp
private readonly string name;
public static LogLevel Level = LogLevel.INFO;
public static string Format = "[%t] [%l] %n: %m";
```

`SetLevel` and `SetFormat` are static methods that assign these shared fields. Changes affect both existing and future Logger objects. `Log` is an instance method because output includes the individual object's name. The private static `GetRandomMessage` helper selects phrases without needing an object name. The validator's logger is an instance member, so each validator owns a logger object.

## 4. Methods and Responsibilities

`ParenthesesValidator` delegates logging to `Logger` so that validation remains responsible for checking parentheses, while Logger handles filtering, formatting, and console output. Changing the output format therefore does not require changing the algorithm. For example, `IsValid` calls its logger object after finding an opening parenthesis:

```csharp
logger.Log(LogLevel.TRACE,
    "Opening parenthesis found");
```

`Logger.Log` first applies severity filtering:

```csharp
if (msgLevel < Level)
{
    return;
}
```

The enum values are assigned in declaration order: TRACE = 0, INFO = 1, WARN = 2, and ERROR = 3. Therefore, the comparison suppresses less severe messages. At INFO, INFO, WARN, and ERROR print; at ERROR, only ERROR prints.

For accepted messages, `Log` replaces `%t` with local time in `HH:mm:ss` format, `%m` with the message, `%n` with the logger name, and `%l` with the message level. The format is scanned once using `StringBuilder`; inserted names and messages are not scanned again. Unknown tokens and other format text remain unchanged. The extra credit `%r` calls `GetRandomMessage`, which selects a phrase array for the message level and chooses an index with `Random.Shared.Next`. The threshold does not determine the phrase category, and repeated phrases are possible.

`ParenthesesValidator.IsValid` scans left to right with a balance counter. An opening parenthesis increases balance and logs TRACE. A closing parenthesis requires positive balance; it decreases balance and logs INFO for a matching pair. If balance is zero, the closing parenthesis has no match: the method logs WARN and stops scanning. Remaining openings also make the input invalid. Invalid input is included in an ERROR message, and the final Boolean result is logged at INFO. All logging remains subject to the configured threshold.

For `(())`, balance follows `0 → 1 → 2 → 1 → 0`, so the result is true. For `(()`, the final balance is 1, so the result is false. For `)(`, the first character is rejected immediately, even though the total numbers of openings and closings are equal. Empty input is valid because no unmatched parentheses exist. Other characters are ignored; null input is unsupported. The balance algorithm takes O(n) time and O(1) additional space.

## 5. Testing and Correctness

Verification with .NET SDK 10.0.400 on 30 October 2026 produced a successful `dotnet build` with zero warnings and zero errors. `dotnet run` passed every automated check and returned exit code 0.

| Input | Expected and actual result |
| --- | --- |
| `()` | true |
| `)((()))` | false |
| `(` | false |
| `(())((()())())` | true |
| empty string | true |
| `(()` | false |
| `())` | false |
| `)(` | false |
| `((()))` | true |

The `)(` test prevents an incorrect implementation that only compares total counts: one opening and one closing parenthesis are present, but their order is invalid. The nine validation checks printed PASS. `TestLogger` also passed four checks, one per threshold, by capturing console output from two named loggers and comparing it with expected messages. These checks cover filtering, shared settings, and distinct instance names. Six further formatting checks passed: literal payloads and repeated tokens, a time-only format, and `%r` phrase categories at all four message levels. Repeated `%r` tokens use the same phrase within one entry. All 19 checks passed. `Main` reports failures through a nonzero exit code.

`DemonstrateLogger` also displayed all four levels, all five format tokens, literal text, and a second shared format at the ERROR threshold. Automated bonus checks accept any phrase from the correct level's list rather than requiring a particular random choice. Execution ended with `ALL TESTS PASSED`.

## 6. Reflection

Composition and separation of responsibilities are the central OOP concepts in this design. The validator contains a Logger object and delegates output to it, so the two classes collaborate without inheritance. Each class has a clear purpose: validation determines correctness, while logging controls how events are displayed. Private object state and helper methods also demonstrate encapsulation, and shared settings distinguish static configuration from instance identity.

Shared configuration makes the small application easy to configure, but prevents different loggers from using independent thresholds or formats. Public configuration fields are simple, although they allow callers to bypass the setter methods. Direct console output and global settings also require tests to capture output and manage shared state.

Possible improvements include validated configuration properties, explicit null handling, an injectable output destination, and tests for the validator's exact log-event sequence. Supporting several bracket types would require tracking opening types with a stack rather than only a counter. These changes could extend the design, but the current implementation remains suitable for demonstrating the laboratory's core OOP concepts.

