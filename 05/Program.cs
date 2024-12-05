using _05.Model;

var path = "05.dat";
var rules = File.ReadAllLines(path).Where(l => l.Contains("|")).ToList();
var updates = File.ReadAllLines(path).Where(l => l.Contains(",")).ToList();
int resultPart1 = 0;
int resultPart2 = 0;

foreach (var update in updates)
{
    if (PrintQueueHandler.IsValidUpdate(update, rules))
    {
        resultPart1 += PrintQueueHandler.GetMiddleItem(update);
    }
    else
    {
        resultPart2 += PrintQueueHandler.GetFixedUpdateMiddleItem(update,rules);
    }
}

Console.WriteLine($"Part 1: {resultPart1}");
Console.WriteLine($"Part 2: {resultPart2}");

