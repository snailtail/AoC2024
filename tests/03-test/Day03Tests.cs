using System.Text.RegularExpressions;

namespace _03_test;

public class Day03
{

    public int Part1(string[] input)
    {
        var pattern = @"mul\((\d+),(\d+)\)";
        Regex rg = new Regex(pattern);
        var result1 = 0;
        foreach (var line in input)
        {
            var matches = rg.Matches(line);
            foreach (Match match in matches)
            {
                var value1 = int.Parse(match.Groups[1].Value);
                var value2 = int.Parse(match.Groups[2].Value);
                result1 += value1 * value2;
            }

        }
        return result1;
    }

    public int Part2(string input)
    {
        var pattern2 = @"don't\(\)|mul\((\d+),(\d+)\)|do\(\)";
        var machesPart2Attempt2 = Regex.Matches(input, pattern2, RegexOptions.Singleline);
        bool Active = true;
        var result2 = 0;
        foreach (Match match in machesPart2Attempt2)
        {
            if (match.Value == "don't()")
            {
                Active = false;
            }
            else if (match.Value == "do()")
            {
                Active = true;
            }
            else if (Active)
            {
                //Console.WriteLine(match.Value);
                var value1 = int.Parse(match.Groups[1].Value);
                var value2 = int.Parse(match.Groups[2].Value);
                result2 += value1 * value2;
            }
        }
        return result2;
    }
}

public class Day03Tests
{
    [Fact]
    public void TestPart1()
    {
        string[] input = [
            "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))"
            ];
        var day = new Day03();
        var result = day.Part1(input);
        Assert.Equal(161, result);
    }

    [Fact]
    public void TestPart2()
    {
        string input = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";
        var day = new Day03();
        var result = day.Part2(input);
        Assert.Equal(48, result);
    }
}