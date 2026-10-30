# LAB1_OOP — Logger and Parentheses Validator

A first university OOP laboratory in C#. The console program implements a configurable logger and uses it to explain the steps of parentheses validation.

## Technologies

- C# and .NET 10 SDK
- JetBrains Rider (or any editor and a terminal)
- Standard .NET libraries only; no external packages or testing frameworks

## Program structure

- `LogLevel.cs`: enum containing TRACE, INFO, WARN, ERROR in increasing severity.
- `Logger.cs`: each object stores its own readonly name. Public static `Level` and `Format` are shared by every logger. Static `SetLevel` and `SetFormat` update those settings. `Log` filters and prints messages. Private `GetRandomMessage` implements the bonus.
- `ParenthesesValidator.cs`: owns a logger named Parentheses. `IsValid` returns a Boolean and logs its progress.
- `Program.cs`: Main runs nine validation cases, four logger threshold checks, six formatting/bonus checks, and configuration demonstrations. Failures produce a nonzero process exit code.

## Logging

The levels are TRACE (detailed steps), INFO (normal progress/results), WARN (unexpected situations), and ERROR (failures). A message prints when its level is at least the configured threshold. The default threshold is INFO.

```csharp
Logger logger = new Logger("Example");
Logger.SetLevel(LogLevel.TRACE);
Logger.SetFormat("[%t] [%l] %n: %m | %r");
logger.Log(LogLevel.INFO, "Ready");
```

| Placeholder | Meaning |
| --- | --- |
| `%t` | Local time in HH:mm:ss format |
| `%m` | Supplied message |
| `%n` | Name of the logger object |
| `%l` | Level of this message |
| `%r` | Random phrase selected for this message's level |

Only placeholders in the format itself are interpreted. Names and messages containing text such as `%m` or `%r` remain literal. Unknown placeholders and other format characters remain unchanged. The default format is `[%t] [%l] %n: %m`.

The extra credit `%r` chooses a phrase using `Random.Shared.Next`. TRACE phrases describe processing, INFO phrases describe normal progress, WARN phrases flag unexpected situations, and ERROR phrases describe failures. Selection uses the message level, not the configured threshold. Repeated phrases are possible. Multiple `%r` tokens in one log entry share the same selected phrase.

## Validation algorithm

Start with balance zero and scan left to right. An opening parenthesis increments balance and produces TRACE. A closing parenthesis requires positive balance; decrement it and log INFO for a matching pair. If balance is already zero, log WARN and stop with an invalid result. Remaining opening parentheses also make the result invalid. Invalid inputs produce ERROR containing the input; every call attempts an INFO final-result message. The logger threshold controls which messages appear.

Empty input is valid. Other characters are ignored; the validator checks parentheses balance rather than a full expression grammar. The input must be a non-null string. Runtime is O(n) with O(1) extra space.

## Run

Install/select the .NET 10 SDK. Open a terminal in LAB1_OOP:

```sh
dotnet build
dotnet run
```

In Rider, select the .NET 10 SDK/toolset and run the project. If `dotnet --version` reports 8, select your .NET 10 installation or put its directory first in the terminal PATH. No project target change is needed.

## Tests and demonstrations

| Input | Expected |
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

Each case displays PASS or FAIL, expected, and actual results. Four additional checks capture console output at each threshold and compare messages from two logger objects, verifying filtering, instance names, and shared settings. Six further automated checks cover literal payloads, repeated tokens, unchanged text, a time-only format, and the four `%r` phrase categories. Demonstrations show all placeholders, all four message levels, random phrases, literal text, and changing the shared format and threshold. Successful execution ends with ALL TESTS PASSED. Timestamp and random phrase values vary between runs.

## Verification and documentation

Verified with .NET SDK 10.0.400 on 30 October 2026: the build completed with zero warnings and zero errors, all 19 checks passed (nine validation, four filtering/shared-setting, and six formatting/bonus checks), and the program exited with code 0. The console demonstration also confirmed time formatting, literal text, and level-specific `%r` phrases. Automated checks validate the timestamp format and the random phrase category; exact timestamps and random choices vary between runs.

- [Laboratory report](REPORT.md): program design, algorithm, test results, and reflection.
- [Presentation notes](PRESENTATION_NOTES.md): a short speaking outline and possible questions for individual preparation.

