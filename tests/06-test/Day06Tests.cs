namespace _06_test;
public static class SuitManufacturingLabMapHandler
{
    public static HashSet<string> walkRegularPath(char[][] mapdata)
    {
        HashSet<string> visitedCoordinates = new HashSet<string>();
        Coordinate guard = new();
        Direction guardDirection = new();
        (guard.Row, guard.Col) = FindGuardPosition(mapdata);

        visitedCoordinates.Add($"{guard.Row}:{guard.Col}");
        while (guard.Row >= 0 && guard.Col >= 0 && guard.Row < mapdata.Length && guard.Col < mapdata[0].Length)
        {
            var nextRow = guard.Row + guardDirection.RowDirection;
            var nextCol = guard.Col + guardDirection.ColDirection;
            if (nextRow < mapdata.Length && nextCol < mapdata[0].Length && nextRow >= 0 && nextCol >= 0)
            {
                if (mapdata[nextRow][nextCol] == '#')
                {
                    guardDirection.Turn();
                }
                else
                {
                    guard.Row = nextRow;
                    guard.Col = nextCol;
                    visitedCoordinates.Add($"{guard.Row}:{guard.Col}");
                }
            }
            else
            {
                guard.Row = nextRow;
                guard.Col = nextCol;
            }
        }
        return visitedCoordinates;
    }


    public static int GetCombinationsOfPossibleLoops(char[][] mapdata)
    {
        // establish base path
        var basePath = walkRegularPath(mapdata);
        // Where is the guard? So we can avoid it later on 
        var guardPos = FindGuardPosition(mapdata);
        var guardCoord = $"{guardPos.Row}:{guardPos.Col}";
        HashSet<string> LoopCoords = new();

        foreach (var coord in basePath)
        {
            var coordParts = coord.Split(":");
            var cRow = int.Parse(coordParts[0]);
            var cCol = int.Parse(coordParts[1]);
            var freshMapData = DeepCopy(mapdata);
            // Don't try to put an obstacle on top of the guard
            if (coord != guardCoord)
            {
                // replace that coordinate with a # and try to walk it to find a possible loop
                freshMapData[cRow][cCol] = '#';
                if (walkPathMakesALoop(freshMapData))
                {
                    LoopCoords.Add(coord);
                }
            }
        }
        return LoopCoords.Count;
    }

    private static char[][] DeepCopy(char[][] source)
    {
        char[][] copy = new char[source.Length][];
        for (int i = 0; i < source.Length; i++)
        {
            copy[i] = new char[source[i].Length];
            Array.Copy(source[i], copy[i], source[i].Length);
        }
        return copy;
    }

    private static (int Row, int Col) FindGuardPosition(char[][] mapdata)
    {

        for (int row = 0; row < mapdata.Length; row++)
        {
            for (int col = 0; col < mapdata[row].Length; col++)
            {
                if (mapdata[row][col] == '^')
                {
                    return (row, col);
                }
            }
        }
        return (-1, -1);
    }

    private static void printMap(char[][] mapdata)
    {
        for (int r = 0; r < mapdata.Length; r++)
        {
            for (int c = 0; c < mapdata[0].Length; c++)
            {
                Console.Write(mapdata[r][c]);
            }
            Console.Write("\r\n");
        }
        Console.Write("\r\n");
        Console.Write("\r\n");
    }
    public static bool walkPathMakesALoop(char[][] mapdata)
    {
        HashSet<string> visitedCoordinates = new HashSet<string>();
        HashSet<string> wallBumpsWithDirection = new(); // Coordinate + Direction travelling

        Coordinate guard = new();
        Direction guardDirection = new();
        for (int row = 0; row < mapdata.Length; row++)
        {
            for (int col = 0; col < mapdata[row].Length; col++)
            {
                if (mapdata[row][col] == '^')
                {
                    guard.Row = row;
                    guard.Col = col;
                }
            }
        }
        visitedCoordinates.Add($"{guard.Row}:{guard.Col}");
        while (guard.Row >= 0 && guard.Col >= 0 && guard.Row < mapdata.Length && guard.Col < mapdata[0].Length)
        {
            var nextRow = guard.Row + guardDirection.RowDirection;
            var nextCol = guard.Col + guardDirection.ColDirection;
            if (nextRow < mapdata.Length && nextCol < mapdata[0].Length && nextRow >= 0 && nextCol >= 0)
            {
                if (mapdata[nextRow][nextCol] == '#')
                {
                    // detected a "bump" into a wall
                    string wallBumpWithDirection = $"{guard.Row}:{guard.Col}:{guardDirection.RowDirection}:{guardDirection.ColDirection}";

                    // check if we can add this to the hashset - if not, we have a loop
                    if (!wallBumpsWithDirection.Add(wallBumpWithDirection))
                    {
                        return true;
                    }
                    guardDirection.Turn();
                }
                else
                {
                    guard.Row = nextRow;
                    guard.Col = nextCol;
                    visitedCoordinates.Add($"{guard.Row}:{guard.Col}");
                }
            }
            else
            {
                guard.Row = nextRow;
                guard.Col = nextCol;
            }
        }
        return false;
    }
}



internal class Coordinate
{
    public int Row { get; set; }
    public int Col { get; set; }
}


enum DirectionType
{
    Up,
    Right,
    Down,
    Left
}
internal class Direction
{
    public int RowDirection { get; private set; }
    public int ColDirection { get; private set; }
    private DirectionType DirType { get; set; }

    public Direction()
    {
        DirType = DirectionType.Up;
        SetDirection();
    }

    public void Turn()
    {
        switch (this.DirType)
        {
            case DirectionType.Up:
                this.DirType = DirectionType.Right;
                break;
            case DirectionType.Right:
                this.DirType = DirectionType.Down;
                break;
            case DirectionType.Down:
                this.DirType = DirectionType.Left;
                break;
            case DirectionType.Left:
                this.DirType = DirectionType.Up;
                break;
            default:
                throw new NotImplementedException();

        }
        SetDirection();
    }

    private void SetDirection()
    {
        switch (this.DirType)
        {
            case DirectionType.Up:
                RowDirection = -1;
                ColDirection = 0;
                break;
            case DirectionType.Right:
                RowDirection = 0;
                ColDirection = 1;
                break;
            case DirectionType.Down:
                RowDirection = 1;
                ColDirection = 0;
                break;
            case DirectionType.Left:
                RowDirection = 0;
                ColDirection = -1;
                break;
            default:
                throw new NotImplementedException();

        }
    }
}
public class Day06Tests
{

    private string[] testInput = [
                    "....#.....",
                    ".........#",
                    "..........",
                    "..#.......",
                    ".......#..",
                    "..........",
                    ".#..^.....",
                    "........#.",
                    "#.........",
                    "......#...",
            ];
    [Fact]
    public void Part1()
    {
        var mapdata = testInput.Select(line => line.ToCharArray()).ToArray();
        var result = SuitManufacturingLabMapHandler.walkRegularPath(mapdata);
        Assert.Equal(41,result.Count);
    }

    [Fact]
    public void Part2()
    {
        var mapdata = testInput.Select(line => line.ToCharArray()).ToArray();
        var resultPart2 = SuitManufacturingLabMapHandler.GetCombinationsOfPossibleLoops(mapdata);
        Assert.Equal(6,resultPart2);
    }
}