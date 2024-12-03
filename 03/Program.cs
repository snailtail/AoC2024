using Console = System.Console;
using System.Text.RegularExpressions;
var filePath = "03.dat";

// Part 1:
var part1Input = File.ReadAllLines(filePath);
var pattern =@"mul\((\d+),(\d+)\)";
Regex rg = new Regex(pattern);
var result1 = 0;
foreach (var input in part1Input)
{
    var matches = rg.Matches(input);
    foreach (Match match in matches)
    {
        var value1 = int.Parse(match.Groups[1].Value);
        var value2 = int.Parse(match.Groups[2].Value);
        result1+=value1*value2;
    }
}
Console.WriteLine($"Part 1: {result1}");

// Part 2:
var part2Input = File.ReadAllText(filePath);
var blockedPattern=@"don't\(\)(.*?)do\(\)";
var result = Regex.Replace(part2Input, blockedPattern, "", RegexOptions.Singleline);

var matchesPart2 = rg.Matches(result);
var result2 = 0;
foreach (Match match in matchesPart2)
{
    var value1 = int.Parse(match.Groups[1].Value);
    var value2 = int.Parse(match.Groups[2].Value);
    result2+=value1*value2;
}
Console.WriteLine($"Part 2: {result2}");