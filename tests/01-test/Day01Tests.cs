namespace _01_test;
public static class Day01
{
    public static (int Part1, int Part2) Solve(string inputFile)
    {
        var lines = File.ReadAllLines(inputFile);
        int[] left = new int[lines.Length];
        int[] right = new int[lines.Length];
        for(int i = 0; i < lines.Length; i++){
            var x = lines[i].Split("   ");
            left[i] = int.Parse(x[0]);    
            right[i] = int.Parse(x[1]);    
        }
        Array.Sort(left);
        Array.Sort(right);
        
        int sumStep1 = 0;
        for(int i = 0; i < left.Length; i++)
        {
            int thisDistance = Math.Max(left[i],right[i])-Math.Min(left[i],right[i]);
            sumStep1 += thisDistance;;
        }
        
        var sumStep2Smarter = right.Where(l => left.Contains(l)).Sum();
        return (sumStep1, sumStep2Smarter);
    }
}
public class Day01Tests
{
    [Fact]
    public void Part1()
    {
        (int result,_) = Day01.Solve("01test.dat");
        Assert.Equal(11,result);
    }
    [Fact]
    public void Part2()
    {
        (_,int result) = Day01.Solve("01test.dat");
        Assert.Equal(13,result);
    }
    
    [Fact]
    public void Part2_Prod()
    {
        (_,int result) = Day01.Solve("01.dat");
        Assert.Equal(24349736,result);
    }
}