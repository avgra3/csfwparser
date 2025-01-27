using fwparser.lib;

namespace fwparser.tests;

public class FwparserLibTests
{
    [Fact]
    public void TestParseDataString()
    {
        string rawData = "12345John      Doe       123 Main St         1234567890";
        Dictionary<string, int[]> dataOutline = new();
        dataOutline.Add("customer_id", new int[] { 1, 5 });
        dataOutline.Add("first_name", new int[] { 6, 10 });
        dataOutline.Add("last_name", new int[] { 16, 10 });
        dataOutline.Add("address", new int[] { 26, 20 });
        dataOutline.Add("phone_number", new int[] { 46, 10 });

        string expectedResult = "customer_id,first_name,last_name,address,phone_number\r\n" + "12345,John,Doe,123 Main St,1234567890\r\n";

        string actualResult = Parser.parseDataFile(
                rawDataFile: rawData,
                headerConfig: dataOutline,
                trimWhiteSpace: true,
                offset: 1);

        Assert.Equal(actualResult, expectedResult);
    }

    [Fact]
    public void TestParseDataFile()
    {
        string rawDataFile = "foo.txt";
        Dictionary<string, int[]> dataOutline = new();
        dataOutline.Add("customer_id", new int[] { 1, 5 });
        dataOutline.Add("first_name", new int[] { 6, 10 });
        dataOutline.Add("last_name", new int[] { 16, 10 });
        dataOutline.Add("address", new int[] { 26, 20 });
        dataOutline.Add("phone_number", new int[] { 46, 10 });

        string expectedResult = "customer_id,first_name,last_name,address,phone_number\r\n"
            + "12345,John,Doe,123 Main St,1234567890\r\n"
            + "12345,John,Doe,123 Main St,1234567890\r\n"
            + "12345,John,Doe,123 Main St,1234567890\r\n"
            + "12345,John,Doe,123 Main St,1234567890\r\n"
            + "12345,John,Doe,123 Main St,1234567890\r\n"
            + "12345,John,Doe,123 Main St,1234567890\r\n"
            + "12345,John,Doe,123 Main St,1234567890\r\n"
            + "12345,John,Doe,123 Main St,1234567890\r\n"
            + "12345,John,Doe,123 Main St,1234567890\r\n"
            + "12345,John,Doe,123 Main St,1234567890\r\n";

        string actualResult = Parser.parseDataFile(
                rawDataFile: rawDataFile,
                headerConfig: dataOutline,
                trimWhiteSpace: true,
                offset: 1);

        Assert.Equal(actualResult, expectedResult);
    }

    [Fact]
    public void TestTomlFileToDictionary()
    {
        string inputToml = "./fooConfig.toml";
        string configHeader = "Configuration";
        Dictionary<string, int[]> expectedResult = new();
        expectedResult.Add("customer_id", new int[] { 0, 5 });
        expectedResult.Add("first_name", new int[] { 5, 10 });
        expectedResult.Add("last_name", new int[] { 15, 10 });
        expectedResult.Add("address", new int[] { 25, 20 });
        expectedResult.Add("phone_number", new int[] { 45, 10 });

        var actualResult = Utilities.TomlFileToDictionary(
                tomlFile: inputToml,
                configurationHeaderName: configHeader
        );

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void TestParseToDataFile()
    {
        string outputFilePath = "./testCase.csv";

        string fixedWidthFile = "./foo.txt";
        Dictionary<string, int[]> dataOutline = new();
        dataOutline.Add("customer_id", new int[] { 0, 5 });
        dataOutline.Add("first_name", new int[] { 5, 10 });
        dataOutline.Add("last_name", new int[] { 15, 10 });
        dataOutline.Add("address", new int[] { 25, 20 });
        dataOutline.Add("phone_number", new int[] { 45, 10 });

        string expectedOutput = "customer_id|first_name|last_name|address|phone_number\r\n"
            + "12345|John|Doe|123 Main St|1234567890\r\n"
            + "12345|John|Doe|123 Main St|1234567890\r\n"
            + "12345|John|Doe|123 Main St|1234567890\r\n"
            + "12345|John|Doe|123 Main St|1234567890\r\n"
            + "12345|John|Doe|123 Main St|1234567890\r\n"
            + "12345|John|Doe|123 Main St|1234567890\r\n"
            + "12345|John|Doe|123 Main St|1234567890\r\n"
            + "12345|John|Doe|123 Main St|1234567890\r\n"
            + "12345|John|Doe|123 Main St|1234567890\r\n"
            + "12345|John|Doe|123 Main St|1234567890\r\n";


        Utilities.OutputToFile(
                inputData: fixedWidthFile,
                outputFileName: outputFilePath,
                headerConfig: dataOutline,
                trimWhiteSpace: true,
                offset: 0,
                delimiter: ValidDelimiter.PIPE,
                overWriteFileIfExists: true);

        string actualOutput = File.ReadAllText(outputFilePath);
        File.Delete(outputFilePath);
        Assert.Equal(expectedOutput, actualOutput);
    }
}

