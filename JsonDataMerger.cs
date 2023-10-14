using Newtonsoft.Json.Linq;

namespace export_data;

public class JsonDataMerger
{
    private const string ExportResultFolderName = "merge-result";
    private const string ExportResultFileName = "merged.json";

    private readonly string _exportDirectoryPath;

    public JsonDataMerger(string exportDirectoryPath)
    {
        if (!Directory.Exists(exportDirectoryPath))
        {
            throw new DirectoryNotFoundException($"The specified directory does not exist: {exportDirectoryPath}");
        }

        _exportDirectoryPath = exportDirectoryPath;
    }

    public void MergeDirectoryJsonFiles()
    {
        using var resultFile = CreateNewResultFile();
        
        FindDirectoryFilesAndMergeThem(resultFile);
    }

    private void FindDirectoryFilesAndMergeThem(TextWriter resultJsonFile)
    {
        resultJsonFile.WriteLine("[");

        var files = Directory.GetFiles(_exportDirectoryPath, "*.json");
        var dataFileLength = files.Length;

        for (var i = 0; i < dataFileLength; i++)
        {
            var filePath = files[i];
            var fileContent = File.ReadAllText(filePath);

            var fileJsonObjects =
                fileContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var fileJsonObjectCount = fileJsonObjects.Length;

            for (var j = 0; j < fileJsonObjectCount; j++)
            {
                var jsonObject = fileJsonObjects[j];

                if (string.IsNullOrWhiteSpace(jsonObject)) continue;

                if (i == dataFileLength - 1 && j == fileJsonObjectCount - 1)
                {
                    resultJsonFile.WriteLine(JObject.Parse(jsonObject));
                }
                else
                {
                    resultJsonFile.WriteLine(JObject.Parse(jsonObject) + ",");
                }
            }

            Console.WriteLine($"Completed File {i + 1} of {dataFileLength}.");

            resultJsonFile.Flush();
        }

        resultJsonFile.WriteLine("]");

        Console.WriteLine("Merge Complete.");
    }

    private StreamWriter CreateNewResultFile()
    {
        var resultPath = Path.GetFullPath($@"{_exportDirectoryPath}\{ExportResultFolderName}\{ExportResultFileName}");

        if (File.Exists(resultPath)) File.Delete(resultPath);

        Directory.CreateDirectory($@"{_exportDirectoryPath}\{ExportResultFolderName}");

        var resultFile = File.CreateText(resultPath);

        return resultFile;
    }
}