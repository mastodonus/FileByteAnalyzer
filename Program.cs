using System.Text;

const int sample = 50;

if (string.IsNullOrEmpty(args[0]))
{
    Error("No file was provided.");
    return;
}

var path = Path.Combine(args[0]);
if (!File.Exists(path))
{
    Error($"Unable to find file: {path}");
    return;
}

var offset = 0;
if (args.Length >= 2)
{
    if (int.TryParse(args[1], out var result))
    {
        offset = result;
    }
    else
    {
        Error($"Offset is not a valid int: {args[1]}");
        return;
    }
}

var bytes = File.ReadAllBytes(path);
var eof = Math.Min(offset + sample, bytes.Length);

Console.WriteLine($"Writing bytes {offset} to {eof}...");
var subsetBytes = bytes[offset..Math.Min(offset + sample, bytes.Length)];

Output("Base64", Convert.ToBase64String(subsetBytes));

var sb = new StringBuilder();
for (int i = 0; i < subsetBytes.Length; i++)
{
    sb.Append(subsetBytes[i].ToString("X2") + " ");
}

Output("Hex", sb.ToString());
Output("Utf8", Encoding.UTF8.GetString(subsetBytes));


void Output(string label, string value)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write(label.PadRight(10));
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write(value);
    Console.WriteLine();
}

void Error(string msg)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"ERROR: {msg}");
}