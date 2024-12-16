using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace day14_test;

public static class RobotExtensions
{
    public static void ElapseTime(this List<Robot> robots, int seconds)
    {
         foreach(var robot in robots)
        {
            robot.ElapseTime(seconds);
        }
    }

    public static int SafetyFactor(this List<Robot> robots, int afterSeconds)
    {
        robots.ElapseTime(afterSeconds);
        int safetyFactor = 1;
        foreach(int i in Enumerable.Range(1,4))
        {
            safetyFactor *= robots.Where(r=> r.IsInQuadrant(i)).Count();
        }
        return safetyFactor;
    }

    public static void PrintGrid(this List<Robot> robots, int gridWidth=11, int gridHeight=7)
    {   
        for(int row = 0; row < gridHeight; row++)
        {
            for(int col = 0; col < gridWidth; col++)
            {
                char output = robots.SortedGrid().Any(r => r.X == col && r.Y == row) ? 'X' : '.';
                Console.Write(output);
            }
            Console.Write("\r\n");
        }
    }

    public static List<Robot> SortedGrid (this List<Robot> robots) => robots.OrderBy(r => r.Y).ThenBy(r => r.X).ToList();

    public static int PercentOnLowerQuadrants(this List<Robot> robots, int gridWidth, int gridHeight)
    {
        var robotsQ1 = robots.Where(r=> r.IsInQuadrant(1));
        var robotsQ2 = robots.Where(r=> r.IsInQuadrant(2));
        var robotsQ3 = robots.Where(r=> r.IsInQuadrant(3));
        var robotsQ4 = robots.Where(r=> r.IsInQuadrant(4));

        int upperQuadrantsCount = robotsQ1.Count() + robotsQ2.Count();
        int lowerQuadrantsCount = robotsQ3.Count() + robotsQ4.Count();
        int totalCount = upperQuadrantsCount+lowerQuadrantsCount;
        return lowerQuadrantsCount * 100 / totalCount;

    }

    
}

public class Robot
{

    private int _elapsedSeconds;
    public int ElapsedSeconds
    {
        get { return _elapsedSeconds; }
    }
    
    private int _gridHeight; 
    public int GridHeight
    {
        get { return _gridHeight; }  
        set { _gridHeight = value; } 
    }
    private int _gridWidth;
    public int GridWidth
    {
        get { return _gridWidth; }
        set { _gridWidth = value; }
    }
    
    
    private int _x;
    public int X
    {
        get { return _x; }
        set { _x = value; }
    }

    private int _y;
    public int Y
    {
        get { return _y; }
        set { _y = value; }
    }

    private int _vx;
    public int VelocityX
    {
        get { return _vx; }
        set { _vx = value; }
    }
    
    private int _vy;
    public int VelocityY
    {
        get { return _vy; }
        set { _vy = value; }
    }

    public void ElapseTime(int seconds = 1)
    {
        this._elapsedSeconds+=seconds;

        int movementX = (this._vx * seconds) % this._gridWidth;
        int movementY = (this._vy * seconds) % this._gridHeight;

        this._x += movementX;
        this._y += movementY;
        if(this._x >= this._gridWidth)
        {
            this._x = this._x % this._gridWidth;
        }

        if(this._y >= this._gridHeight)
        {
            this._y = this._y % this._gridHeight;
        }

        if(this._x < 0)
        {
            this._x = this._gridWidth + this._x;
        }

        if(this._y < 0)
        {
            this._y = this._gridHeight + this._y;
        }
    }

    public bool InMiddleOfGrid => _x == _gridWidth / 2 || _y == _gridHeight / 2;


    public bool IsInQuadrant(int quadrantNumber)
    {
        if(this.InMiddleOfGrid)
        {
            return false;
        }
        
        var status = quadrantNumber switch
        {
            1 => this._x < _gridWidth / 2 && this._y < _gridHeight / 2,
            2 => this._x > _gridWidth / 2 && this._y < _gridHeight / 2,
            3 => this._x < _gridWidth / 2 && this._y > _gridHeight / 2,
            4 => this._x > _gridWidth / 2 && this._y > _gridHeight / 2,
            _ => throw new NotImplementedException($"Unknown quadrant number {quadrantNumber}")
        };

        return status;

    }


    public static List<Robot> ParseFromInputArray(string[] input, int gridWidth=11, int gridHeight=7)
    {
        List<Robot> robots = [];
        foreach(var line in input)
        {
            robots.Add(new Robot(line,gridWidth,gridHeight));
        }
        return robots;
    }
    
