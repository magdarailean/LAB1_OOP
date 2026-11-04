# LAB1_OOP - Logger and Parentheses Validator

A beginner C# OOP laboratory targeting .NET 8, using standard libraries only.

Run from the project directory:

```sh
dotnet build
dotnet run
```

## Files and tasks

- `LogLevel.cs`: TRACE, INFO, WARN, ERROR, in increasing severity.
- `Logger.cs`: Task 1, filtering and configurable console logging.
- `ParenthesesValidator.cs`: Task 2, parentheses validation; Task 3, integration with Logger.
- `Program.cs`: straightforward logger examples and nine validation checks using arrays, loops, if statements, and Console.WriteLine.

Each Logger has its own private readonly name. Public static Level and Format are shared by all loggers. SetLevel and SetFormat change these settings. Log prints messages at or above the threshold; the default is INFO.

| Token | Replacement |
| --- | --- |
| %t | Local time, HH:mm:ss |
| %m | Message |
| %n | Logger name |
| %l | Message level |
| %r | Optional random phrase for the message level |

The default format is `[%t] [%l] %n: %m`. The format is scanned with a loop and if statements. Inserted names and messages stay literal; unknown tokens and ordinary text stay unchanged. Multiple %r tokens in one entry use the same phrase. The bonus uses a phrase array and Random.Shared.Next.

The validator counts unmatched opening parentheses. It logs TRACE for an opening, INFO for a matching pair, WARN for a closing without a matching opening, ERROR with the input for an invalid string, and INFO for the final result. All events are subject to the logger threshold.

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

Other characters are ignored. Input must be a non-null string. Program displays expected and actual results and PASS or FAIL; a failed validation check sets exit code 1. Logger examples are inspected directly in the console rather than through captured-output tests.

Verified on 4 November 2026 with SDK 8.0.424: dotnet build succeeded with zero warnings and errors; dotnet run exited with code 0 and all nine parentheses checks passed. Logger output showed correct threshold filtering, shared settings, distinct names, formatting, and bonus phrases.

See [REPORT.md](REPORT.md) for presentation explanations.
