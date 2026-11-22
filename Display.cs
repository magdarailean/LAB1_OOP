using System;

class Display
{
    public string Model { get; private set; }
    private int width;
    private int height;
    private float ppi;


    public Display(string model, int width, int height, float ppi)
    {
        Model = model;
        this.width = width;
        this.height = height;
        this.ppi = ppi;
    }

    public double Diagonal()
    {
        double diagonal = (Math.Sqrt(width * width + height * height)) / ppi;
        return diagonal;
    }
    
    public int CompareSize(Display other)
    {
        double myDiagonal = Diagonal();
        double otherDiagonal = other.Diagonal();

        if (myDiagonal > otherDiagonal)
            return 1;

        if (myDiagonal < otherDiagonal)
            return -1;

        return 0;
    }

    public int CompareSharpness(Display other)
    {
        if (ppi > other.ppi)
            return 1;

        if (ppi < other.ppi)
            return -1;

        return 0;
    }

    public string CompareWithMonitor(Display other)
    {
        int size = CompareSize(other);
        int sharpness = CompareSharpness(other);

        string sizeResult = "the same size";
        if (size > 0)
            sizeResult = "larger";
        else if (size < 0)
            sizeResult = "smaller";

        string sharpnessResult = "equally sharp";
        if (sharpness > 0)
            sharpnessResult = "sharper";
        else if (sharpness < 0)
            sharpnessResult = "less sharp";

        return $"{Model} ({Diagonal()} in) vs {other.Model} ({other.Diagonal()} in): {sizeResult} and {sharpnessResult}.";
    }
}