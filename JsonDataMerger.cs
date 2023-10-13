using Newtonsoft.Json.Linq;

namespace export_data;

public class JsonDataMerger
{
    private string _exportDirectoryPath;

    public JsonDataMerger(string exportDirectoryPath)
    {
        if (!Directory.Exists(exportDirectoryPath))
        {
            throw new DirectoryNotFoundException($"The specified directory does not exist: {exportDirectoryPath}");
        }

        _exportDirectoryPath = exportDirectoryPath;
    }

    public void MergeDirectoryJsons()
    {
        if (Directory.Exists(Path.Combine(_exportDirectoryPath, "merged.json")))
        {
            File.Delete(Path.Combine(_exportDirectoryPath, "merged.json"));
        }
        
        using var destinationJsonFile = File.CreateText(Path.Combine(_exportDirectoryPath, "merged.json"));

        destinationJsonFile.WriteLine("[");

        var files = Directory.GetFiles(_exportDirectoryPath, "*.json");
        var dataFileLength = files.Length - 1;
        for (var i = 0; i < dataFileLength; i++)
        {
            var filePath = files[i];
            var fileContent = File.ReadAllText(filePath);
            var jsonObjects = fileContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries); 
            
            for (var j = 0; j < jsonObjects.Length; j++)
            {
                var jsonObject = jsonObjects[j];

                if (string.IsNullOrWhiteSpace(jsonObject)) continue;
                
                if (i == dataFileLength - 1 && j == jsonObjects.Length - 1)
                {
                    destinationJsonFile.WriteLine(JObject.Parse(jsonObject));
                }
                else
                {
                    destinationJsonFile.WriteLine(JObject.Parse(jsonObject) + ",");
                }
            }

            Console.WriteLine($"Completed File {i + 1} of {dataFileLength}");
            
            destinationJsonFile.Flush();
        }
        
        destinationJsonFile.WriteLine("]");
        
        Console.WriteLine($"Merge Complete.");
    }
}