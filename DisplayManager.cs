using System;
using System.Collections.Generic;

class DisplayManager
{
    private string name;
    private List<Display> assignedDisplays = new List<Display>();

    public DisplayManager(string name)
    {
        this.name = name;
    }

    public void AssignDisplay(Display display)
    {
        assignedDisplays.Add(display);
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
        if (assignedDisplays.Remove(display))
            return display;

        return null;
    }
}