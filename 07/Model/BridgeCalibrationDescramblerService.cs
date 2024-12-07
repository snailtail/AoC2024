using System.Text.RegularExpressions;

namespace _07.Model;

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