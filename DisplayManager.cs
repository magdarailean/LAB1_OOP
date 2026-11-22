using System;
using System.Collections.Generic;

class DisplayManager
{
    public string name { get; private set; }
    private List<Display> assignedDisplays = new List<Display>();
    
    private Logger logger;

    public DisplayManager(string name, Logger logger)
    {
        this.name = name;
        this.logger = logger;

        logger.Log(LogLevel.TRACE, $"Manager {name} created");
    }
    
    public int DisplayCount
    {
        get { return assignedDisplays.Count; }
    }
    

    public void AssignDisplay(Display display)
    {
        assignedDisplays.Add(display);
        logger.Log(LogLevel.TRACE, $"{display.Model} assigned to {name}");
    }
    
    public bool HasDisplay(Display display)
    {
        return assignedDisplays.Contains(display);
    }

    public void Assist()
    {
        for (int i = 0; i < assignedDisplays.Count - 1; i++)
        {
            Display current = assignedDisplays[i];
            Display next = assignedDisplays[i + 1];

            string result = current.CompareWithMonitor(next);
            Console.WriteLine(result);
        }
    }

    public Display? BuyDisplay(Display display)
    {
        if (!assignedDisplays.Remove(display))
        {
            logger.Log(LogLevel.ERROR, $"{name} does not have {display.Model}");
            return null;
        }

        logger.Log(LogLevel.INFO, $"{name} sold {display.Model}");

        if (assignedDisplays.Count == 0)
            logger.Log(LogLevel.WARN, $"{name} has no displays left");

        return display;
    }
}