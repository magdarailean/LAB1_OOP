using System;
using System.Collections.Generic;
using System.Globalization;

static class CommandParser
{
    public static void ParseCommand(string line, List<DisplayManager> managers,
        List<Display> displays, Logger shopLogger, Logger managerLogger, int lineNumber)
    {
        string[] words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (words.Length == 0 || words[0].StartsWith("#"))
            return;

        string command = words[0];

        if (command == "MANAGER" && words.Length == 2)
        {
            if (FindManager(managers, words[1]) != null)
            {
                shopLogger.Log(LogLevel.ERROR, $"Line {lineNumber}: Manager {words[1]} already exists");
                return;
            }

            managers.Add(new DisplayManager(words[1], managerLogger));
        }
        else if (command == "DISPLAY" && words.Length == 5)
        {
            if (int.TryParse(words[2], out int width) && int.TryParse(words[3], out int height) &&
                float.TryParse(words[4], NumberStyles.Float,
                    CultureInfo.InvariantCulture, out float ppi))
            {
                Display display = new Display(words[1], width, height, ppi);
                displays.Add(display);
                shopLogger.Log(LogLevel.TRACE, $"Display {display.Model} created");
            }
            else
            {
                shopLogger.Log(LogLevel.ERROR, $"Line {lineNumber}: Invalid display numbers");
            }
        }
        else if ((command == "ASSIGN" || command == "BUY") && words.Length == 3)
        {
            var display = FindDisplay(displays, words[1]);
            var manager = FindManager(managers, words[2]);

            if (display == null)
            {
                shopLogger.Log(LogLevel.ERROR,
                    $"Line {lineNumber}: Display {words[1]} not found");
                return;
            }

            if (manager == null)
            {
                shopLogger.Log(LogLevel.ERROR,
                    $"Line {lineNumber}: Manager {words[2]} not found");
                return;
            }

            if (command == "ASSIGN")
            {
                if (manager.HasDisplay(display))
                {
                    shopLogger.Log(LogLevel.WARN,
                        $"Line {lineNumber}: {display.Model} is already assigned to {manager.name}");
                    return;
                }

                manager.AssignDisplay(display);
            }
            else
            {
                if (!manager.HasDisplay(display))
                {
                    shopLogger.Log(LogLevel.ERROR,
                        $"Line {lineNumber}: {manager.name} does not have {display.Model}");
                    return;
                }

                manager.BuyDisplay(display);
            }
        }
        else if (command == "COMPARE" && words.Length == 3)
        {
            var first = FindDisplay(displays, words[1]);
            var second = FindDisplay(displays, words[2]);

            if (first == null)
                shopLogger.Log(LogLevel.ERROR, $"Line {lineNumber}: Display {words[1]} not found");
            else if (second == null)
                shopLogger.Log(LogLevel.ERROR, $"Line {lineNumber}: Display {words[2]} not found");
            else
                Console.WriteLine(first.CompareWithMonitor(second));
        }
        else if (command == "ASSIST" && words.Length == 2)
        {
            var manager = FindManager(managers, words[1]);

            if (manager == null)
                shopLogger.Log(LogLevel.ERROR, $"Line {lineNumber}: Manager {words[1]} not found");
            else if (manager.DisplayCount == 0)
                shopLogger.Log(LogLevel.WARN, $"Line {lineNumber}: {manager.name} has no displays to compare");
            else
                manager.Assist();
        }
        else if (command == "MANAGER" || command == "DISPLAY" ||
                 command == "ASSIGN" || command == "BUY" ||
                 command == "COMPARE" || command == "ASSIST")
        {
            shopLogger.Log(LogLevel.ERROR,
                $"Line {lineNumber}: Wrong number of arguments: {line}");
        }
        else
        {
            shopLogger.Log(LogLevel.ERROR,
                $"Line {lineNumber}: Unknown command: {command}");
        }
    }

    static Display? FindDisplay(List<Display> displays, string model)
    {
        foreach (Display display in displays)
        {
            if (display.Model == model)
                return display;
        }

        return null;
    }

    static DisplayManager? FindManager(List<DisplayManager> managers, string name)
    {
        foreach (DisplayManager manager in managers)
        {
            if (manager.name == name)
                return manager;
        }

        return null;
    }
}