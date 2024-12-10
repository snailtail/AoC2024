using _04.Model;


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
        Assert.Equal(_testInput.Length, mySearcher.CharGrid.Length);
    }
    [Fact]
    public void Part1_TestInput_ResultShouldBeEqualTo18()
    {
        
        var mySearcher = new WordSearcher(_testInput);
        int result = mySearcher.SolvePart1();
        Assert.Equal(18, result);
    }

    [Fact]
    public void Part2_TestInput_ResultShouldBeEqualTo9()
    {
        var mySearcher = new WordSearcher(_testInput);
        int result = mySearcher.SolvePart2();
        Assert.Equal(9,result);
    }
}