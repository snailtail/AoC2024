using System.Text;

namespace _16_test;



class Part2
{
    const char WALL = '#';
    const char FREE = '.';
    const int EAST = 1; 

    record Reindeer((int x, int y) Pos, int Cost, int Direction, HashSet<(int x, int y)> Visited) : IComparable<Reindeer>
    {
        public int CompareTo(Reindeer other) => Cost.CompareTo(other.Cost);
    }
    

    static (int x, int y) FindInGrid(char[][] grid, char target)
    {
        for (int x = 0; x < grid.Length; x++)
        {
            for (int y = 0; y < grid[x].Length; y++)
            {
                if (grid[x][y] == target)
                    return (x, y);
            }
        }
        throw new Exception($"Target '{target}' not found in grid.");
    }

    static char Tile(char[][] grid, (int x, int y) pos) => grid[pos.x][pos.y];

    static IEnumerable<(int x, int y)> Neighbours(
        (int x, int y) pos,
        char[][] grid,
        bool allowDiagonal,
        Func<char[][], (int x, int y), bool> moveCondition)
    {
        var dirs = new (int x, int y)[] { (0, 1), (1, 0), (0, -1), (-1, 0) }; 
        foreach (var (dx, dy) in dirs)
        {
            var adj = (pos.x + dx, pos.y + dy);
            if (InBounds(grid, adj) && moveCondition(grid, adj))
                yield return adj;
        }
    }

    static bool InBounds(char[][] grid, (int x, int y) pos) =>
        pos.x >= 0 && pos.x < grid.Length && pos.y >= 0 && pos.y < grid[0].Length;

    static int Direction((int x, int y) to, (int x, int y) from)
    {
        if (to.x > from.x) return 1; // Moving down
        if (to.x < from.x) return 3; // Moving up
        if (to.y > from.y) return 0; // Moving right
        return 2; // Moving left
    }

    static int NewCost(Reindeer reindeer, (int x, int y) p2)
    {
        int cost = reindeer.Cost;
        if (Direction(p2, reindeer.Pos) != reindeer.Direction)
        {
            cost += 1000;
        }
        return cost + 1;
    }

    public static (int MinCost, int MinPaths) Process((char[][] Grid, (int x, int y) Start, (int x, int y) End) input)
    {
        var (grid, start, end) = input;

        bool MoveCondition(char[][] g, (int x, int y) pos) => Tile(g, pos) != WALL;

        var queue = new PriorityQueue<Reindeer, int>();
        queue.Enqueue(new Reindeer(start, 0, EAST, new HashSet<(int, int)>()), 0);

        var tileCosts = new Dictionary<((int x, int y) Pos, int Direction), int>();
        int minCost = int.MaxValue;
        var minPaths = new HashSet<(int, int)>();

        while (queue.Count > 0)
        {
            var reindeer = queue.Dequeue();

            foreach (var adj in Neighbours(reindeer.Pos, grid, true, MoveCondition))
            {
                if (reindeer.Cost > minCost)
                    continue;

                if (tileCosts.TryGetValue((reindeer.Pos, reindeer.Direction), out var recordedCost) &&
                    recordedCost < reindeer.Cost)
                    continue;

                tileCosts[(reindeer.Pos, reindeer.Direction)] = reindeer.Cost;

                int cost = NewCost(reindeer, adj);
                int dir = Direction(adj, reindeer.Pos);

                if (adj == end)
                {
                    if (cost < minCost)
                    {
                        minCost = cost;
                        minPaths = new HashSet<(int, int)>(reindeer.Visited);
                    }
                    else if (cost == minCost)
                    {
                        foreach (var pos in reindeer.Visited)
                            minPaths.Add(pos);
                    }
                }
                else if (!reindeer.Visited.Contains(adj))
                {
                    var visitedCopy = new HashSet<(int, int)>(reindeer.Visited) { adj };
                    if (cost < minCost)
                        queue.Enqueue(new Reindeer(adj, cost, dir, visitedCopy), cost);
                }
            }
        }

        return (minCost, minPaths.Count + 2); // Add 2 for start and end
    }
}


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
    
    public static List<(int cost, List<(int,int)>)> FindAllPaths(this char[][] grid, (int row, int col) start, (int row, int col) end)
    {
        int rows = grid.Length;
        int cols = grid[0].Length;
        List<(int cost, List<(int,int)>)> allPaths = new();
        
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
                allPaths.Add((currentCost, new List<(int row, int col)>(currentPath)));
                //return currentCost;
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


        return allPaths;
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
        HashSet<(int row, int col, (int,int) dir)> visited = new();
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

                if (IsValid(grid, newRow, newCol) && !visited.Contains((newRow,newCol,newDirection)))
                {
                    int turnCost = CalculateTurnCost(currentDirection, newDirection);
                    int newCost = currentCost + 1 + turnCost;

                    if (newCost <= targetCost)
                    {
                        var newPath = new List<(int, int)>(path) { newPos };
                        queue.Enqueue((newPos, newPath, newCost, newDirection));
                    }
                    visited.Add((newRow, newCol, newDirection));
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
        var result = Part2.Process((maze.Data,(maze.StartPoint.X,maze.StartPoint.Y), (maze.EndPoint.X,maze.EndPoint.Y)));
        Assert.Equal(45, result.MinPaths);
    }

    [Fact]
    public void testAllPathsWithLowestCostSecondTestInput()
    {
        var maze = new Maze(secondTestInput);
        var result = Part2.Process((maze.Data,(maze.StartPoint.X,maze.StartPoint.Y), (maze.EndPoint.X,maze.EndPoint.Y)));
        Assert.Equal(64, result.MinPaths);
    }

}
