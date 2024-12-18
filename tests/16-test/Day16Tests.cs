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

public class Coordinate(int y, int x)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public static Coordinate operator +(Coordinate a, Coordinate b)
    {
        return new Coordinate(a.X + b.X, a.Y + b.Y);
    }
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
    [Fact]
    public void TestParsing()
    {
        var maze = new Maze(testInput);
        Assert.Equal(15, maze.Data.Count());
        Assert.True(maze.Data.All(row => row.Length == 15));
        Assert.Equal(1,maze.EndPoint.Y);
        Assert.Equal(13,maze.StartPoint.Y);
        Assert.Equal(13,maze.EndPoint.X);
        Assert.Equal(1,maze.StartPoint.X);
    }
}
