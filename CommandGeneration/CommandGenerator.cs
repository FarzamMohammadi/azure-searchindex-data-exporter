namespace export_data.CommandGeneration;

public class CommandGenerator
{
    private readonly string _filePath;

    protected CommandGenerator(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new DirectoryNotFoundException($"The specified directory does not exist: {filePath}");
        }

        _filePath = filePath;
    }
    
    protected string GetFirstLineOfFile()
    {
        using var streamReader = new StreamReader(_filePath);

        return streamReader.ReadLine();
    }
}