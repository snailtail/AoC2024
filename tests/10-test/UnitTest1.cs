using System.Text;

namespace _10_test;

public class Day10
{
    public static int Part1(string inputFilePath)
    {
        var grid = readGridFromFile(inputFilePath);

        (var peaks, _) = FindPaths(grid);
        var scoreSum = 0;
        foreach (var peak in peaks)
        {
            scoreSum += peak.Value.Count();
        }

        return scoreSum;
    }
    public static int Part2(string inputFilePath)
    {
        var grid = readGridFromFile(inputFilePath);

        (_, var paths) = FindPaths(grid);
        return paths.Count;
    }

    private static int[,] readGridFromFile(string path)
    {
        // Read the file and split into lines
        string[] lines = File.ReadAllLines(path);

        // Get the dimensions of the grid
        int rows = lines.Length;
        int cols = lines[0].Length;

        // Create a 2D array
        int[,] grid = new int[rows, cols];

        // Fill the grid
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                grid[r, c] = lines[r][c] - '0'; // Convert char to int
            }
        }

        return grid;
    }

    static (Dictionary<(int,int),HashSet<(int, int)>>, HashSet<string>) FindPaths(int[,] grid)
    {
        var trailHeadWithPeaks = new Dictionary<(int,int),HashSet<(int, int)>>();
        var paths = new HashSet<string>();
        var peaks = new HashSet<(int, int)>();
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        // Find all starting points (cells with value 0)
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (grid[r, c] == 0)
                {
                    var visited = new HashSet<(int, int)>();
                    var currentPath = new List<(int, int)>();
                    DFS(grid, r, c, visited, currentPath, peaks, paths);
                    trailHeadWithPeaks.Add((r,c),peaks);
                    peaks = new HashSet<(int, int)>();
                }
            }
        }

        return (trailHeadWithPeaks, paths);
    }

    static void DFS(int[,] grid, int r, int c, HashSet<(int, int)> visited, List<(int, int)> currentPath, HashSet<(int, int)> peaks, HashSet<string> paths)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        // Add the current cell to the path and mark it as visited
        currentPath.Add((r, c));
        visited.Add((r, c));

        // If we reached a cell with value 9, save the path
        if (grid[r, c] == 9)
        {
            paths.Add(pathToString(currentPath));
            peaks.Add((r,c));
        }
        else
        {
            // Explore neighbors (up, down, left, right)
            var directions = new (int dr, int dc)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
            foreach (var (dr, dc) in directions)
            {
                int nr = r + dr;
                int nc = c + dc;

                // Check if the neighbor is within bounds, not visited, and valid (value is 1 greater)
                if (nr >= 0 && nr < rows && nc >= 0 && nc < cols &&
                    !visited.Contains((nr, nc)) &&
                    grid[nr, nc] == grid[r, c] + 1)
                {
                    DFS(grid, nr, nc, visited, currentPath, peaks, paths);
                }
            }
        }

        // Backtrack: remove the current cell from path and visited set
        currentPath.RemoveAt(currentPath.Count - 1);
        visited.Remove((r, c));
    }

    private static string pathToString(List<(int, int)> path)
    {
        StringBuilder sb = new();
        for (int i = 0; i < path.Count; i++)
        {
            (int x, int y) = path[i];
            sb.Append($"({x}, {y})");
        }
        return sb.ToString();
    }
    
}

public class Day10Test
{
    [Fact]
    public void TestPart1()
    {
        var result = Day10.Part1("10test.dat");
        Assert.Equal(36,result);

    }
    
    [Fact]
    public void TestPart2()
    {
        var result = Day10.Part2("10test.dat");
        Assert.Equal(81,result);

    }
}

public class PartsTest
{
    [Fact()]
    public void Part1()
    {
        var result = Day10.Part1("10.dat");
        Assert.Equal(717,result);
    }
    
    [Fact()]
    public void Part2()
    {
        var result = Day10.Part2("10.dat");
        Assert.Equal(1686,result);
    }
}