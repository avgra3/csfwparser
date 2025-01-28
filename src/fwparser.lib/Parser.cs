namespace fwparser.lib;

public static class Parser
{
    public static List<string> GetColumnNames(
            Dictionary<string, int[]> headerConfig
            )
    {
        List<Tuple<string, int>> headers = new();
        foreach (string key in headerConfig.Keys)
        {
            headers.Add(new Tuple<string, int>(key, headerConfig[key][0]));
        }

        List<string> sortedHeaders = headers.OrderBy(x => x.Item2).Select(x => x.Item1).ToList();
        return sortedHeaders;
    }

    public static Dictionary<string, string> ParseDataByLine(
            Dictionary<string, int[]> headerConfig,
            string rawDataLine,
            bool trimWhiteSpace = false,
            int offset = 0)
    {
        Dictionary<string, string> parsedFields = new();
        foreach (string key in headerConfig.Keys)
        {
            string header = key;
            int startValue = headerConfig[key][0] - offset;
            if (startValue < 0)
            {
                throw new IndexOutOfRangeException($"The index of value '{startValue} is less than zero (NOT ALLOWED). Please check your configuration.");
            }
            int endValue = headerConfig[key][1];

            string data = rawDataLine.Substring(startIndex: startValue, length: endValue);
            if (trimWhiteSpace)
            {
                data = data.Trim();
            }
            parsedFields.Add(header, data);
        }
        return parsedFields;
    }

    public static List<Dictionary<string, string>> ParseAllData(
            List<string> allData,
            Dictionary<string, int[]> headerConfig,
            bool trimWhiteSpace = false,
            int offset = 0)
    {
        List<Dictionary<string, string>> result = new();
        foreach (string line in allData)
        {
            Dictionary<string, string> rawData = ParseDataByLine(
                headerConfig: headerConfig,
                rawDataLine: line,
                trimWhiteSpace: trimWhiteSpace,
                offset: offset
             );

            result.Add(rawData);
        }

        return result;
    }

    public static List<string> SplitRawData(string rawDataFile)
    {
        List<string> data = new();
        // if path is file: read and split lines, return result
        if (File.Exists(rawDataFile))
        {
            data = File.ReadLines(rawDataFile).ToList();
            return data;
        }
        // if is directory: return error specifying it is a directory
        if (Directory.Exists(rawDataFile))
        {
            throw new Exception($"The filepath, \"{rawDataFile}\" is  a directory. Please try again.");
        }
        // if the "file" is actually just a string, split by line endings
        if (rawDataFile is string)
        {
            data = rawDataFile.Split(
                    new string[] {
                "\r\n",
                "\r",
                "\n"
                    },
                    StringSplitOptions.None
                    ).ToList();
            return data;
        }
        // otherwise return Exception
        throw new Exception($"The raw data path you included is neither a valid file path or of type string:\n{rawDataFile}");
    }

    public static string parseDataFile(
            string rawDataFile,
            Dictionary<string, int[]> headerConfig,
            bool trimWhiteSpace = false,
            int offset = 0,
            ValidDelimiter delimiter = ValidDelimiter.COMMA
            )
    {
        string parsedData = "";

        List<string> headers = GetColumnNames(headerConfig: headerConfig);
        List<string> rawDataList = SplitRawData(rawDataFile: rawDataFile);
        List<Dictionary<string, string>> dataList = ParseAllData(
            allData: rawDataList,
            headerConfig: headerConfig,
            trimWhiteSpace: trimWhiteSpace,
            offset: offset
    );
        char delim = Utilities.GetDelimiter(eDelim: delimiter);

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
                data += line[value] + $"{delim}";
            }
            parsedData += data.TrimEnd(delim) + "\r\n";
        }
        return parsedData;
    }
}
