using System.Text;

namespace _16_test;


public class Maze
{
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

}

public static class MazeExtensions
{
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
        int result = maze.Data.FindCheapestPath(maze.StartPoint.ToTuple(), maze.EndPoint.ToTuple());
        Assert.Equal(7036, result);

    }

}
