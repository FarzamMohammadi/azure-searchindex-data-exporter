using export_data.CommandGeneration;
using Newtonsoft.Json;

namespace export_data;

public class JsonCommandGenerator : CommandGenerator
{
    public JsonCommandGenerator(string jsonFilePath) : base(jsonFilePath)
    {
    }

    public void PrintSqlTableCreationCommand()
    {
        var jsonObjectStr = GetFirstLineOfFile();

        var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonObjectStr);

        var sqlCommand = $"CREATE TABLE YourTableName (\n";

        sqlCommand = jsonObject.Keys.Aggregate(sqlCommand, (current, key) => current + $"    {key} NVARCHAR(250),\n");

        sqlCommand = sqlCommand.TrimEnd(',', '\n') + "\n);";

        Console.WriteLine(sqlCommand);
    }

    public void PrintSqlInsertCommand()
    {
        var jsonObjectStr = GetFirstLineOfFile();

        var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonObjectStr);

        var columns = string.Join(", ", jsonObject.Keys);
        var sqlCommand = $@"
INSERT INTO YourTableName ({columns})
        SELECT {columns}
        FROM OPENROWSET(BULK @filepath, SINGLE_CLOB) AS json
";

        Console.WriteLine(sqlCommand);
    }
}