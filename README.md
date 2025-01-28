# C# Fixed Width Parser (CSFWParser)

A simple library to help you parse your fixed width files, written in pure C#.

You might be wondering why the name. Simple: I have a [Python version](https://github.com/avgra3/fwparser/) with a similar name. Go check it out if you're interested.

## Example

This code is also located [here](./src/fwparser.example/Program.cs).

```cs
using fwparser.lib;

// Showcasing what the fwparser library can do!
// Assuming foo.txt contains the below (with many other lines of the same text).
// 12345John      Doe       123 Main St         1234567890

string fixedWidthFile = "foo.txt";

Dictionary<string, int[]> dataOutline = new();
dataOutline.Add("customer_id", new int[] { 1, 5 });
dataOutline.Add("first_name", new int[] { 6, 10 });
dataOutline.Add("last_name", new int[] { 16, 10 });
dataOutline.Add("address", new int[] { 26, 20 });
dataOutline.Add("phone_number", new int[] { 46, 10 });

var data = Parser.parseDataFile(
        rawDataFile: fixedWidthFile,
        headerConfig: dataOutline,
        trimWhiteSpace: true, // Defaults to false.
        offset: 1 // Only needed if your config does not have an index of zero
        );

Console.WriteLine(data);

/* This would be the expected result
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
```

### CLI tool

This assumes you installed as a tool. For information on how to do this, it is detailed [below](##building-cli-tool).

```bash
fwparser --help

# Usage: fwparser [--toml-file <String>] [--table-name <String>] [--delimiter <Char>] [--overwrite] [--trim-whitespace] [--offset <Int32>] [--output-file-name <String>] [--help] [--version] input
#
# fwparser.cli
#
# Arguments:
#   0: input    A file or standard input to be read
#
# Options:
#   --toml-file <String>               The TOML Configuration file (Required)
#   -h, --table-name <String>          Change the default configuration table name from "Configuration" (Required)
#   -d, --delimiter <Char>             Set the delimiter to use as output. Default = "," (Required)
#   -w, --overwrite                    Overwrite the file if it exists. Defaults to true
#   -t, --trim-whitespace              Trim whitespace on output. Default is false
#   -i, --offset <Int32>               If the starting index of the configuration file is not zero (Required)
#   -o, --output-file-name <String>    Output to a file on disk. Defaults to sending to standard out
#   --help                             Show help message
#   --version                          Show version
```

## Motivation

You might be wondering why this library exists. I found that working width fixed width files was a hassle each time I worked with a new one. To be clear, they still are but at least I have an easy tool that I can use to work with them and I can more easily convert them to CSV files (or other delimited file types).

Personally, I like using a TOML file for configuration of the different fields then using that in place of the data outline dictionary I use in the example.

## Dependencies

- [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (although other versions would probably work, but are not tested here).
- [Cocona](https://github.com/mayuki/Cocona).
- [xunit](https://xunit.net/) (For testing purposes only).

## Building Cli tool

This package is not on NuGet but if you want to use it yourself, use the below commands. Please note that this project is used by me and therefore may not be the best tool for you.

```bash
git clone https://github.com/avgra3/csfwparser.git

cd csfwparser

dotnet pack ./src/fwparser.sln --nologo -c Release --output ./nupkgs

dotnet tool install --global --add-source ./nupkgs avgra3.fwparser.cli
```
## Issues/Bugs

If you find any issues while using this library, feel free to open an issue detailing the issue. Otherwise, clone this repository, fix the bug and then make a pull request with your changes.

## TODOs

- [x] Add toml support. Where we can use a toml file, that will be loaded into the library and we can proceed from there.
- [x] Add support to directly write to a file with a specified delimiter.
- [x] Make an easy to use CLI with output parameters.
