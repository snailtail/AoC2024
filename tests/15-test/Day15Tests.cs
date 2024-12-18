using System.Text;

namespace _15_test;

public class WarehouseMap
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
        for(int row = 0; row < MapData.Length;row++)
        {
            for(int col = 0; col < MapData[row].Length;col++)
            {
                Console.Write(MapData[row][col]);
            }
            Console.Write("\r\n");
        }
    }
    private char[][] _mapData;
    public WarehouseMap(string[] input)
    {
        this._mapData=  input.Select(l => l.ToCharArray()).ToArray();
    }


    public virtual char[][] MapData => _mapData;
    public (int row, int col) RobotPosition => MapData
            .SelectMany((row, rowIndex) =>
                row.Select((value, columnIndex) => new { value, rowIndex, columnIndex }))
            .Where(cell => cell.value == '@').Select(coord => (coord.rowIndex, coord.columnIndex)).First();

    public virtual List<(int row, int col)> OCoordinates => MapData
            .SelectMany((row, rowIndex) => 
                row.Select((value, colIndex) => new { value, rowIndex, colIndex }))
            .Where(cell => cell.value == 'O')
            .Select(cell => (cell.rowIndex, cell.colIndex))
            .ToList();

    public virtual int GPSCoordinateSum => OCoordinates.Select(oc => oc.row * 100 + oc.col).Sum();

    public virtual bool isBlocked((int row, int col) coordinate, char direction)
    {
        var checkRow = coordinate.row + Directions[direction].row;
        var checkCol = coordinate.col + Directions[direction].col;

        if(MapData[checkRow][checkCol]=='#')
        {
            return true;
        }
        
        if(MapData[checkRow][checkCol]=='.')
        {
            return false;
        }

        if(MapData[checkRow][checkCol]=='O')
        {
            return isBlocked((checkRow,checkCol), direction);
        }
        return false;
    }

    public virtual bool isNextToBox((int row, int col) coordinate, char direction)
    {
        var checkRow = coordinate.row + Directions[direction].row;
        var checkCol = coordinate.col + Directions[direction].col;
        if(MapData[checkRow][checkCol]=='O')
        {
            return true;
        }

        return false;
    }

    public virtual bool MoveRobot(char direction)
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

                MapData[box.row][box.col] = '.';
                int newRow = box.row + Directions[direction].row;
                int newCol = box.col + Directions[direction].col;
                MapData[newRow][newCol] = 'O';
            }
            // finally move the robot position
            int robotNewRow = this.RobotPosition.row + Directions[direction].row;
            int robotNewCol = this.RobotPosition.col + Directions[direction].col;

            MapData[RobotPosition.row][RobotPosition.col] = '.';
            MapData[robotNewRow][robotNewCol] = '@';
        }
        return !blocked;
    }

}


public class SecondWarehouseMap : WarehouseMap
{
    public char[][] _extendedMapdata;

    public override char[][] MapData => _extendedMapdata;
    public SecondWarehouseMap(string[] input) : base(input)
    {
        char[][] _initialMapData; 
        _initialMapData = input.Select(l => l.ToCharArray()).ToArray();
        _initialMapData = new char[input.Length][];
        
        for(int l = 0; l < input.Length; l++)
        {
            StringBuilder sb = new();
            var line = input[l];
            foreach(char c in line)
            {
                string extendedChars = c switch{
                    '#' => "##",
                    'O' => "[]",
                    '.' => "..",
                    '@' => "@.",
                    _ => "\0"
                };
                sb.Append(extendedChars);
            }
            _initialMapData[l]=sb.ToString().ToCharArray();
        }
        this._extendedMapdata = _initialMapData.Select(l => l).ToArray();

    }


    public override List<(int row, int col)> OCoordinates => MapData
            .SelectMany((row, rowIndex) => 
                row.Select((value, colIndex) => new { value, rowIndex, colIndex }))
            .Where(cell => cell.value == '[')
            .Select(cell => (cell.rowIndex, cell.colIndex))
            .ToList();

    public override int GPSCoordinateSum => this.GetGPSCoordinateSum();

    private int GetGPSCoordinateSum()
    {
        int sum = 0;
        foreach(var coordinate in OCoordinates)
        {
            int distanceFromLeftEdge = coordinate.col;
            int distanceFromTopEdge = coordinate.row;
            int result = distanceFromLeftEdge + distanceFromTopEdge * 100;
            sum += result;
        }
        return sum;
    }