    public Robot(string input, int gridWidth=11, int gridHeight=7)
    {
        string positionPattern = @"p=(\d+),(\d+)";
        string velocityPattern = @"v=(-*\d+),(-*\d+)";
        
        // Fetch the position parameters for this Robot.
        var positionMatch = Regex.Match(input,positionPattern);
        if(positionMatch.Groups.Count < 2)
        {
            throw new ArgumentOutOfRangeException("Position pattern not resulting in enough matches...");
        }
        _x = int.Parse(positionMatch.Groups[1].Value);
        _y = int.Parse(positionMatch.Groups[2].Value);


        // Fetch the velocity parameters for this Robot.
        var velocityMatch = Regex.Match(input,velocityPattern);
        if(velocityMatch.Groups.Count < 2)
        {
            throw new ArgumentOutOfRangeException("Velocity pattern not resulting in enough matches...");
        }
        _vx = int.Parse(velocityMatch.Groups[1].Value);
        _vy = int.Parse(velocityMatch.Groups[2].Value);

        _gridWidth=gridWidth;
        _gridHeight=gridHeight;
    }    
    

}
public class Day14Tests
{

    private string[] testInput = [
        "p=0,4 v=3,-3",
        "p=6,3 v=-1,-3",
        "p=10,3 v=-1,2",
        "p=2,0 v=2,-1",
        "p=0,0 v=1,3",
        "p=3,0 v=-2,-2",
        "p=7,6 v=-1,-3",
        "p=3,0 v=-1,-2",
        "p=9,3 v=2,3",
        "p=7,3 v=-1,2",
        "p=2,4 v=2,-3",
        "p=9,5 v=-3,-3"
    ];


    [Fact]
    public void TestParsingRobotsFromArray()
    {
        var myRobots = Robot.ParseFromInputArray(testInput);
        Assert.Equal(12,myRobots.Count);
        Assert.Equal(0,myRobots[0].X);
        Assert.Equal(4,myRobots[0].Y);
        Assert.Equal(3,myRobots[0].VelocityX);
        Assert.Equal(-3,myRobots[0].VelocityY);

        Assert.Equal(6,myRobots[1].X);
        Assert.Equal(3,myRobots[1].Y);
        Assert.Equal(-1,myRobots[1].VelocityX);
        Assert.Equal(-3,myRobots[1].VelocityY);
        Assert.Equal(7,myRobots[1].GridHeight);
        Assert.Equal(11,myRobots[1].GridWidth);

        Assert.Equal(10,myRobots[2].X);
        Assert.Equal(3,myRobots[2].Y);
        Assert.Equal(-1,myRobots[2].VelocityX);
        Assert.Equal(2,myRobots[2].VelocityY);
    }

    [Fact]
    public void TestMovingOneRobotOneSecondAtATime()
    {
        string robotInput = "p=2,4 v=2,-3";
        var myRobot = new Robot(robotInput);
        Assert.Equal(2,myRobot.X);
        Assert.Equal(4,myRobot.Y);
        
        // After 1 second
        myRobot.ElapseTime(1);
        Assert.Equal(4,myRobot.X);
        Assert.Equal(1,myRobot.Y);
        
        // After 2 seconds X=6 Y=5
        myRobot.ElapseTime(1);
        Assert.Equal(6,myRobot.X);
        Assert.Equal(5,myRobot.Y);

        // After 3 seconds x=8, y=2
        myRobot.ElapseTime(1);
        Assert.Equal(8,myRobot.X);
        Assert.Equal(2,myRobot.Y);

        // After 4 seconds x=10, Y = 6
        myRobot.ElapseTime(1);
        Assert.Equal(10,myRobot.X);
        Assert.Equal(6,myRobot.Y);

        // After 5 seconds x=1, y=3
        myRobot.ElapseTime(1);
        Assert.Equal(1,myRobot.X);
        Assert.Equal(3,myRobot.Y);

    }

    [Fact]
    public void TestMovingOneRobotFiveSeconds()
    {
        string robotInput = "p=2,4 v=2,-3";
        var myRobot = new Robot(robotInput);
        Assert.Equal(2,myRobot.X);
        Assert.Equal(4,myRobot.Y);
        
        // After 5 seconds x=1, y=3
        myRobot.ElapseTime(5);
        Assert.Equal(1,myRobot.X);
        Assert.Equal(3,myRobot.Y);
    }

