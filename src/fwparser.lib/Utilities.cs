namespace fwparser.lib;

public class Utilities
{
    public static Dictionary<string, int[]> TomlFileToDicionary()
    {
        // TODO: Take in a toml file
        // TODO: Map to the necessary dicionary format we need
        throw new NotImplementedException("This is currently not implemented.");
    }

    public static void OutputToFile(
            string inputData,
            string outputFileName,
            Dictionary<string, int[]> headerConfig,
            bool trimWhiteSpace = false,
            int offset = 0,
            ValidDelimiter delimiter = ValidDelimiter.COMMA,
            bool overWriteFileIfExists = true
            )
    {
        // Fail early if outputFileName is is null
        if (outputFileName is null)
        {
            Console.WriteLine("ERROR: The output file name was _null_. Please try again.");
            return;
        }
        // Fail early if overWriteFileIfExists == false and file exists
        if (File.Exists(outputFileName) && overWriteFileIfExists)
        {
            Console.WriteLine($"ERROR: the file {outputFileName} already exists and the overWriteFileIfExists was set to _false_.");
            return;
        }

        char delim;
        switch (delimiter)
        {
            case ValidDelimiter.COLON:
                {
                    delim = ':';
                    break;
                }
            case ValidDelimiter.SEMICOLON:
                {
                    delim = ';';
                    break;
                }
            case ValidDelimiter.COMMA:
                {
                    delim = ',';
                    break;
                }
            case ValidDelimiter.PIPE:
                {
                    delim = '|';
                    break;
                }
            default:
                {
                    delim = ',';
                    break;
                }
        }
        ;

        string parsedData = "";

        List<string> headers = Parser.GetColumnNames(headerConfig: headerConfig);
        List<string> rawDataList = Parser.SplitRawData(rawDataFile: inputData);
        List<Dictionary<string, string>> dataList = Parser.ParseAllData(
            allData: rawDataList,
            headerConfig: headerConfig,
            trimWhiteSpace: trimWhiteSpace,
            offset: offset
    );

        foreach (string header in headers)
        {
            parsedData += $"{header}{delim}";
        }
        parsedData = parsedData.TrimEnd(delim) + "\r\n";

        foreach (Dictionary<string, string> line in dataList)
        {
            string data = "";
            foreach (string value in line.Keys)
            {
                data += line[value] + delim;
            }
            parsedData += data.TrimEnd(delim) + "\r\n";
        }
        if (overWriteFileIfExists)
        {
            File.WriteAllText(outputFileName, parsedData);
        }
        if (!overWriteFileIfExists)
        {
            File.AppendAllText(outputFileName, parsedData);
        }
        return;
    }
}
