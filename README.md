---
title: CSFWParser
data: January 25, 2025
---
# CSFWParser

A simple library to help you parse your fixed width files, written in pure C#.

You might be wondering why the name. Simple: I have a [Python version](https://github.com/avgra3/fwparser/) with a similar name. Go check it out if you're interested.

## Example
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

Dictionary<string, int[]> tomlConfig = Utilities.TomlFileToDicionary(
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

## Motivation

You might be wondering why this library exists. I found that working width fixed width files was a hassle each time I worked with a new one. To be clear, they still are but at least I have an easy tool that I can use to work with them and I can more easily convert them to CSV files (or other delimited file types).

Personally, I like using a TOML file for configuration of the different fields then using that in place of the data outline dictionary I use in the example.

## Dependencies

- .NET 8.0 (although other versions would probably work, but are not tested here).

## Issues/Bugs

If you find any issues while using this library, feel free to open an issue or pull request.

## TODOs

- [x] Add toml support. Where we can use a toml file, that will be loaded into the library and we can proceed from there.
- [x] Add support to directly write to a file with a specified delimiter.

