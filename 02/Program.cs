unusualData[] uData = File.ReadLines("02.dat").ToArray().Select(l => l.Split(' ').Select(i => int.Parse(i)).ToArray()).Select(arr => new unusualData(arr)).ToArray();


int validDataCount = 0;
int step2ValidCount = 0;
foreach(var entry in uData)
{    
    validDataCount += entry.IsValid ? 1 : 0;
}

Console.WriteLine($"Step 1: {validDataCount}");
Console.WriteLine($"Step 2: {step2ValidCount}");

