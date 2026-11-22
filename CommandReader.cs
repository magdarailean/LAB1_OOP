using System.IO;

class CommandReader
{
    private Logger logger;

    public CommandReader(Logger logger)
    {
        this.logger = logger;
    }

    public string[] Read(string path)
    {
        if (!File.Exists(path))
        {
            logger.Log(LogLevel.ERROR, $"File not found: {path}");
            return new string[0];
        }

        return File.ReadAllLines(path);
    }
}