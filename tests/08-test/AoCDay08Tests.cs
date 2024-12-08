namespace _08_test;

public class AntennaMap
{
    private readonly char[][] _mapData;
    public Dictionary<char, List<(int,int)>> FrequencyMap { get; } = new(); // Char = [(rowX,colY),(rowX,colY)]
    private Dictionary<char, List<((int, int), (int, int))>> FrequencyWithCoordinatePairs { get; } // Char = [(Coordinate1, Coordinate2),(Coordinate1, Coordinate2)] 
    public AntennaMap(string inputFile)
    {
        var input = File.ReadAllLines(inputFile);
        _mapData = input.Select(line => line.ToCharArray()).ToArray();
        
        // get all unique frequencies (anything that is not a '.' in the map data)
        var uniqueChars = string.Join('.',input).ToCharArray().Where(c => c!= '.').Distinct().ToArray();
        
        // extract all coordinates (rowX, colY) for each frequency
        foreach (var freqChar in uniqueChars)
        {
            // Scan each line for this char, and get its index.
            List<(int, int)> coordinates = new();
            for (int r = 0; r < input.Length; r++)
            {
                var foundCoordinates = input[r].Select((ch, index) => (ch, index))
                    .Where(pair => pair.ch == freqChar)
                    .Select(pair => (r,pair.index))        // Extrahera index
                    .ToList();
                coordinates.AddRange(foundCoordinates);
            }

            if (!FrequencyMap.TryAdd(freqChar, coordinates))
            {
                FrequencyMap[freqChar].AddRange(coordinates);
            }
        }

        // get all coordinate pairs for each frequency
        FrequencyWithCoordinatePairs = FrequencyMap.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value
                    .SelectMany(_ => kvp.Value, (coord1, coord2) => (coord1, coord2))
                    .Where(pair => pair.Item1 != pair.Item2) // Undvik att skapa par med samma koordinat
                    .ToList()
            );
        Console.WriteLine($"Found {FrequencyMap.Count} frequency coordinates");
    }

    public List<(int, int)> AntiNodes => GetInvertedPoints(FrequencyWithCoordinatePairs).ToList();
    private HashSet<(int, int)> GetInvertedPoints(
        Dictionary<char, List<((int, int), (int, int))>> coordinatePairs)
    {
        var uniquePoints = new HashSet<(int, int)>();

        foreach (var kvp in coordinatePairs)
        {
            var pairs = kvp.Value; // Listan med koordinatpar

            foreach (var pair in pairs)
            {
                var (first, second) = pair; // split the coordinate pair up
                var (row1, col1) = first;   // First coordinate
                var (row2, col2) = second; // Second coordinate

                // Calculate the new coordinate for where the AntiNode would be placed.
                int newRow = 2 * row1 - row2;
                int newCol = 2 * col1 - col2;
                
                // Filter out points that would be outside our map/grid
                if (newRow < 0 || newRow >= _mapData.Length || newCol < 0 || newCol >= _mapData[0].Length)
                {
                    continue;
                }

                // Add this coordinate to the HashSet
                uniquePoints.Add((newRow, newCol));
            }
        }

        return uniquePoints;
    }
    
}

public class MapTests
{
    [Fact]
    public void Part1_Find_All_Unique_Chars()
    {
        var map = new AntennaMap("08test.dat");
        var frequencyKeys = map.FrequencyMap.Keys.ToArray();
        Array.Sort(frequencyKeys);
        Assert.Equal(2, frequencyKeys.Length);
        Assert.Equal('0',frequencyKeys[0]);
        Assert.Equal('A',frequencyKeys[1]);
    }
    
    [Theory]
    [InlineData('0',4)]
    [InlineData('A',3)]
    public void Part1_Check_Antenna_Count_By_Frequency(char frequency, int expectedCount)
    {
        var map = new AntennaMap("08test.dat");
        var count = map.FrequencyMap[frequency].Count;
        Assert.Equal(expectedCount,count);
    }
    
    [Fact]
    public void Part1_Check_Coordinates_For_Frequency0()
    {
        var map = new AntennaMap("08test.dat");
        var coordinates = map.FrequencyMap['0'];
        var expectedCoordinates = new List<(int, int)>() { (1, 8), (2, 5), (3, 7), (4, 4) };
        Assert.Equal(expectedCoordinates,coordinates);
    }
    
    [Fact]
    public void Part1_Check_Coordinates_For_FrequencyA()
    {
        var map = new AntennaMap("08test.dat");
        var coordinates = map.FrequencyMap['A'];
        var expectedCoordinates = new List<(int, int)>() { (5, 6), (8, 8), (9, 9) };
        Assert.Equal(expectedCoordinates,coordinates);
    }
    
    [Fact]
    public void Part1_Check_AntiNodeCount()
    {
        var map = new AntennaMap("08test.dat");
        var antiNodeCount = map.AntiNodes.Count;
        var expectedAntiNodeCount = 14;
        Assert.Equal(expectedAntiNodeCount,antiNodeCount);
    }
}

public class AoCDay08Tests
{
    [Theory()]
    [InlineData("08test.dat",14)]
    [InlineData("08.dat",252)]
    public void Part1(string fileName, int expectedCount)
    {
        var map = new AntennaMap(fileName);
        var result = map.AntiNodes.Count;
        Assert.Equal(expectedCount,result);
    }
}