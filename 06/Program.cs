var mapdata = File.ReadAllLines("06.dat").Select(line => line.ToCharArray()).ToArray();
Coordinate guard = new();
Direction guardDirection = new();

HashSet<string> visitedCoordinates = new HashSet<string>();
HashSet<(int row, int col)> obstacleCoordinates = new();
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
            obstacleCoordinates.Add((nextRow, nextCol));
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

Console.WriteLine($"Part 1: {visitedCoordinates.Count}");
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
        Console.WriteLine($"Turning from direction {this.DirType.ToString()}");
        switch (this.DirType)
        {
            case DirectionType.Up:
                this.DirType=DirectionType.Right;
                break;
            case DirectionType.Right:
                this.DirType=DirectionType.Down;
                break;
            case DirectionType.Down:
                this.DirType=DirectionType.Left;
                break;
            case DirectionType.Left:
                this.DirType=DirectionType.Up;
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