    public override bool isBlocked((int row, int col) coordinate, char direction)
    {
        var checkRow = coordinate.row + Directions[direction].row;
        var checkCol = coordinate.col + Directions[direction].col;

        // The contents of this exact coordinate
        char thisTile = MapData[coordinate.row][coordinate.col];

        // the tile we are checking initially
        char nextTile = MapData[checkRow][checkCol];
        
        // is this coordinate the robot itself?
        var isRobot = thisTile == '@';
        
        // om thisTile = @ så är vi roboten

        if(nextTile=='#')
        {
            return true;
        }
        
        if(nextTile=='.')
        {
            return false;
        }

        if(nextTile=='[' || nextTile==']')
        {
            // if we are moving left or right we can just go on checking on one row
            if(direction=='<' || direction=='>')
            {
                return isBlocked((checkRow,checkCol), direction);
            }
            //if we are moving up or down we need to account for "skewed" boxes.
            /*
                For example if the robot is here and moving up:
                #######
                #.....#
                #[][].#
                #.[]..#
                #..@..#
                #.....#
                It will see an ] above it
                then we will need to check above the tile to the left of it as well
                so based on whether the next tile contains a ] or [ we will need to calculate
                another coordinate to the left or right of it, which also needs to be checked.
                might work with an OR - since we are only checking two tiles - that should propagate outwards recursively from there.
                return isBlocked((x,y),direction) || isBlocked((x,y))
            */
            int checkSecondColumn = nextTile == ']' ? checkCol-1 : checkCol+1;
            return isBlocked((checkRow,checkCol), direction) || isBlocked((checkRow,checkSecondColumn), direction);
        }
        return false;
    }

