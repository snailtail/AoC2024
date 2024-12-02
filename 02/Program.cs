unusualData[] uData = File.ReadLines("02.dat").ToArray().Select(l => l.Split(' ').Select(i => int.Parse(i)).ToArray()).Select(arr => new unusualData(arr)).ToArray();


int step1ValidCount = 0;
int step2ValidCount = 0;
foreach(var entry in uData)
{    
    step1ValidCount += entry.IsValid ? 1 : 0;
    step2ValidCount += entry.IsAlmostValid ? 1 : 0;
}

Console.WriteLine($"Step 1: {step1ValidCount}");
Console.WriteLine($"Step 2: {step2ValidCount}");

