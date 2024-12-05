namespace _05.Model;

public static class PrintQueueHandler
{
    public static bool isValidUpdate(string update, List<string> rules)
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
    
    public static int getMiddleItem(string update)
    {
        var parts = update.Split(",");
        int middleItem = parts.Length / 2;
        return int.Parse(parts[middleItem]);
    }
    
    public static int getFixedUpdateMiddleItem(string badUpdate, List<string> rules)
    {
        
        // 75,97,47,61,53 = Bad
        // Rule = 97|75
        // First - find only rules containing two numbers who are in the badUpdate
        var relevantRules = getRelevantRules(badUpdate, rules);
        return -1;
    }
    
    public static List<int[]> getRelevantRules(string update, List<string> rules)
    {
        List<int[]> relevantRules = new List<int[]>();
        for (int i = 0; i < rules.Count; i++)
        {
            var ruleParts = rules[i].Split("|");
            var rule1Index = update.IndexOf(ruleParts[0]);
            var rule2Index = update.IndexOf(ruleParts[1]);
            if (rule1Index != -1 && rule2Index != -1)
            {
                relevantRules.Add(rules[i].Split('|').Select(r=> int.Parse(r)).ToArray());
            }
        }
        return relevantRules;
    }
}