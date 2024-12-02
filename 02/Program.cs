var unusualData = File.ReadLines("../data/02test.dat").ToArray().Select(l => l.Split(' ').Select(i => int.Parse(i)).ToArray()).ToArray();



foreach(var entry in unusualData)
{
    //check "direction"
    bool isIncreasing = false;
    isIncreasing = entry[1] > entry[0];
    var result = isValidStep1(entry,isIncreasing);
}



bool isValidStep1(int[] arr, bool isIncreasing)
{
    
    for(int i = 0; i < arr.Length-1; i++)
    {
        if((isIncreasing && arr[i+1] < arr[i]) || (!isIncreasing && arr[i+1] > arr[i]))
        {
            // 
            return false;
        }
    }
}
Console.WriteLine("");

