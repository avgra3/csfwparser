using fwparser.lib;

// Showcasing what the fwparser library can do!
// Assuming foo.txt contains the below
// 12345John      Doe       123 Main St         1234567890

string fixedWidthFile = "../foo.txt";

Dictionary<string, int[]> dataOutline = new();
dataOutline.Add("customer_id", new int[] { 0, 5 });
dataOutline.Add("first_name", new int[] { 5, 10 });
dataOutline.Add("last_name", new int[] { 15, 10 });
dataOutline.Add("address", new int[] { 25, 20 });
dataOutline.Add("phone_number", new int[] { 45, 10 });

var data = Parser.parseDataFile(
        rawDataFile: fixedWidthFile,
        headerConfig: dataOutline,
        trimWhiteSpace: true, // Defaults to false
        offset: 0 // Only needed if your config does not have an index of zero
        );

Console.WriteLine(data);
/*
customer_id,first_name,last_name,address,phone_number
12345,John,Doe,123 Main St,1234567890
12345,John,Doe,123 Main St,1234567890
12345,John,Doe,123 Main St,1234567890
12345,John,Doe,123 Main St,1234567890
12345,John,Doe,123 Main St,1234567890
12345,John,Doe,123 Main St,1234567890
12345,John,Doe,123 Main St,1234567890
12345,John,Doe,123 Main St,1234567890
12345,John,Doe,123 Main St,1234567890
12345,John,Doe,123 Main St,1234567890
*/


// What if we wrote to a file?
Utilities.OutputToFile(
                inputData: fixedWidthFile,
                outputFileName: "./foo_cleaned.csv",
                headerConfig: dataOutline,
                trimWhiteSpace: true,
                offset: 0,
                delimiter: ValidDelimiter.PIPE,
                overWriteFileIfExists: true);

// Check we actually wrote to the file
var exampleOutput = File.ReadAllText("./foo_cleaned.csv");
Console.WriteLine(exampleOutput);
/*
customer_id|first_name|last_name|address|phone_number
12345|John|Doe|123 Main St|1234567890
12345|John|Doe|123 Main St|1234567890
12345|John|Doe|123 Main St|1234567890
12345|John|Doe|123 Main St|1234567890
12345|John|Doe|123 Main St|1234567890
12345|John|Doe|123 Main St|1234567890
12345|John|Doe|123 Main St|1234567890
12345|John|Doe|123 Main St|1234567890
12345|John|Doe|123 Main St|1234567890
12345|John|Doe|123 Main St|123456789
*/

// Delete the file we created
File.Delete("./foo_cleaned.csv");


// Using a Toml file!
string ourTomlFile = "../fooConfig.toml";

Dictionary<string, int[]> tomlConfig = Utilities.TomlFileToDictionary(
                tomlFile: ourTomlFile,
                configurationHeaderName: "Configuration"); // You can define the name or use the default
// What if we wrote to a file with our configuration file
Utilities.OutputToFile(
                inputData: fixedWidthFile,
                outputFileName: "./foo_toml_cleaned.csv",
                headerConfig: tomlConfig,
                trimWhiteSpace: true,
                offset: 0,
                delimiter: ValidDelimiter.PIPE,
                overWriteFileIfExists: true);

// Check we actually wrote to the file
var tomlExampleOutput = File.ReadAllText("./foo_toml_cleaned.csv");
Console.WriteLine(tomlExampleOutput);

// Delete the file we created
File.Delete("./foo_toml_cleaned.csv");