    [Fact]
    public void TestMovingAllRobots100Seconds()
    {
        
        var robots = Robot.ParseFromInputArray(testInput);
        
        robots.ElapseTime(100);
        
            /*
                Expected robot distribution:
                ......2..1.
                ...........
                1..........
                .11........
                .....1.....
                ...12......
                .1....1....
            */
        
        // Two robots should be at X=6, Y=0
        var filteredRobots = robots.Where(r=>r.Y==0 && r.X ==6);
        Assert.Equal(2,filteredRobots.Count());
        
        // One robot should be at X=9, Y=0
        filteredRobots = robots.Where(r=>r.Y==0 && r.X ==9);
        Assert.Single(filteredRobots);

        // No robots should be anywhere on Y=1
        filteredRobots = robots.Where(r=>r.Y==1);
        Assert.Empty(filteredRobots);

        // One robot should be at X = 0, Y =2
        filteredRobots = robots.Where(r=>r.Y==2 && r.X ==0);
        Assert.Single(filteredRobots);

        // One robot should be at X = 1, Y =3
        filteredRobots = robots.Where(r=>r.Y==3 && r.X ==1);
        Assert.Single(filteredRobots);

        // One robot should be at X = 2, Y =3
        filteredRobots = robots.Where(r=>r.Y==3 && r.X ==2);
        Assert.Single(filteredRobots);

        // One robot should be at X = 5, Y =4
        filteredRobots = robots.Where(r=>r.Y==4 && r.X ==5);
        Assert.Single(filteredRobots);

        // One robot should be at X = 3, Y =5
        filteredRobots = robots.Where(r=>r.Y==5 && r.X ==3);
        Assert.Single(filteredRobots);
        
        // Two robots should be at X = 4, Y =5
        filteredRobots = robots.Where(r=>r.Y==5 && r.X ==4);
        Assert.Equal(2,filteredRobots.Count());

        // One robot should be at X = 1, Y =6
        filteredRobots = robots.Where(r=>r.Y==6 && r.X ==1);
        Assert.Single(filteredRobots);

        // One robot should be at X = 6, Y =6
        filteredRobots = robots.Where(r=>r.Y==6 && r.X ==6);
        Assert.Single(filteredRobots);

        // Three of them should be smack in the middle
        var robotsInTheMiddle = robots.Where(r=> r.InMiddleOfGrid).ToArray();
        Assert.Equal(3,robotsInTheMiddle.Length);
    }

    [Fact]
    public void TestQuadrantsAfter100Seconds()
    {
        
        var robots = Robot.ParseFromInputArray(testInput);
        
        robots.ElapseTime(100);

        var robotsQ1 = robots.Where(r=> r.IsInQuadrant(1));
        var robotsQ2 = robots.Where(r=> r.IsInQuadrant(2));
        var robotsQ3 = robots.Where(r=> r.IsInQuadrant(3));
        var robotsQ4 = robots.Where(r=> r.IsInQuadrant(4));

        // Only one robot in Q1
        Assert.Single(robotsQ1);

        // three robots in Q2
        Assert.Equal(3,robotsQ2.Count());

        // four robots in Q3
        Assert.Equal(4,robotsQ3.Count());

        // Only one robot in Q4
        Assert.Single(robotsQ4);
    }

    [Fact]
    public void TestSafetyFactorAfterOnTestInput()
    {
        var robots = Robot.ParseFromInputArray(testInput);
        var safetyFactor = robots.SafetyFactor(100);
        Assert.Equal(12,safetyFactor);
    }


    [Fact]
    public void TestPrintingGrid()
    {
        var robots = Robot.ParseFromInputArray(testInput);
        var safetyFactor = robots.SafetyFactor(100);
        robots.PrintGrid(11,7);
        Assert.Equal(12,safetyFactor);
    }

    [Fact(Skip="Only works with the real input file for day 14")]
    public void TestPrintingRealGrid()
    {
        var robots = Robot.ParseFromInputArray(File.ReadAllLines("14.dat"),101,103);
        int startFrom = 7568;
        int runTo = startFrom + 1;
        int threshold = 63;
        robots.ElapseTime(startFrom);
        for(int i = startFrom; i < runTo; i++)
        {
            robots.ElapseTime(1);
            int percentOnLowerHalf = robots.PercentOnLowerQuadrants(101,103);
            if(percentOnLowerHalf > threshold)
            {
                robots.PrintGrid(101,103);
                Console.WriteLine($"Elapsed seconds: {robots[0].ElapsedSeconds}");
            }
        }
        Assert.Equal(1,1);
    }



}