    public override bool isNextToBox((int row, int col) coordinate, char direction)
    {
        var checkRow = coordinate.row + Directions[direction].row;
        var checkCol = coordinate.col + Directions[direction].col;
        var checkTile = MapData[checkRow][checkCol];

        if(checkTile=='[' || checkTile==']')
        {
            return true;
        }

        return false;
    }
    public override bool MoveRobot(char direction)
    {
        var blocked = isBlocked(this.RobotPosition,direction);
        Stack<(int row, int col)> BoxesToMove = new();
        if(!blocked)
        {
            int checkRow = RobotPosition.row;
            int checkCol = RobotPosition.col;

            // for left-right we only need to check on one row.
            if(direction == '<' || direction =='>')
            {
                while(this.isNextToBox((checkRow,checkCol),direction))
                {
                    checkRow = checkRow + Directions[direction].row;
                    checkCol = checkCol + Directions[direction].col;
                    BoxesToMove.Push((checkRow, checkCol));
                }
            }

            // for up-down we need to check more...
             if(direction == 'v' || direction =='^')
            {
                Queue<(int row, int col)> tilesToCheck = new();
                tilesToCheck.Enqueue((checkRow,checkCol));
                while(tilesToCheck.Count > 0)
                {
                    var box = tilesToCheck.Dequeue();
                    char thisTile = MapData[box.row][box.col];
                    char nextTile = MapData[box.row+Directions[direction].row][box.col];
                    
                    if(nextTile=='[')
                    {
                        if(!BoxesToMove.Contains((box.row+Directions[direction].row,box.col+1)))
                            BoxesToMove.Push((box.row+Directions[direction].row,box.col+1)); // move the tile to the right of nexttile - because it belongs to that box ]
                        
                        if(!BoxesToMove.Contains((box.row+Directions[direction].row,box.col)))
                            BoxesToMove.Push((box.row+Directions[direction].row,box.col)); // move nexttile [
                        
                        tilesToCheck.Enqueue((box.row+Directions[direction].row,box.col+1));
                        tilesToCheck.Enqueue((box.row+Directions[direction].row,box.col));

                    }

                    if(nextTile==']')
                    {
                        if(!BoxesToMove.Contains((box.row+Directions[direction].row,box.col-1)))
                            BoxesToMove.Push((box.row+Directions[direction].row,box.col-1)); // move the tile to the left of nexttile - because it belongs to that box [
                        
                        if(!BoxesToMove.Contains((box.row+Directions[direction].row,box.col)))
                            BoxesToMove.Push((box.row+Directions[direction].row,box.col)); // move nexttile ]
                        
                        tilesToCheck.Enqueue((box.row+Directions[direction].row,box.col-1));
                        tilesToCheck.Enqueue((box.row+Directions[direction].row,box.col));
                    }
                }
            }


            // Move the boxes
            while(BoxesToMove.Count > 0)
            {
                var box = BoxesToMove.Pop();
                char boxChar = MapData[box.row][box.col];
                MapData[box.row][box.col] = '.';
                int newRow = box.row + Directions[direction].row;
                int newCol = box.col + Directions[direction].col;
                MapData[newRow][newCol] = boxChar;
            }
            
            // finally move the robot position
            int robotNewRow = this.RobotPosition.row + Directions[direction].row;
            int robotNewCol = this.RobotPosition.col + Directions[direction].col;
            MapData[RobotPosition.row][RobotPosition.col] = '.';
            MapData[robotNewRow][robotNewCol] = '@';
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

    private string[] smallTestInput = [
        "#######",
        "#...#.#",
        "#.....#",
        "#..OO@#",
        "#..O..#",
        "#.....#",
        "#######",
        "",
        "<vv<<^^<<^^"
    ];

    private string[] extraCornerCaseInput = [
        "#######",
        "#...#.#",
        "#.....#",
        "#.....#",
        "#.....#",
        "#.....#",
        "#.OOO@#",
        "#.OOO.#",
        "#..O..#",
        "#.....#",
        "#.....#",
        "#######",
        "",
        "v<vv<<^^^^^"        
        ];

#region Part1Tests
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
        Assert.Equal(10092,map.GPSCoordinateSum);
    }
    #endregion

    [Fact()]
    public void Part2Parsing()
    {
        var map = new SecondWarehouseMap(testInput.Where(l=> l.Contains('#')).ToArray());
        Assert.NotNull(map);
        Assert.Equal('#',map.MapData[1][0]);
        Assert.Equal('#',map.MapData[1][1]);
        Assert.Equal('[',map.MapData[1][6]);
        Assert.Equal(']',map.MapData[1][7]);
        Assert.Equal('.',map.MapData[8][17]);
        Assert.Equal('#',map.MapData[8][18]);
        Assert.Equal('@',map.MapData[4][8]);
        Assert.Equal((4,8),map.RobotPosition);
    }

    [Fact]
    public void Part2SmallInputParsing()
    {
        var map = new SecondWarehouseMap(smallTestInput.Where(l => l.Contains('#')).ToArray());
        Assert.Equal((3,10),map.RobotPosition);
    }


    [Fact()]
    public void Part2SmallInputTestIsBlocked()
    {
        var map = new SecondWarehouseMap(smallTestInput.Where(l => l.Contains('#')).ToArray());
        map.MoveRobot('>');
        Assert.Equal((3,11),map.RobotPosition);
        Assert.True(map.isBlocked(map.RobotPosition,'>'));
        Assert.False(map.isBlocked(map.RobotPosition,'<'));
    }
    
    [Fact()]
    public void Part2SmallInputTestAllMoves()
    {
        var map = new SecondWarehouseMap(smallTestInput.Where(l => l.Contains('#')).ToArray());
        var sequence = new MoveSequence(smallTestInput.Where(l => !l.Contains('#')).ToArray());
        
        foreach(char c in sequence.Sequence)
        {
            map.MoveRobot(c);
        }
        Assert.Equal((2,5),map.RobotPosition);
    }

    [Fact()]
    public void Part2testInputTestAllMoves()
    {
        var map = new SecondWarehouseMap(testInput.Where(l => l.Contains('#')).ToArray());
        var sequence = new MoveSequence(testInput.Where(l => !l.Contains('#')).ToArray());
        
        foreach(char c in sequence.Sequence)
        {
            map.MoveRobot(c);
        }
        Assert.Equal((7,4),map.RobotPosition);
        map.PrintMap();
        Assert.Equal(9021,map.GPSCoordinateSum);
    }

    [Fact]
    public void Part2TestExtraCornerCases()
    {
        var map = new SecondWarehouseMap(extraCornerCaseInput.Where(l => l.Contains('#')).ToArray());
        var sequence = new MoveSequence(extraCornerCaseInput.Where(l => !l.Contains('#')).ToArray());
        
        foreach(char c in sequence.Sequence)
        {
            map.MoveRobot(c);
        }
        Assert.Equal(2339,map.GPSCoordinateSum);
    }
}
