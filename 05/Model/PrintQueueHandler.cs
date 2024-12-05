namespace _05.Model;

public static class PrintQueueHandler
{
    public static bool IsValidUpdate(string update, List<string> rules)
    {
        foreach (var rule in rules)
        {
            var ruleParts = rule.Split("|");
            var rule1Index = update.IndexOf(ruleParts[0], StringComparison.Ordinal);
            var rule2Index = update.IndexOf(ruleParts[1], StringComparison.Ordinal);
            if (rule1Index != -1 && rule2Index != -1 && rule1Index > rule2Index)
            {
                return false;
            }
        }
        return true;
    }
    
    public static int GetMiddleItem(string update)
    {
        var parts = update.Split(",");
        int middleItem = parts.Length / 2;
        return int.Parse(parts[middleItem]);
    }
    
    public static int GetFixedUpdateMiddleItem(string badUpdate, List<string> rules)
    {
        var relevantRules = GetRelevantRules(badUpdate, rules);
        while (IsValidUpdate(badUpdate, relevantRules) == false)
        {
            badUpdate = SortBadUpdate(badUpdate, relevantRules);
        }
        
        return GetMiddleItem(badUpdate);
    }

    private static string SortBadUpdate(string badUpdate, List<string> relevantRules)
    {
        int[] updateNums = badUpdate.Split(",").Select(int.Parse).ToArray();
        
        foreach (var rule in relevantRules)
        {
            var ruleParts = rule.Split("|").Select(int.Parse).ToArray(); 
            var rule1Index = Array.IndexOf(updateNums,ruleParts[0]);
            var rule2Index = Array.IndexOf(updateNums,ruleParts[1]);
            if (rule2Index != -1 && rule1Index != -1 && rule2Index < rule1Index)
            {
                updateNums = EnforceRule(updateNums, rule1Index, rule2Index);
            }
        }
        
        return string.Join(",", updateNums);
    }
    
    static int[] EnforceRule(int[] nums, int rule1Index, int rule2Index)
    {
        List<int> numsList = [..nums];
        
        int valueToMove = nums[rule1Index];
        numsList.RemoveAt(rule1Index);
        numsList.Insert(rule2Index, valueToMove);
        
        return numsList.ToArray();
    }

    private static List<string> GetRelevantRules(string update, List<string> rules)
    {
        List<string> relevantRules = new();
        foreach (var rule in rules)
        {
            var ruleParts = rule.Split("|");
            var rule1Index = update.IndexOf(ruleParts[0], StringComparison.Ordinal);
            var rule2Index = update.IndexOf(ruleParts[1], StringComparison.Ordinal);
            if (rule1Index != -1 && rule2Index != -1)
            {
                relevantRules.Add(rule);
            }
        }
        return relevantRules;
    }
}