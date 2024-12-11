using System.Diagnostics;

namespace _11_test;

public static class Day11Extensions
{
    public static List<string> Blink(this List<string> stones)
    {
        List<string> newList = [];
        foreach (string stone in stones)
        {
            var processedStones = stone.ProcessRules();
            foreach (var ps in processedStones)
            {
                newList.Add(ps);
            }
        }
        
        return newList;
    }

    public static string[] ProcessRules(this string stone)
    {
        // stone number equal to 0 becomes a 1        
        if (stone == "0")
            return ["1"];

        // stone number length is even - split into two stones
        if (stone.Length % 2 == 0)
        {
            var s1 = stone[..(stone.Length / 2)].ToString();
            var s2 = long.Parse(stone[((stone.Length / 2)) ..].ToString()).ToString();
            
            return [s1, s2];
        }
        
        // else we multiply the stones number by 2024
        long stoneValue = long.Parse(stone);
        long updatedValue = stoneValue * 2024;
        return [$"{updatedValue}"];

    }
}


public class Day11Tests
{

    [Theory]
    [InlineData("10", new string[]{"1","0"})]
    [InlineData("0", new string[]{"1"})]
    [InlineData("1234", new string[]{"12","34"})]
    [InlineData("1004", new string[]{"10","4"})]
    [InlineData("1", new string[]{"2024"})]
    [InlineData("125", new string[]{"253000"})]
    public void Test_Transform_Stone(string stone, string[] expectedStones)
    {
        var result = stone.ProcessRules();
        Assert.Equal(expectedStones, result);
    }
    [Fact]
    public void TestPart1BlinkOnce()
    {
        List<string> data = ["125","17"];
        List<string> expectedData = ["253000", "1", "7"]; 
        var result = data.Blink();
        
        Assert.Equal(expectedData, result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void Part1BlinkSixTimes()
    {
        List<string> data = ["125", "17"];
        List<string> expectedResult =
        [
            "2097446912", "14168", "4048", "2", "0", "2", "4", "40", "48", "2024", "40", "48", "80", "96", "2", "8",
            "6", "7", "6", "0", "3", "2"
        ];
        for (int i = 0; i < 6; i++)
        {
            data = data.Blink();
        }
        Assert.Equal(expectedResult, data);
        

    }
    
    [Fact]
    public void Part1Prod()
    {
        List<string> data = File.ReadAllText("11.dat").Split(" ").ToList();
        
        for (int i = 0; i < 25; i++)
        {
            data = data.Blink();
        }
        Assert.Equal(193899, data.Count);
    }
    
    [Fact]
    public void Part2Prod()
    {
        List<string> data = File.ReadAllText("11.dat").Split(" ").ToList();
        
        for (int i = 0; i < 75; i++)
        {
            data = data.Blink();
        }
        Assert.Equal(55312, data.Count);
    }
}