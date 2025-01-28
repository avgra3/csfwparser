using fwparser.lib;
using Cocona;

CoconaApp.Run((
        [Argument(Description = "A file or standard input to be read")] string? input,
        [Option(Description = "The TOML Configuration file")] string tomlFile,
        [Option('h', Description = "Change the default configuration table name from \"Configuration\"")] string tableName,
        [Option('d', Description = "Set the delimiter to use as output. Default = \",\"")] char delimiter,
        [Option('w', Description = "Overwrite the file if it exists. Defaults to true")] bool overwrite,
        [Option('t', Description = "Trim whitespace on output. Default is false")] bool trimWhitespace,
        [Option('i', Description = "If the starting index of the configuration file is not zero")] int offset,
    [Option('o', Description = "Output to a file on disk. Defaults to sending to standard out")] string? outputFileName) =>
{
    if (input is null && Console.IsInputRedirected)
    {
        string tempFilePath = Path.GetTempFileName();
        try
        {
            using (TextReader reader = Console.In)
            using (var writer = new StreamWriter(tempFilePath))
            {
                writer.Write(reader.ReadToEnd());
            }
            input = tempFilePath;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occured: {e.Message}");
        }
    }
    if (tableName is null)
    {
        tableName = "Configuration";
    }
    Dictionary<string, int[]> tomlConfig = Utilities.TomlFileToDictionary(
            tomlFile: tomlFile,
            configurationHeaderName: tableName
        );
    ValidDelimiter validDelim = Utilities.GetDelimiterEnum(delimiter);
    if (outputFileName is null)
    {
        string data = Parser.parseDataFile(
                rawDataFile: input ?? "",
                headerConfig: tomlConfig,
                trimWhiteSpace: trimWhitespace,
                offset: offset,
        delimiter: validDelim
            );
        Console.WriteLine("The output from your input is...");
        Console.WriteLine(data);

    }
    else
    {
        Utilities.OutputToFile(
                inputData: input ?? "",
                outputFileName: outputFileName,
                headerConfig: tomlConfig,
                trimWhiteSpace: trimWhitespace,
                offset: offset,
                delimiter: validDelim,
                overWriteFileIfExists: overwrite);
        Console.WriteLine($"Your file has been saved to {outputFileName}");
    }
});
