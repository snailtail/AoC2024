using System.ComponentModel.DataAnnotations;

var lines = File.ReadAllLines("../data/01.dat");
int[] left = new int[lines.Length];
int[] right = new int[lines.Length];
for(int i = 0; i < lines.Length; i++){
    var x = lines[i].Split("   ");
    left[i] = int.Parse(x[0]);    
    right[i] = int.Parse(x[1]);    
}
Array.Sort(left);
Array.Sort(right);
int Sum_Step1 = 0;
for(int i = 0; i < left.Length; i++)
{
    int thisDistance = Math.Max(left[i],right[i])-Math.Min(left[i],right[i]);
    Sum_Step1 += thisDistance;;
}
Console.WriteLine("Step 1: " + Sum_Step1);

//Step 2
int similarity = 0;

foreach(int leftValue in left)
{
    int appearances = right.Where(r => r == leftValue).Count();
    int thisSimilarity = leftValue * appearances;
    similarity += thisSimilarity;
}

Console.WriteLine("Step 2: " + similarity);