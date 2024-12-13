using System.Text.RegularExpressions;

namespace _13_test;

public static class ClawMachineExtensions
{
    public static long GetMinimumCost(this ClawMachine machine)
    {
        // Cramer's rule: https://en.wikipedia.org/wiki/Cramer%27s_rule
        
        // Determinant: https://en.wikipedia.org/wiki/Determinant
        
        // Calculate the determinant (det) using the button move values
        long determinant = machine.ButtonA.X * machine.ButtonB.Y - machine.ButtonA.Y * machine.ButtonB.X;
    
        // Calculate how many times to press Button A and Button B, using the determinant
        long pressesA = (machine.Prize.X * machine.ButtonB.Y - machine.Prize.Y * machine.ButtonB.X) / determinant;
        long pressesB = (machine.ButtonA.X * machine.Prize.Y - machine.ButtonA.Y * machine.Prize.X) / determinant;
    
        // Check if the calculated presses satisfy the prize position
        if ((machine.ButtonA.X * pressesA + machine.ButtonB.X * pressesB, 
                machine.ButtonA.Y * pressesA + machine.ButtonB.Y * pressesB) 
            == machine.Prize)
        {
            // Calculate the total cost (3 tokens per A press, 1 token per B press)
            return pressesA * 3 + pressesB;
        }
        else
        {
            // Return 0 if no valid solution is found
            return 0;
        }
    }

}

public class ClawMachine
{
    public (long X, long Y) ButtonA;
    public (long X, long Y) ButtonB;
    public (long X, long Y) Prize;
}

public class ClawMachineParser
{
    public static List<ClawMachine> Parse(string[] input, bool Part2 = false)
    {
        long i = 0;
        List<ClawMachine> clawMachines = new();
        var clawMachine = new ClawMachine();
        while (i < input.Length)
        {
            var line = input[i];
            string pattern;
            if (line.StartsWith("Button A:"))
            {
                pattern = @"[XY]\+(\d+)";
                var matches = Regex.Matches(line, pattern);
                clawMachine.ButtonA = (long.Parse(matches[0].Groups[1].Value),long.Parse(matches[1].Groups[1].Value));
            }
            else if (line.StartsWith("Button B:"))
            {
                pattern = @"[XY]\+(\d+)";
                var matches = Regex.Matches(line, pattern);
                clawMachine.ButtonB = (long.Parse(matches[0].Groups[1].Value),long.Parse(matches[1].Groups[1].Value));
            }
            else if (line.StartsWith("Prize:"))
            {
                pattern = @"[XY]=(\d+)";
                var matches = Regex.Matches(line, pattern);
                clawMachine.Prize = (long.Parse(matches[0].Groups[1].Value),long.Parse(matches[1].Groups[1].Value));
                if (Part2)
                {
                    clawMachine.Prize.X += 10_000_000_000_000;
                    clawMachine.Prize.Y += 10_000_000_000_000;;
                }
                clawMachines.Add(clawMachine);
                clawMachine = new ClawMachine();
            }

            i++;
        }
        return clawMachines;
    }
}

public class UnitTest1
{

    private string[] testInput = [
        "Button A: X+94, Y+34",
        "Button B: X+22, Y+67",
        "Prize: X=8400, Y=5400",
        "",
        "Button A: X+26, Y+66",
        "Button B: X+67, Y+21",
        "Prize: X=12748, Y=12176",
        "",
        "Button A: X+17, Y+86",
        "Button B: X+84, Y+37",
        "Prize: X=7870, Y=6450",
        "",
        "Button A: X+69, Y+23",
        "Button B: X+27, Y+71",
        "Prize: X=18641, Y=10279",
    ];
    [Fact]
    public void TestParsingToClawMachines()
    {
        var machines = ClawMachineParser.Parse(testInput);
        Assert.Equal(4,machines.Count);
        Assert.Equal(94,machines[0].ButtonA.X);
        Assert.Equal(34,machines[0].ButtonA.Y);
        Assert.Equal(22,machines[0].ButtonB.X);
        Assert.Equal(67,machines[0].ButtonB.Y);
        Assert.Equal(8400,machines[0].Prize.X);
        Assert.Equal(5400,machines[0].Prize.Y);
        
        Assert.Equal(69,machines[3].ButtonA.X);
        Assert.Equal(23,machines[3].ButtonA.Y);
        Assert.Equal(27,machines[3].ButtonB.X);
        Assert.Equal(71,machines[3].ButtonB.Y);
        Assert.Equal(18641,machines[3].Prize.X);
        Assert.Equal(10279,machines[3].Prize.Y);
    }

    [Fact]
    public void TestMachine0()
    {
        var machines = ClawMachineParser.Parse(testInput);
        var result = machines[0].GetMinimumCost();
        Assert.Equal(280, result);
    }
    
    [Fact]
    public void TestMinimumTokensForAllMachines()
    {
        var machines = ClawMachineParser.Parse(testInput);
        long result = 0;
        foreach (var machine in machines)
        {
            result += machine.GetMinimumCost();
        }
        Assert.Equal(480, result);
    }
}
