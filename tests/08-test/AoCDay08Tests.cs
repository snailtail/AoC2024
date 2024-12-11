namespace _08_test;

public class AntennaMap
{
    private readonly char[][] _mapData;
    public Dictionary<char, List<(int,int)>> FrequencyMap { get; } = new(); // Char = [(rowX,colY),(rowX,colY)]
    private Dictionary<char, List<((int, int), (int, int))>> FrequencyWithCoordinatePairs { get; } // Char = [(Coordinate1, Coordinate2),(Coordinate1, Coordinate2)] 
    public AntennaMap(string[] input)
    {
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
        
        // for part 1 we only find ones that are at a set distance form c1 and c2 - where the distance to c2 has to be twice the distance to c1.
        // for part 2 we need to find all possible points in a straight line from the pair - no matter the distances between ip and c1 and c2. 
        // so this is more to find an angle between the two coordinates and draw each coordinate on that line somehow.
        
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
    
    
    public int GetAntiNodesCount_Part2()
    {
        HashSet<(int, int)> antiNodes = new();

        for (int row = 0; row < _mapData.Length; row++)
        {
            for (int col = 0; col < _mapData[0].Length; col++)
            {
                foreach (var kvp in FrequencyWithCoordinatePairs)
                {
                    foreach (var coord in kvp.Value)
                    {
                        ((int row1, int col1), (int row2, int col2)) = coord;

                        // Create vector from the current point to the first coordinate
                        var checkVector = (row - row1, col - col1);

                        // Pair vector
                        var pairVector = (row2 - row1, col2 - col1);

                        // Are the two vectors parallell? (Are the vectors scalar multiples of each other?)
                        if (checkVector.Item1 * pairVector.Item2 == checkVector.Item2 * pairVector.Item1)
                        {
                            antiNodes.Add((row, col));
                        }
                    }
                }
            }
        }
        return antiNodes.Count;
    }


}

public class MapTests
{

    private string[] testInput = [
        "............",
        "........0...",
        ".....0......",
        ".......0....",
        "....0.......",
        "......A.....",
        "............",
        "............",
        "........A...",
        ".........A..",
        "............",
        "............",
        ];
    [Fact]
    public void Part1_Find_All_Unique_Chars()
    {
        var map = new AntennaMap(testInput);
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
        var map = new AntennaMap(testInput);
        var count = map.FrequencyMap[frequency].Count;
        Assert.Equal(expectedCount,count);
    }
    
    [Fact]
    public void Part1_Check_Coordinates_For_Frequency0()
    {
        var map = new AntennaMap(testInput);
        var coordinates = map.FrequencyMap['0'];
        var expectedCoordinates = new List<(int, int)>() { (1, 8), (2, 5), (3, 7), (4, 4) };
        Assert.Equal(expectedCoordinates,coordinates);
    }
    
    [Fact]
    public void Part1_Check_Coordinates_For_FrequencyA()
    {
        var map = new AntennaMap(testInput);
        var coordinates = map.FrequencyMap['A'];
        var expectedCoordinates = new List<(int, int)>() { (5, 6), (8, 8), (9, 9) };
        Assert.Equal(expectedCoordinates,coordinates);
    }
    
    [Fact]
    public void Part1_Check_AntiNodeCount()
    {
        var map = new AntennaMap(testInput);
        var antiNodeCount = map.AntiNodes.Count;
        var expectedAntiNodeCount = 14;
        Assert.Equal(expectedAntiNodeCount,antiNodeCount);
    }
    
    [Fact()]
    public void Get_AntiNodes_With_Part2_Rules()
    {
        var map = new AntennaMap(testInput);
        var result = map.GetAntiNodesCount_Part2();
        Assert.Equal(34, result);
    }
}

public class AoCDay08Tests
{
    private string[] testInput = [
        "............",
        "........0...",
        ".....0......",
        ".......0....",
        "....0.......",
        "......A.....",
        "............",
        "............",
        "........A...",
        ".........A..",
        "............",
        "............",
        ];
    
    [Fact()]    
    public void Part1()
    {
        var map = new AntennaMap(testInput);
        var result = map.AntiNodes.Count;
        Assert.Equal(14,result);
    }
    
    [Fact()]
    public void Part2()
    {
        var map = new AntennaMap(testInput);
        var result = map.GetAntiNodesCount_Part2();
        Assert.Equal(34, result);
    }
}