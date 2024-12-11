namespace _02_test;

public class unusualData
{
    private int[] _data;
    public int[] Data
    {
        get { return _data; }
        set { _data = value; }
    }

    public unusualData(int[] data)
    {
        _data = data;
    }

    

    public bool IsValid => isValid(_data,false);
    public bool IsAlmostValid => isValid(_data,true);

    private bool isValid(int[] arr, bool allowErrors = false)
    {    
        bool isIncreasing = arr[1] > arr[0];
        for(int i = 0; i < arr.Length-1; i++)
        {

            // Check if increasing or decreasing breaks
            if(
                (isIncreasing && arr[i+1] < arr[i]) 
                || 
                (!isIncreasing && arr[i+1] > arr[i]) 
                || 
                arr[i+1] == arr[i]
                ||
                (Math.Abs(arr[i+1] - arr[i]) < 1 || Math.Abs(arr[i+1] - arr[i]) > 3)
            )
            {
                // so we have an error
                // for step 2 - we can check if this array would be valid if we remove any value
                if(allowErrors){
                    for(int ii = 0; ii < arr.Length; ii++)
                    {
                        int[] newArr = arr.Where((_, index) => index != ii).ToArray();
                        bool isAlmostValid = isValid(newArr,false);
                        if(isAlmostValid)
                        {
                            return true;
                        }
                    }

                }
                return false;
            }

            
        }

        return true;
    }    
}
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
        Assert.Equal(ExpectedResult, result);
        
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
         Assert.Equal(ExpectedResult, result);
        
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
         Assert.Equal(ExpectedResult, result);
        
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
        Assert.True(result);
        
    }

    private string[] testInput = [
                    "7 6 4 2 1",
                    "1 2 7 8 9",
                    "9 7 6 2 1",
                    "1 3 2 4 5",
                    "8 6 4 4 1",
                    "1 3 6 7 9",
                    ];

    [Fact]
    public void Part1()
    {
        unusualData[] uData = testInput.ToArray().Select(l => l.Split(' ').Select(i => int.Parse(i)).ToArray()).Select(arr => new unusualData(arr)).ToArray();
        int step1ValidCount = 0;
        foreach(var entry in uData)
        {    
            step1ValidCount += entry.IsValid ? 1 : 0;
        }
        Assert.Equal(2,step1ValidCount);
    }
    
    [Fact]
    public void Part2()
    {
        unusualData[] uData = testInput.ToArray().Select(l => l.Split(' ').Select(i => int.Parse(i)).ToArray()).Select(arr => new unusualData(arr)).ToArray();
        
        int step2ValidCount = 0;
        foreach(var entry in uData)
        {    
            step2ValidCount += entry.IsAlmostValid ? 1 : 0;
        }
        Assert.Equal(4,step2ValidCount);
    }
}