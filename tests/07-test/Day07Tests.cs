using System.Text.RegularExpressions;

namespace _07_test;

public static class BridgeCalibrationDescramblerService
{
    public static long Part1(string[] input)
    {
        long sumOfTestValues = 0;
        foreach (string equation in input)
        {
            (long testValue, List<long> numbers) = ParseEquation(equation);
            if(IsValidCalibration(testValue, numbers.ToArray(),false))
            {
                sumOfTestValues+=testValue;
            }
        }
        return sumOfTestValues;
    }
    
    public static long Part2(string[] input)
    {
        long sumOfTestValues = 0;
        foreach (string equation in input)
        {
            (long testValue, List<long> numbers) = ParseEquation(equation);
            if(IsValidCalibration(testValue, numbers.ToArray(),true))
            {
                sumOfTestValues+=testValue;
            }
        }
        return sumOfTestValues;
    }
    public static bool IsValidCalibration(long testValue, long[] numbers, bool allowConcatenation)
    {
        
        return TryFindCombination(numbers.ToArray(),testValue, allowConcatenation);
    }

    public static (long testValue, List<long> numbers) ParseEquation(string input)
    {
        var inputParts = input.Split(": ");
        var numbers = new List<long>();
        long testValue = long.Parse(inputParts[0]);
        string pattern = @"(\d+)";
        var matches = Regex.Matches(inputParts[1], pattern);
        foreach (Match match in matches)
        {
            numbers.Add(long.Parse(match.Groups[1].Value));
        }
        return (testValue, numbers);
        
    }

    private static long Concatenate(long val1, long val2)
    {
        return long.Parse($"{val1}{val2}");
    }
    
    static bool TryFindCombination(long[] inputs, long target, bool allowConcatenation = false)
    {
        return TryFindCombinationRecursive(inputs, target, 0, inputs[0], allowConcatenation);
    }

    static bool TryFindCombinationRecursive(long[] inputs, long target, int index, long currentResult, bool allowConcatenation)
    {
        // Have we reached the end of inputs?
        if (index == inputs.Length - 1)
        {
            // Do we have a match for our target?
            if (currentResult == target)
            {
                return true;
            }
            return false;
        }

        // Move to next index
        int nextIndex = index + 1;
        long nextValue = inputs[nextIndex];

        // Try addition
        if (TryFindCombinationRecursive(inputs, target, nextIndex, currentResult + nextValue, allowConcatenation))
        {
            return true;
        }

        // Try multiplication
        if (TryFindCombinationRecursive(inputs, target, nextIndex, currentResult * nextValue, allowConcatenation))
        {
            return true;
        }

        if (allowConcatenation)
        {
            // Try concatenation
            if (TryFindCombinationRecursive(inputs, target, nextIndex, Concatenate(currentResult, nextValue), allowConcatenation))
            {
                return true;
            }
        }

        return false; // No combinations were valid
    }
    
}
public class Day07Tests
{

    private string[] testInput = [
        "190: 10 19",
        "3267: 81 40 27",
        "83: 17 5",
        "156: 15 6",
        "7290: 6 8 6 15",
        "161011: 16 10 13",
        "192: 17 8 14",
        "21037: 9 7 18 13",
        "292: 11 6 16 20",
        ];
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
        Assert.Equal(expectedStatus, isValid);
    }

    [Fact]
    public void Part1_Check_Final_Result_Is_3749()
    {
        
        var result = BridgeCalibrationDescramblerService.Part1(testInput);
        Assert.Equal(3749,result);
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
        Assert.Equal(expectedStatus, isValid);
    }
    
    [Fact]
    public void Part2_Check_Final_Result_Is_11387()
    {
        var result = BridgeCalibrationDescramblerService.Part2(testInput);
        Assert.Equal(11387,result);
    }
}