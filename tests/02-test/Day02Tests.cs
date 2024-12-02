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
    
}