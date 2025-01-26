using fwparser.lib;

// Showcasing what the fwparser library can do!
// Assuming foo.txt contains the below
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
        trimWhiteSpace: true, // Defaults to false
        offset: 1 // Only needed if your config does not have an index of zero
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

