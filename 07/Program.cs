using _07.Model;
var input = File.ReadAllLines("07.dat");
var resultPart1 = BridgeCalibrationDescramblerService.Part1(input);
Console.WriteLine($"Part 1 : {resultPart1}");

var resultPart2 = BridgeCalibrationDescramblerService.Part2(input);
Console.WriteLine($"Part 2 : {resultPart2}");
