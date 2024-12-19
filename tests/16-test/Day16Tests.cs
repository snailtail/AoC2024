using System.Text;

namespace _16_test;


public class Maze
{
    private static readonly (int dCol, int dRow)[] directions =  { (-1, 0), (0, 1), (1, 0), (0, -1) }; // up, right, down, left

    private char[][] _data;
    public Coordinate StartPoint => Data
        .SelectMany((row, rowIndex) =>
            row.Select((value, columnIndex) => new { value, rowIndex, columnIndex }))
        .Where(cell => cell.value == 'S')
        .Select(coord => new Coordinate(coord.rowIndex, coord.columnIndex))
        .First();

    public Coordinate EndPoint => Data
        .SelectMany((row, rowIndex) =>
            row.Select((value, columnIndex) => new { value, rowIndex, columnIndex }))
        .Where(cell => cell.value == 'E')
        .Select(coord => new Coordinate(coord.rowIndex, coord.columnIndex))
        .First();
    public char[][] Data => _data;
    public Maze(string[] input)
    {
        this._data = input.Select(line => line.ToCharArray()).ToArray();
    }


    private IEnumerable<(int cost, (int row, int col, int direction))> getNeighbors((int row, int col, int direction) current)
    {
        var directions = new (int dCol, int dRow)[] { (-1, 0), (0, 1), (1, 0), (0, -1) }; // up, right, down, left
        var (currentRow, currentCol, currentDirection) = current;
        yield return (1000, (currentRow, currentCol, (currentDirection + 1) % 4));
        yield return (1000, (currentRow, currentCol, (currentDirection - 1) % 4));

        int newRow = currentRow + directions[currentDirection].dRow;
        int newCol = currentCol + directions[currentDirection].dCol;
        if(Data[newRow][newCol] != '#')
        {
            yield return (1, (newRow, newCol, currentDirection));
        }
    }

    

    public int Part1 => Data.FindCheapestPath(this.StartPoint.ToTuple(), this.EndPoint.ToTuple());



}

public static class MazeExtensions
{
    private static readonly (int dCol, int dRow)[] directions =  { (-1, 0), (0, 1), (1, 0), (0, -1) }; // up, right, down, left

    
    public static int FindCheapestPath(this char[][] grid, (int row, int col) start, (int row, int col) end)
    {
        int rows = grid.Length;
        int cols = grid[0].Length;

        var directions = new (int dRow, int dCol)[] { (-1, 0), (0, 1), (1, 0), (0, -1) };

        var pq = new PriorityQueue<(int cost, int row, int col, int direction, List<(int row, int col)> path), int>();

        var visited = new HashSet<(int row, int col, int direction)>();

        // We initially face right/east (1)
        int initialDirection = 1;
        for (int newDirection = 0; newDirection < 4; newDirection++)
        {
            int turnCost = (initialDirection != newDirection) ? 1000 : 0; // Add a turning cost if we're not facing the right direction
            pq.Enqueue((turnCost, start.row, start.col, newDirection, new List<(int row, int col)> { start }), turnCost);
        }

        while (pq.Count > 0)
        {
            var (currentCost, currentRow, currentCol, currentDirection, currentPath) = pq.Dequeue();

            // We hit our target - return cost and path
            if ((currentRow, currentCol) == end)
            {
                return currentCost;
            }

            // Skip already visited coordinates with the same direction
            if (visited.Contains((currentRow, currentCol, currentDirection)))
            {
                continue;
            }

            visited.Add((currentRow, currentCol, currentDirection));


            for (int newDirection = 0; newDirection < 4; newDirection++)
            {
                int newRow = currentRow + directions[newDirection].dRow;
                int newCol = currentCol + directions[newDirection].dCol;

                // Can we move in that direction?
                if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols && grid[newRow][newCol] != '#')
                {

                    int turnCost = (currentDirection != newDirection) ? 1000 : 0;
                    int stepCost = 1;
                    int totalCost = currentCost + turnCost + stepCost;

                    var newPath = new List<(int row, int col)>(currentPath) { (newRow, newCol) };

                    pq.Enqueue((totalCost, newRow, newCol, newDirection, newPath), totalCost);
                }
            }
        }

        // No path was found
        return int.MinValue;
    }


    public static string Printable(this List<(int row, int col)> path)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < path.Count; i++)
        {
            sb.Append($"({path[i].row}, {path[i].col})");
            if (i < path.Count - 1)
            {
                sb.Append(" -> ");
            }
        }
        return sb.ToString();
    }



    private static readonly (int, int)[] Directions = { (0, 1), (1, 0), (0, -1), (-1, 0) }; // East, South, West, North
    private static readonly Dictionary<(int, int), int> DirectionIndices = new Dictionary<(int, int), int>
    {
        [(0, 1)] = 0,
        [(1, 0)] = 1,
        [(0, -1)] = 2,
        [(-1, 0)] = 3
    };

    public static List<List<(int, int)>> BfsFindAllPaths(this char[][] grid, (int, int) start, (int, int) end, int targetCost)
    {
        var queue = new Queue<((int, int) pos, List<(int, int)> path, int cost, (int, int) direction)>();
        var allPaths = new List<List<(int, int)>>();
        var initialDirection = (0, 1); // Facing East
        queue.Enqueue((start, new List<(int, int)> { start }, 0, initialDirection));

        while (queue.Count > 0)
        {
            var (currentPos, path, currentCost, currentDirection) = queue.Dequeue();
            var (currentRow, currentCol) = currentPos;
            if(currentCost > targetCost)
            {
                continue;
            }
            
            if (currentPos == end && currentCost == targetCost)
            {
                allPaths.Add(new List<(int, int)>(path));
                continue;
            }

            for (int i = 0; i < Directions.Length; i++)
            {
                var newDirection = Directions[i];
                int newRow = currentRow + newDirection.Item1;
                int newCol = currentCol + newDirection.Item2;
                var newPos = (newRow, newCol);

                if (IsValid(grid, newRow, newCol))
                {
                    int turnCost = CalculateTurnCost(currentDirection, newDirection);
                    int newCost = currentCost + 1 + turnCost;

                    if (newCost <= targetCost)
                    {
                        var newPath = new List<(int, int)>(path) { newPos };
                        queue.Enqueue((newPos, newPath, newCost, newDirection));
                    }
                }
            }
        }

        return allPaths;
    }


    private static int CalculateTurnCost((int, int) currentDirection, (int, int) newDirection)
    {
        if (currentDirection == newDirection)
        {
            return 0;
        }

        int currentDirIndex = DirectionIndices[currentDirection];
        int newDirIndex = DirectionIndices[newDirection];

        int turnDelta = Math.Abs(currentDirIndex - newDirIndex);
        if (turnDelta == 3) // Special case for wrapping around (East to North or North to East)
        {
            turnDelta = 1;
        }

        return turnDelta * 1000;
    }
    private static bool IsValid(char[][] grid, int row, int col)
    {
        return row >= 0 && row < grid.Length && col >= 0 && col < grid[0].Length && grid[row][col] != '#';
    }

}

