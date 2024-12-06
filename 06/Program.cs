var mapdata = File.ReadAllLines("06.dat").Select(line => line.ToCharArray()).ToArray();

var resultPart1 = SuitManufacturingLabMapHandler.walkRegularPath(mapdata).Count;
Console.WriteLine($"Part 1: {resultPart1}");

var resultPart2 = SuitManufacturingLabMapHandler.GetCombinationsOfPossibleLoops(mapdata);
Console.WriteLine($"Part 2: {resultPart2}");