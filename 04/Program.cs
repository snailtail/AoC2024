using _04.Model;

var testInput = File.ReadAllLines("04.dat");
var mySearcher = new WordSearcher(testInput);
int result = mySearcher.SolveStep1();
Console.WriteLine(result);