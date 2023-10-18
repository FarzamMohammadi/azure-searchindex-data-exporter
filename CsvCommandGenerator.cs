using export_data.CommandGeneration;

namespace export_data;

public class CsvCommandGenerator : CommandGenerator
{
    public CsvCommandGenerator(string filePath) : base(filePath)
    {
    }

    public void PrintSqlTableCreationCommand()
    {
        var csvObjectStr = GetFirstLineOfFile();

        var csvObject = csvObjectStr.Split(',');

        var sqlCommand = $"CREATE TABLE YourTableName (\n";

        sqlCommand = csvObject.Aggregate(sqlCommand, (current, key) => current + $"    {key} NVARCHAR(250),\n");

        sqlCommand = sqlCommand.TrimEnd(',', '\n') + "\n);";

        Console.WriteLine(sqlCommand);
    }
}