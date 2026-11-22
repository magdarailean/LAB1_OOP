using System;
using System.Collections.Generic;
using System.Globalization;

static class CommandParser
{
    public static void ParseCommand(string line, List<DisplayManager> managers,
        List<Display> displays, Logger shopLogger, Logger managerLogger)
    {
        string[] words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (words.Length == 0 || words[0].StartsWith("#"))
            return;

        string command = words[0];

        if (command == "MANAGER" && words.Length == 2)
        {
            managers.Add(new DisplayManager(words[1], managerLogger));
        }
        else if (command == "DISPLAY" && words.Length == 5)
        {
            if (int.TryParse(words[2], out int width) &&
                int.TryParse(words[3], out int height) &&
                float.TryParse(words[4], NumberStyles.Float,
                    CultureInfo.InvariantCulture, out float ppi))
            {
                Display display = new Display(words[1], width, height, ppi);
                displays.Add(display);
                shopLogger.Log(LogLevel.TRACE, $"Display {display.Model} created");
            }
        }
        else if ((command == "ASSIGN" || command == "BUY") && words.Length == 3)
        {
            var display = FindDisplay(displays, words[1]);
            var manager = FindManager(managers, words[2]);

            if (display == null || manager == null)
                return;

            if (command == "ASSIGN")
                manager.AssignDisplay(display);
            else
                manager.BuyDisplay(display);
        }
        else if (command == "COMPARE" && words.Length == 3)
        {
            var first = FindDisplay(displays, words[1]);
            var second = FindDisplay(displays, words[2]);

            if (first != null && second != null)
                Console.WriteLine(first.CompareWithMonitor(second));
        }
        else if (command == "ASSIST" && words.Length == 2)
        {
            var manager = FindManager(managers, words[1]);

            if (manager != null)
                manager.Assist();
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