public class Coordinate(int y, int x)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public static Coordinate operator +(Coordinate a, Coordinate b)
    {

        return new Coordinate(a.X + b.X, a.Y + b.Y);
    }
    public (int Row, int Col) ToTuple() => (Y, X);
}

public class Day16Tests
{
    private string[] testInput = [
        "###############",
        "#.......#....E#",
        "#.#.###.#.###.#",
        "#.....#.#...#.#",
        "#.###.#####.#.#",
        "#.#.#.......#.#",
        "#.#.#####.###.#",
        "#...........#.#",
        "###.#.#####.#.#",
        "#...#.....#.#.#",
        "#.#.#.###.#.#.#",
        "#.....#...#.#.#",
        "#.###.#.#.#.#.#",
        "#S..#.....#...#",
        "###############"
        ];

    private string[] secondTestInput = [
        "#################",
        "#...#...#...#..E#",
        "#.#.#.#.#.#.#.#.#",
        "#.#.#.#...#...#.#",
        "#.#.#.#.###.#.#.#",
        "#...#.#.#.....#.#",
        "#.#.#.#.#.#####.#",
        "#.#...#.#.#.....#",
        "#.#.#####.#.###.#",
        "#.#.#.......#...#",
        "#.#.###.#####.###",
        "#.#.#...#.....#.#",
        "#.#.#.#####.###.#",
        "#.#.#.........#.#",
        "#.#.#.#########.#",
        "#S#.............#",
        "#################"
        ];

    [Fact]
    public void TestParsing()
    {
        var maze = new Maze(testInput);
        Assert.Equal(15, maze.Data.Count());
        Assert.True(maze.Data.All(row => row.Length == 15));
        Assert.Equal(1, maze.EndPoint.Y);
        Assert.Equal(13, maze.StartPoint.Y);
        Assert.Equal(13, maze.EndPoint.X);
        Assert.Equal(1, maze.StartPoint.X);
    }

    [Fact]
    public void testCheapestPath()
    {
        var maze = new Maze(testInput);
        int result = maze.Part1;
        Assert.Equal(7036, result);

    }

    [Fact]
    public void testAllPathsWithLowestCost()
    {
        var maze = new Maze(testInput);
        int result = maze.Data.FindCheapestPath(maze.StartPoint.ToTuple(), maze.EndPoint.ToTuple());
        Assert.Equal(7036, result);

        var paths = maze.Data.BfsFindAllPaths(maze.StartPoint.ToTuple(), maze.EndPoint.ToTuple(), result);
        HashSet<(int row, int col)> uniqueCoordinates = new();
        foreach (var path in paths)
        {
            foreach (var coord in path)
            {
                uniqueCoordinates.Add(coord);
            }
        }
        Assert.Equal(45, uniqueCoordinates.Count);
    }

    [Fact]
    public void testAllPathsWithLowestCostSecondTestInput()
    {
        var maze = new Maze(secondTestInput);
        int result = maze.Data.FindCheapestPath(maze.StartPoint.ToTuple(), maze.EndPoint.ToTuple());
        Assert.Equal(11048, result);

        var paths = maze.Data.BfsFindAllPaths(maze.StartPoint.ToTuple(), maze.EndPoint.ToTuple(), result);
        HashSet<(int row, int col)> uniqueCoordinates = new();
        foreach (var path in paths)
        {
            foreach (var coord in path)
            {
                uniqueCoordinates.Add(coord);
            }
        }
        Assert.Equal(64, uniqueCoordinates.Count);
    }

    [Fact(Skip="DoesNotCompute")]
    public void testPart2RealInput()
    {
        var maze = new Maze(File.ReadAllLines("16.dat"));
        int result = maze.Data.FindCheapestPath(maze.StartPoint.ToTuple(), maze.EndPoint.ToTuple());
        Assert.Equal(91464, result);

        var paths = maze.Data.BfsFindAllPaths(maze.StartPoint.ToTuple(), maze.EndPoint.ToTuple(), result);
        HashSet<(int row, int col)> uniqueCoordinates = new();
        foreach (var path in paths)
        {
            foreach (var coord in path)
            {
                uniqueCoordinates.Add(coord);
            }
        }
        Assert.Equal(64, uniqueCoordinates.Count);
    }

}
