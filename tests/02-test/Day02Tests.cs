namespace _02_test;
using Shouldly;
public class Day02Tests
{
    [Theory]
    [InlineData(new int[] {7,6,4,2,1},true)]
    [InlineData(new int[] {1,2,7,8,9},false)]
    [InlineData(new int[] {9,7,6,2,1},false)]
    [InlineData(new int[] {1,3,2,4,5},false)]
    [InlineData(new int[] {8,6,4,4,1},false)]
    [InlineData(new int[] {1,3,6,7,9},true)]
    public void TestStep1ExampleData(int[] arr, bool ExpectedResult)
    {
        // Set up
        var uData = new unusualData(arr);

        // Execute
        var result = uData.IsValid;

        // Validate
        result.ShouldBe(ExpectedResult);
    }


    [Theory]
    [InlineData(new int[] {7,6,4,2,1},true)]
    [InlineData(new int[] {1,2,7,8,9},false)]
    [InlineData(new int[] {9,7,6,2,1},false)]
    [InlineData(new int[] {1,3,2,4,5},true)]
    [InlineData(new int[] {8,6,4,4,1},true)]
    [InlineData(new int[] {1,3,6,7,9},true)]
    public void TestStep2ExampleData(int[] arr, bool ExpectedResult)
    {
         // Set up
        var uData = new unusualData(arr);

        // Execute
        var result = uData.IsAlmostValid;

         // Validate
        result.ShouldBe(ExpectedResult);
    }

    [Theory]
    [InlineData(new int[] {20,22,25,27,28,29,33},true)]
    public void TestExtendedSampleData(int[] arr, bool ExpectedResult)
    {
         // Set up
        var uData = new unusualData(arr);

        // Execute
        var result = uData.IsAlmostValid;

         // Validate
        result.ShouldBe(ExpectedResult);
    }
    
    [Theory]
    [InlineData(new int[] {48, 46, 47, 49, 51, 54, 56})]
    [InlineData(new int[] {1, 1, 2, 3, 4, 5})]
    [InlineData(new int[] {1, 2, 3, 4, 5, 5})]
    [InlineData(new int[] {5, 1, 2, 3, 4, 5})]
    [InlineData(new int[] {1, 4, 3, 2, 1})]
    [InlineData(new int[] {1, 6, 7, 8, 9})]
    [InlineData(new int[] {1, 2, 3, 4, 3})]
    [InlineData(new int[] {9, 8, 7, 6, 7})]
    [InlineData(new int[] {7, 10, 8, 10, 11})]
    [InlineData(new int[] {29, 28, 27, 25, 26, 25, 22, 20})]
    public void TestEdgeCasesStep2_AllShouldBeValid(int[] arr)
    {
         // Set up
        var uData = new unusualData(arr);

        // Execute
        var result = uData.IsAlmostValid;

        // Validate
        result.ShouldBeTrue();
    }
}