using Newtonsoft.Json;

namespace export_data;

public class CommandGenerator
{
    private string _jsonFilePath;

    public CommandGenerator(string jsonFilePath)
    {
        if (!File.Exists(jsonFilePath))
        {
            throw new DirectoryNotFoundException($"The specified directory does not exist: {jsonFilePath}");
        }

        _jsonFilePath = jsonFilePath;
    }

    public void PrintSqlTableCreationCommand()
    {
        var jsonObjectStr = GetFirstLineOfJson();

        var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonObjectStr);

        var sqlCommand = $"CREATE TABLE YourTableName (\n";

        sqlCommand = jsonObject.Keys.Aggregate(sqlCommand, (current, key) => current + $"    {key} NVARCHAR(250),\n");

        sqlCommand = sqlCommand.TrimEnd(',', '\n') + "\n);";

        Console.WriteLine(sqlCommand);
    }

    public void PrintSqlInsertCommand()
    {
        var jsonObjectStr = GetFirstLineOfJson();

        var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonObjectStr);

        // Start constructing the SQL command string
        var columns = string.Join(", ", jsonObject.Keys);
        var sqlCommand = $@"
INSERT INTO YourTableName ({columns})
        SELECT {columns}
        FROM OPENROWSET(BULK @filepath, SINGLE_CLOB) AS json
";

        Console.WriteLine(sqlCommand);
    }

    private string GetFirstLineOfJson()
    {
        using var streamReader = new StreamReader(_jsonFilePath);

        return streamReader.ReadLine();
    }
}