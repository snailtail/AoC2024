using System.Xml.XPath;

var path = "05.dat";
var rules = File.ReadAllLines(path).Where(l => l.Contains("|")).ToList();
var updates = File.ReadAllLines(path).Where(l => l.Contains(",")).ToList();
int resultPart1 = 0;
int resultPart2 = 0;
foreach (var update in updates)
{
    if (isValidUpdate(update, rules))
    {
        resultPart1 += getMiddleItem(update);
    }
    else
    {
        resultPart2 += getFixedUpdateMiddleItem(update);
    }
}

Console.WriteLine($"Part 1: {resultPart1}");
Console.WriteLine($"Part 2: {resultPart2}");

bool isValidUpdate(string update, List<string> rules)
{
    foreach (var rule in rules)
    {
        var ruleParts = rule.Split("|");
        var rule1Index = update.IndexOf(ruleParts[0]);
        var rule2Index = update.IndexOf(ruleParts[1]);
        if (rule1Index != -1 && rule2Index != -1 && rule1Index > rule2Index)
        {
            return false;
        }
    }
    return true;
}

int getMiddleItem(string update)
{
    var parts = update.Split(",");
    int middleItem = parts.Length / 2;
    return int.Parse(parts[middleItem]);
}

int getFixedUpdateMiddleItem(string s)
{
    throw new NotImplementedException();
}