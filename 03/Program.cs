using Console = System.Console;
using System.Text.RegularExpressions;

var inputs = File.ReadAllLines("03.dat");

Console.WriteLine("Hello, World!");
string pattern =@"mul\((\d+),(\d+)\)";
Regex rg = new Regex(pattern);
int result1 = 0;
foreach (var input in inputs)
{
    var matches = rg.Matches(input);
    foreach (Match match in matches)
    {
        int value1 = int.Parse(match.Groups[1].Value);
        int value2 = int.Parse(match.Groups[2].Value);
        result1+=value1*value2;
    }
}
Console.WriteLine($"Part 1: {result1}");

// Part 2:
string part2Input = File.ReadAllText("03.dat");
string blockedPattern=@"don't\(\)(.*?)do\(\)";

var blockMatches = Regex.Matches(part2Input, blockedPattern);
string result = Regex.Replace(part2Input, blockedPattern, "", RegexOptions.Singleline);

var matchesPart2 = rg.Matches(result);
int result2 = 0;
foreach (Match match in matchesPart2)
{
    int value1 = int.Parse(match.Groups[1].Value);
    int value2 = int.Parse(match.Groups[2].Value);
    result2+=value1*value2;
}
Console.WriteLine($"Step 2: {result2}");