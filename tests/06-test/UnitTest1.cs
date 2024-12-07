using Shouldly;

namespace _06_test;

public class UnitTest1
{
    [Fact]
    public void Part1()
    {
        var mapdata = File.ReadAllLines("06test.dat").Select(line => line.ToCharArray()).ToArray();
        var result = SuitManufacturingLabMapHandler.walkRegularPath(mapdata);
        result.Count.ShouldBe(41);
    }
}