namespace _15_test;

public class WarehouseMap(string[] input)
{
    public Dictionary<char,(int row,int col)> Directions = new()
    {
        {'<',(0,-1)},
        {'>',(0,1)},
        {'^',(-1,0)},
        {'v',(1,0)},
    };

    public void PrintMap()
    {
        for(int row = 0; row < _mapData.Length;row++)
        {
            for(int col = 0; col < _mapData[row].Length;col++)
            {
                Console.Write(_mapData[row][col]);
            }
            Console.Write("\r\n");
        }
    }
    char[][] _mapData =  input.Select(l => l.ToCharArray()).ToArray();
    public char[][] MapData => _mapData;
    public (int row, int col) RobotPosition => _mapData
            .SelectMany((row, rowIndex) =>
                row.Select((value, columnIndex) => new { value, rowIndex, columnIndex }))
            .Where(cell => cell.value == '@').Select(coord => (coord.rowIndex, coord.columnIndex)).First();

    public List<(int row, int col)> OCoordinates => _mapData
            .SelectMany((row, rowIndex) => 
                row.Select((value, colIndex) => new { value, rowIndex, colIndex }))
            .Where(cell => cell.value == 'O')
            .Select(cell => (cell.rowIndex, cell.colIndex))
            .ToList();

    public int GPSCoordinateSum => OCoordinates.Select(oc => oc.row * 100 + oc.col).Sum();

    public bool isBlocked((int row, int col) coordinate, char direction)
    {
        var checkRow = coordinate.row + Directions[direction].row;
        var checkCol = coordinate.col + Directions[direction].col;

        if(_mapData[checkRow][checkCol]=='#')
        {
            return true;
        }
        
        if(_mapData[checkRow][checkCol]=='.')
        {
            return false;
        }

        if(_mapData[checkRow][checkCol]=='O')
        {
            return isBlocked((checkRow,checkCol), direction);
        }
        return false;
    }

    public bool isNextToBox((int row, int col) coordinate, char direction)
    {
        var checkRow = coordinate.row + Directions[direction].row;
        var checkCol = coordinate.col + Directions[direction].col;
        if(_mapData[checkRow][checkCol]=='O')
        {
            return true;
        }

        return false;
    }

    public bool MoveRobot(char direction)
    {
        var blocked = isBlocked(this.RobotPosition,direction);
        Stack<(int row, int col)> BoxesToMove = new();
        if(!blocked)
        {
            // Use IsNextToBox((int, int) coord, Direction direction) that checks if there is a box next to us in the direction we're moving.
            // For each true, add that coordinate to a stack and continue until false.
            // Then process the stack and move those boxes one step in the intended direction.
            // Lastly move the robot
            
            int checkRow = RobotPosition.row;
            int checkCol = RobotPosition.col;
            while(this.isNextToBox((checkRow,checkCol),direction))
            {
                checkRow = checkRow + Directions[direction].row;
                checkCol = checkCol + Directions[direction].col;
                BoxesToMove.Push((checkRow, checkCol));
            }
            // Move the boxes

            while(BoxesToMove.Count > 0)
            {
                var box = BoxesToMove.Pop();

                _mapData[box.row][box.col] = '.';
                int newRow = box.row + Directions[direction].row;
                int newCol = box.col + Directions[direction].col;
                _mapData[newRow][newCol] = 'O';
            }
            // finally move the robot position
            int robotNewRow = this.RobotPosition.row + Directions[direction].row;
            int robotNewCol = this.RobotPosition.col + Directions[direction].col;

            _mapData[RobotPosition.row][RobotPosition.col] = '.';
            _mapData[robotNewRow][robotNewCol] = '@';
        }
        return !blocked;
    }

}

public class MoveSequence(string[] input)
{
    char[] _sequence = String.Join("",input).ToCharArray();
    public char[] Sequence => _sequence;

}

public class Day15Tests
{

    private string[] testInput = [
        "##########",
        "#..O..O.O#",
        "#......O.#",
        "#.OO..O.O#",
        "#..O@..O.#",
        "#O#..O...#",
        "#O..O..O.#",
        "#.OO.O.OO#",
        "#....O...#",
        "##########",
        "",
        "<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^",
        "vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v",
        "><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<",
        "<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^",
        "^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><",
        "^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^",
        ">^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^",
        "<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>",
        "^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>",
        "v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^"
    ];

    [Fact]
    public void TestParsing()
    {
        var map = new WarehouseMap(testInput.Where(l=> l.Contains('#')).ToArray());
        var sequence = new MoveSequence(testInput.Where(l=> !l.Contains('#')).ToArray());
        Assert.Equal(10,map.MapData.Length);
        Assert.Equal(10,map.MapData[0].Length);
        Assert.Equal(700,sequence.Sequence.Length);
        Assert.Equal('<',sequence.Sequence[0]);
        Assert.Equal('v',sequence.Sequence[1]);
        Assert.Equal('>',sequence.Sequence[3]);
        Assert.Equal('^',sequence.Sequence[4]);
        Assert.Equal((4,4),map.RobotPosition);
    }


    [Fact]
    public void TestBlocked()
    {
        var map = new WarehouseMap(testInput.Where(l=> l.Contains('#')).ToArray());
        
        Assert.False(map.isBlocked((1,1),'>'));
        Assert.True(map.isBlocked((1,1),'<'));
        Assert.True(map.isBlocked((7,7),'>'));
        Assert.False(map.isBlocked((6,1),'>'));
        Assert.True(map.isBlocked((7,5),'v'));        
    }

    [Fact]
    public void TestAFewMoves()
    {
        var map = new WarehouseMap(testInput.Where(l=> l.Contains('#')).ToArray());
        
        map.MoveRobot('>');
        Assert.Equal((4,5),map.RobotPosition);
        
        map.MoveRobot('>');
        Assert.Equal((4,6),map.RobotPosition);
        
        map.MoveRobot('>');
        Assert.Equal((4,7),map.RobotPosition);
        
        map.MoveRobot('>');
        Assert.Equal((4,7),map.RobotPosition);
        
        map.MoveRobot('v');
        Assert.Equal((5,7),map.RobotPosition);
        
        map.MoveRobot('v');
        Assert.Equal((6,7),map.RobotPosition);
        
        map.MoveRobot('v');
        Assert.Equal((6,7),map.RobotPosition);
               
    }


     [Fact]
    public void TestAllTestInputMoves()
    {

        
        var map = new WarehouseMap(testInput.Where(l=> l.Contains('#')).ToArray());
        var sequence = new MoveSequence(testInput.Where(l=> !l.Contains('#')).ToArray());
        
        foreach(char c in sequence.Sequence)
        {
            map.MoveRobot(c);
        }
        map.PrintMap();
        Assert.Equal('O',map.MapData[8][8]);
        Assert.Equal((4,3),map.RobotPosition);
        Assert.Contains((1,2),map.OCoordinates);
        Assert.DoesNotContain((1,1),map.OCoordinates);
    }

     [Fact]
    public void TestPart1Sum()
    {

        
        var map = new WarehouseMap(testInput.Where(l=> l.Contains('#')).ToArray());
        var sequence = new MoveSequence(testInput.Where(l=> !l.Contains('#')).ToArray());
        foreach(char c in sequence.Sequence)
        {
            map.MoveRobot(c);
        }
        map.PrintMap();
        Assert.Equal(10092,map.GPSCoordinateSum);
    }
}
