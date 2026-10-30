# Presentation Notes — 3–5 minutes

## Introduction (about 30 seconds)

My project is a C# console application using .NET 10. It contains a configurable Logger and a ParenthesesValidator. The validator uses the logger to show what happens during validation. The project uses standard .NET libraries only.

## Structure and OOP (about 45 seconds)

I separated the program into four source files. LogLevel defines the four severity values. Logger handles output. ParenthesesValidator checks the input. Program contains Main, tests, and demonstrations.

A class describes an object. For example, I create two Logger objects with different names. Each object has its own private readonly name, assigned by the constructor. The validator contains its own Logger object. This is composition: one object contains and uses another object.

## Static settings and logging (about 60 seconds)

Level and Format are static, so all Logger objects share them. SetLevel and SetFormat change these settings. Log is an instance method because it uses the individual logger name.

The enum values are TRACE = 0, INFO = 1, WARN = 2, and ERROR = 3. This order allows a numeric comparison with the threshold. Log ignores messages below the threshold. At INFO, TRACE is hidden. At ERROR, only errors appear.

The format supports time, message, logger name, and message level using percent t, percent m, percent n, and percent l. Ordinary punctuation and text stay in the output. The extra credit percent r chooses a random phrase suitable for the message level. An ERROR message gets an error phrase even when the threshold is TRACE.

## Validation (about 60 seconds)

The validator scans from left to right with a balance counter. An opening parenthesis increases balance and logs TRACE. A closing parenthesis needs a previous unmatched opening. If one exists, balance decreases and INFO reports a pair. Otherwise, WARN reports an unmatched closing and the input is invalid.

At the end, balance must be zero. Invalid input produces ERROR containing the input. The final Boolean result is logged at INFO, subject to filtering. Empty input is valid. Characters other than parentheses are ignored. Each character is visited at most once, so the time is O(n), and the extra space is constant.

## Demonstration and conclusion (about 45 seconds)

Running `dotnet run` shows the four required tests and five edge cases, including empty input and incorrect ordering. All 19 checks passed during verification: nine validation cases, four logger threshold checks, and six formatting and bonus checks. The output shows PASS/FAIL alongside expected and actual values. The demonstration then shows all levels, random phrases, two logger names, and an ERROR-only configuration with a changed format. Automated checks verify the timestamp format and random phrase category. The formatter reads the format once, so placeholder text inside a name or message remains literal.

The main OOP concepts are private instance state, shared static configuration, and composition. Shared settings keep this program simple, but they also mean individual loggers cannot have independent configurations.

## Possible professor questions

- What is the difference between a class and an object? A class defines structure and behavior; an object is an instance with its own state.
- Why is the name an instance member? Different loggers identify different sources.
- Why are Level and Format static? Every logger shares the same configuration.
- Does changing Level affect loggers already created? Yes; they read the same static field on each call.
- What is encapsulated? The logger name, random-message helper, and validator logger are private. Configuration fields are public in this simple implementation.
- Why does filtering work with a comparison? Enum values increase from TRACE to ERROR.
- Does filtering change validation? No. It changes console output, not the Boolean algorithm.
- Why is checking only the final counts insufficient? A closing parenthesis can appear before any opening, as in `)(`.
- Why is empty input valid? There are no unmatched parentheses.
- What happens to letters? They are ignored. Null is not supported.
- Is this inheritance or polymorphism? These are separate classes using composition; no custom inheritance hierarchy or polymorphic design is needed.
- How does percent r work? A switch chooses the message-level phrase array, and Random.Shared.Next selects an index. Repeats are allowed.
- How are tests checked without a framework? Validation results and captured logger output are compared with expectations, and failures set a nonzero exit code.


