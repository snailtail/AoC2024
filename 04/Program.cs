using _04.Model;

var testInput = File.ReadAllLines("04.dat");
var mySearcher = new WordSearcher(testInput);

int resultPart1 = mySearcher.SolvePart1();
Console.WriteLine($"Part 1: {resultPart1}");

int resultPart2 = mySearcher.SolvePart2();
Console.WriteLine($"Part 2: {resultPart2}");
