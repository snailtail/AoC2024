using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

var lines = File.ReadAllLines("01test.dat");
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
var stopwatch = Stopwatch.StartNew();
long elapsedFirstMethod;
var sumStep2 = left.Select(l => l * (right.Where(r => r == l).Count())).Sum();
stopwatch.Stop();
elapsedFirstMethod = stopwatch.ElapsedTicks;


long elapsedSecondtMethod;
stopwatch = Stopwatch.StartNew();
var sumStep2Smarter = right.Where(l => left.Contains(l)).Sum();
stopwatch.Stop();
elapsedSecondtMethod = stopwatch.ElapsedTicks;

Console.WriteLine("Step 2: " + sumStep2 + " Time: " + elapsedFirstMethod);
Console.WriteLine("Step 2: " + sumStep2Smarter + " Time: " + elapsedSecondtMethod );