using _04.Model;
using Shouldly;

namespace _04_test;

public class UnitTest1
{
    string[] _testInput = @"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX".Split().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();


    [Fact]
    public void Part1_Check_Size_Of_CharGrid()
    {
        var mySearcher = new WordSearcher(_testInput);
        mySearcher.CharGrid.Length.ShouldBe(_testInput.Length);
    }
    [Fact]
    public void Part1_TestInput_ResultShouldBeEqualTo18()
    {
        
        var mySearcher = new WordSearcher(_testInput);
        int result = mySearcher.SolvePart1();
        result.ShouldBe(18);
    }

    [Fact]
    public void Part2_TestInput_ResultShouldBeEqualTo9()
    {
        var mySearcher = new WordSearcher(_testInput);
        int result = mySearcher.SolvePart2();
        result.ShouldBe(9);
    }
}