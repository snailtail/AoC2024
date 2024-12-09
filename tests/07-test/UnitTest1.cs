using _07.Model;
using Shouldly;

namespace _07_test;

public class UnitTest1
{
    [Theory]
    [InlineData("190: 10 19",true)]
    [InlineData("3267: 81 40 27", true)]
    [InlineData("83: 17 5", false)]
    [InlineData("292: 11 6 16 20", true)]
    [InlineData("156: 15 6", false)]
    [InlineData("7290: 6 8 6 15", false)]
    [InlineData("161011: 16 10 13", false)]
    [InlineData("192: 17 8 14", false)]
    [InlineData("21037: 9 7 18 13", false)]
    public void Part1_Check_IsValidCalibration(string input, bool expectedStatus)
    {
        (long testValue, List<long> numbers) = BridgeCalibrationDescramblerService.ParseEquation(input);
        bool isValid = BridgeCalibrationDescramblerService.IsValidCalibration(testValue, numbers.ToArray(),false);
        isValid.ShouldBe(expectedStatus);
    }

    [Fact]
    public void Part1_Check_Final_Result_Is_3749()
    {
        var testinput = File.ReadAllLines("07test.dat");
        var result = BridgeCalibrationDescramblerService.Part1(testinput);
        result.ShouldBe(3749);
    }
    
    
    [Theory]
    [InlineData("190: 10 19",true)]
    [InlineData("3267: 81 40 27", true)]
    [InlineData("83: 17 5", false)]
    [InlineData("292: 11 6 16 20", true)]
    [InlineData("156: 15 6", true)]
    [InlineData("7290: 6 8 6 15", true)]
    [InlineData("161011: 16 10 13", false)]
    [InlineData("192: 17 8 14", true)]
    [InlineData("21037: 9 7 18 13", false)]
    public void Part2_Check_IsValidCalibration(string input, bool expectedStatus)
    {
        (long testValue, List<long> numbers) = BridgeCalibrationDescramblerService.ParseEquation(input);
        bool isValid = BridgeCalibrationDescramblerService.IsValidCalibration(testValue, numbers.ToArray(),true);
        isValid.ShouldBe(expectedStatus);
    }
    
    [Fact]
    public void Part2_Check_Final_Result_Is_11387()
    {
        var testinput = File.ReadAllLines("07test.dat");
        var result = BridgeCalibrationDescramblerService.Part2(testinput);
        result.ShouldBe(11387);
    }
}