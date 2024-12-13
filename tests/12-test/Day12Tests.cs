namespace _12_test;

public class Garden
{
    private char[][] _gardenMap;
    private bool[][] _visited;

    public char[][] GardenMap => _gardenMap;
    public Garden(string[] input)
    {
        _gardenMap = input.Select(l => l.ToCharArray()).ToArray();
        _visited = new bool[_gardenMap.Length][];
        for (int i = 0; i < _gardenMap.Length; i++)
        {
            _visited[i] = new bool[_gardenMap[i].Length];
        }
    }

    public List<(char character, List<(int y, int x)> coordinates)> FindPlots()
    {
        var plots = new List<(char character, List<(int y, int x)> coordinates)>();

        for (int y = 0; y < _gardenMap.Length; y++)
        {
            for (int x = 0; x < _gardenMap[y].Length; x++)
            {
                if (!_visited[y][x])
                {
                    var cluster = FloodFill(y, x, _gardenMap[y][x]);
                    if (cluster.Count > 0)
                    {
                        plots.Add((_gardenMap[y][x], cluster));
                    }
                }
            }
        }

        return plots;
    }

    private List<(int x, int y)> FloodFill(int startY, int startX, char targetChar)
    {
        var coordinates = new List<(int y, int x)>();
        var queue = new Queue<(int y, int x)>();

        queue.Enqueue((startY, startX));
        _visited[startY][startX] = true;

        while (queue.Count > 0)
        {
            var (y, x) = queue.Dequeue();
            coordinates.Add((y, x));

            // Check all four directions
            foreach (var (dy, dx) in new[] { (0, -1), (0, 1), (-1, 0), (1, 0) })
            {
                int newY = y + dy;
                int newX = x + dx;

                if (IsInBounds(newY, newX) && !_visited[newY][newX] && _gardenMap[newY][newX] == targetChar)
                {
                    queue.Enqueue((newY, newX));
                    _visited[newY][newX] = true;
                }
            }
        }

        return coordinates;
    }

    public int CalculatePlotPerimeter(List<(int y, int x)> coordinates, char character)
    {
        int totalScore = 0;

        foreach (var (y, x) in coordinates)
        {
            foreach (var (dy, dx) in new[] { (0, -1), (0, 1), (-1, 0), (1, 0) })
            {
                int newX = x + dx;
                int newY = y + dy;
                
                if (!IsInBounds(newX, newY) || _gardenMap[newY][newX] != character)
                {
                    totalScore++;
                }
            }
        }

        return totalScore;
    }

    public int CalculatePlotCorners(List<(int y, int x)> coordinates)
    {
        int cornerCount = 0;

        foreach (var (y, x) in coordinates)
        {
            if (IsPlotCorner(y, x))
            {
                cornerCount++;
            }
        }
        return cornerCount;
    }
    
    private bool IsInBounds(int y, int x)
    {
        return y >= 0 && y < _gardenMap.Length && x >= 0 && x < _gardenMap[y].Length;
    }

    public void PrepareGardenForStep2()
    {
        _gardenMap = QuadrupleGridSize(_gardenMap);
        _visited = new bool[_gardenMap.Length][];
        for (int i = 0; i < _gardenMap.Length; i++)
        {
            _visited[i] = new bool[_gardenMap[i].Length];
        }
    }
    private char[][] QuadrupleGridSize(char[][] originalGrid)
    {
        int originalRows = originalGrid.Length;
        int originalCols = originalGrid[0].Length;
    
        // Create the new jagged array with doubled row and column sizes
        char[][] expandedGrid = new char[originalRows * 2][];
    
        // Populate each row in the expanded grid
        for (int i = 0; i < expandedGrid.Length; i++)
        {
            expandedGrid[i] = new char[originalCols * 2];
        }

        // Iterate over the original grid
        for (int i = 0; i < originalRows; i++)
        {
            for (int j = 0; j < originalCols; j++)
            {
                // Character to expand
                char c = originalGrid[i][j];

                // Map the character to a 2x2 block in the expanded grid
                expandedGrid[i * 2][j * 2] = c;
                expandedGrid[i * 2][j * 2 + 1] = c;
                expandedGrid[i * 2 + 1][j * 2] = c;
                expandedGrid[i * 2 + 1][j * 2 + 1] = c;
            }
        }

        return expandedGrid;
    }


    public bool IsPlotCorner(int y, int x)
    {
        var character = _gardenMap[y][x];
        var neighborDirections = new List<(int dy, int dx)[]>();
        neighborDirections.Add(
            [
                (0, -1), // left
                (-1, 0), // top
                (-1,-1), // top left
            ]
        );
        
        neighborDirections.Add(
            [
                (0, 1), // right
                (-1, 0), // top
                (-1,1), // top right
            ]
        );
        
        neighborDirections.Add(
            [
                (0, -1), // left
                (1, 0), // bottom
                (1,-1), // bottom left
            ]
        );
        
        neighborDirections.Add(
            [
                (0, 1), // right
                (1, 0), // bottom
                (1,1), // bottom right
            ]
        );


        // Check "outer corners"
        foreach (var tests in neighborDirections)
        {
            int differentNeighborCount = 0;
            foreach (var (dy, dx) in tests)
            {
                int newY = y + dy;
                int newX = x + dx;

                // If the neighbor is out of bounds or has a different character, count it.
                if (!IsInBounds(newY, newX) || _gardenMap[newY][newX] != character)
                {
                    differentNeighborCount++;
                }
            }

            if (differentNeighborCount == 3)
            {
                return true;
            }
            
            // Check inner corners
        
            /*
             * right==samma char && top == samma char && topright!= samma char
               or
               left==samma char && top == samma char && topleft!= samma char
               or
               right==samma char && bottom == samma char && bottomright!= samma char
               or
               left==samma char && bottom == samma char && bottomleft!= samma char
             *
             */
            // om [0]== && [1]== && [2] !=
            (int dy0, int dx0) = tests[0];
            (int dy1, int dx1) = tests[1];
            (int dy2, int dx2) = tests[2];
            
            int newY0 = y + dy0;
            int newX0 = x + dx0;
            int newY1 = y + dy1;
            int newX1 = x + dx1;
            int newY2 = y + dy2;
            int newX2 = x + dx2;

            if (
                IsInBounds(newY0, newX0)
                && IsInBounds(newY1, newX2)
                && _gardenMap[newY0][newX0] == character
                && _gardenMap[newY1][newX1] == character
                && (
                    !IsInBounds(newY2,newX2) || (IsInBounds(newY2,newX2) && _gardenMap[newY2][newX2]!= character)
                    )
            )
            {
                return true;
            }
            else if ( // Corners where two plots meet diagonally - tricky!!
                (!IsInBounds(newY0,newX0) || _gardenMap[newY0][newX0] != character)
                &&
                ((!IsInBounds(newY1,newX1) || _gardenMap[newY1][newX1] != character))
            )
            {
                return true;
            }
            
        }
        
        

        return false;
    }



    
    
}


public class Day12Tests
{
    private string[] smallTestInput = [
        "AAAA",
        "BBCD",
        "BBCC",
        "EEEC"];

    private string[] mediumTestInput =
    [
        "OOOOO",
        "OXOXO",
        "OOOOO",
        "OXOXO",
        "OOOOO",
    ];

    private string[] largeTestInput =
    [
        "RRRRIICCFF",
        "RRRRIICCCF",
        "VVRRRCCFFF",
        "VVRCCCJFFF",
        "VVVVCJJCFE",
        "VVIVCCJJEE",
        "VVIIICJJEE",
        "MIIIIIJJEE",
        "MIIISIJEEE",
        "MMMISSJEEE",
    ];

    private string[] eShapedTestInput = [
        "EEEEE",
        "EXXXX",
        "EEEEE",
        "EXXXX",
        "EEEEE"];

    private string[] sneakyTestInput = [
        "AAAAAA",
        "AAABBA",
        "AAABBA",
        "ABBAAA",
        "ABBAAA",
        "AAAAAA"];
    
    
    [Fact]
    public void Test1()
    {
        var myGarden = new Garden(smallTestInput);
        var result = myGarden.FindPlots();
        Assert.Equal(5, result.Count);
        
    }
    
    [Fact]
    public void Test2()
    {
        var myGarden = new Garden(mediumTestInput);
        var result = myGarden.FindPlots();
        Assert.Equal(5, result.Count);
        
    }
    
    [Fact]
    public void Test3()
    {
        var myGarden = new Garden(largeTestInput);
        var result = myGarden.FindPlots();
        Assert.Equal(11, result.Count);
        
    }
    
    [Fact]
    public void TestPerimeters_SmallInput()
    {
        var myGarden = new Garden(smallTestInput);
        var result = myGarden.FindPlots();
        var plotPerimeter = myGarden.CalculatePlotPerimeter(result[0].Item2, result[0].Item1);
        Assert.Equal(5, result.Count);
        Assert.Equal(10,plotPerimeter);
        
        plotPerimeter = myGarden.CalculatePlotPerimeter(result[1].Item2, result[1].Item1);
        Assert.Equal(8,plotPerimeter);
        
        plotPerimeter = myGarden.CalculatePlotPerimeter(result[2].Item2, result[2].Item1);
        Assert.Equal(10,plotPerimeter);
        
        plotPerimeter = myGarden.CalculatePlotPerimeter(result[3].Item2, result[3].Item1);
        Assert.Equal(4,plotPerimeter);
        
        plotPerimeter = myGarden.CalculatePlotPerimeter(result[4].Item2, result[4].Item1);
        Assert.Equal(8,plotPerimeter);
    }
    
    [Fact]
    public void TestPerimeters_MediumInput()
    {
        var myGarden = new Garden(mediumTestInput);
        var result = myGarden.FindPlots();
        var plotPerimeter = myGarden.CalculatePlotPerimeter(result[0].Item2, result[0].Item1);
        Assert.Equal(5, result.Count);
        Assert.Equal(36,plotPerimeter);
        
        plotPerimeter = myGarden.CalculatePlotPerimeter(result[1].Item2, result[1].Item1);
        Assert.Equal(4,plotPerimeter);
        
        plotPerimeter = myGarden.CalculatePlotPerimeter(result[2].Item2, result[2].Item1);
        Assert.Equal(4,plotPerimeter);
        
        plotPerimeter = myGarden.CalculatePlotPerimeter(result[3].Item2, result[3].Item1);
        Assert.Equal(4,plotPerimeter);
        
        plotPerimeter = myGarden.CalculatePlotPerimeter(result[4].Item2, result[4].Item1);
        Assert.Equal(4,plotPerimeter);
    }
    
    [Fact]
    public void TestPerimeters_LargeInput()
    {
        var myGarden = new Garden(largeTestInput);
        var result = myGarden.FindPlots();
        var plotPerimeter = myGarden.CalculatePlotPerimeter(result[0].Item2, result[0].Item1);
        Assert.Equal(11, result.Count);
        Assert.Equal(18,plotPerimeter);
        
        plotPerimeter = myGarden.CalculatePlotPerimeter(result[1].Item2, result[1].Item1);
        Assert.Equal(8,plotPerimeter);
        
        plotPerimeter = myGarden.CalculatePlotPerimeter(result[2].Item2, result[2].Item1);
        Assert.Equal(28,plotPerimeter);
        
        plotPerimeter = myGarden.CalculatePlotPerimeter(result[3].Item2, result[3].Item1);
        Assert.Equal(18,plotPerimeter);
        
        plotPerimeter = myGarden.CalculatePlotPerimeter(result[4].Item2, result[4].Item1);
        Assert.Equal(20,plotPerimeter);
        
        plotPerimeter = myGarden.CalculatePlotPerimeter(result[5].Item2, result[5].Item1);
        Assert.Equal(20,plotPerimeter);
        
        plotPerimeter = myGarden.CalculatePlotPerimeter(result[6].Item2, result[6].Item1);
        Assert.Equal(4,plotPerimeter);
        
        plotPerimeter = myGarden.CalculatePlotPerimeter(result[7].Item2, result[7].Item1);
        Assert.Equal(18,plotPerimeter);
        
        plotPerimeter = myGarden.CalculatePlotPerimeter(result[8].Item2, result[8].Item1);
        Assert.Equal(22,plotPerimeter);
        
        plotPerimeter = myGarden.CalculatePlotPerimeter(result[9].Item2, result[9].Item1);
        Assert.Equal(12,plotPerimeter);
        
        plotPerimeter = myGarden.CalculatePlotPerimeter(result[10].Item2, result[10].Item1);
        Assert.Equal(8,plotPerimeter);
    }

    [Fact]
    public void TestTotalPricePart1()
    {
        var myGarden = new Garden(largeTestInput);
        var plots = myGarden.FindPlots();
        int sum = 0;
        foreach (var plot in plots)
        {
            var perimeter = myGarden.CalculatePlotPerimeter(plot.Item2, plot.Item1);
            var area = plot.Item2.Count;
            var price = perimeter * area;
            sum += price;
        }
        Assert.Equal(1930, sum);
    }


    [Fact]
    public void TestExpandedGrid()
    {
        var myGarden = new Garden(smallTestInput);
        myGarden.PrepareGardenForStep2();
        Assert.Equal(['A','A','A','A','A','A','A','A'],myGarden.GardenMap[0]);
        Assert.Equal(['A','A','A','A','A','A','A','A'],myGarden.GardenMap[1]);
        Assert.Equal(['B','B','B','B','C','C','D','D'],myGarden.GardenMap[2]);
        Assert.Equal(['B','B','B','B','C','C','D','D'],myGarden.GardenMap[3]);
        Assert.Equal(['B','B','B','B','C','C','C','C'],myGarden.GardenMap[4]);
        Assert.Equal(['B','B','B','B','C','C','C','C'],myGarden.GardenMap[5]);
        Assert.Equal(['E','E','E','E','E','E','C','C'],myGarden.GardenMap[6]);
        Assert.Equal(['E','E','E','E','E','E','C','C'],myGarden.GardenMap[7]);
    }

    [Theory()]
    [InlineData(0,0,true)]
    [InlineData(0,1,false)]
    [InlineData(0,2,false)]
    [InlineData(0,3,false)]
    [InlineData(0,4,false)]
    [InlineData(0,5,false)]
    [InlineData(0,6,false)]
    [InlineData(0,7,true)]
    [InlineData(1,0,true)]
    [InlineData(1,1,false)]
    [InlineData(1,2,false)]
    [InlineData(1,3,false)]
    [InlineData(1,4,false)]
    [InlineData(1,5,false)]
    [InlineData(1,6,false)]
    [InlineData(1,7,true)]
    [InlineData(2,0,true)]
    [InlineData(2,1,false)]
    [InlineData(2,2,false)]
    [InlineData(2,3,true)]
    [InlineData(2,4,true)]
    [InlineData(2,5,true)]
    [InlineData(2,6,true)]
    [InlineData(2,7,true)]
    [InlineData(3,0,false)]
    [InlineData(3,1,false)]
    [InlineData(3,2,false)]
    [InlineData(3,3,false)]
    [InlineData(3,4,false)]
    [InlineData(3,5,false)]
    [InlineData(3,6,true)]
    [InlineData(3,7,true)]
    [InlineData(4,0,false)]
    [InlineData(4,1,false)]
    [InlineData(4,2,false)]
    [InlineData(4,3,false)]
    [InlineData(4,4,false)]
    [InlineData(4,5,true)]
    [InlineData(4,6,false)]
    [InlineData(4,7,true)]
    [InlineData(5,0,true)]
    [InlineData(5,1,false)]
    [InlineData(5,2,false)]
    [InlineData(5,3,true)]
    [InlineData(5,4,true)]
    [InlineData(5,5,false)]
    [InlineData(5,6,true)]
    [InlineData(5,7,false)]
    [InlineData(6,0,true)]
    [InlineData(6,1,false)]
    [InlineData(6,2,false)]
    [InlineData(6,3,false)]
    [InlineData(6,4,false)]
    [InlineData(6,5,true)]
    [InlineData(6,6,false)]
    [InlineData(6,7,false)]
    [InlineData(7,0,true)]
    [InlineData(7,1,false)]
    [InlineData(7,2,false)]
    [InlineData(7,3,false)]
    [InlineData(7,4,false)]
    [InlineData(7,5,true)]
    [InlineData(7,6,true)]
    [InlineData(7,7,true)]
    
    public void TestPlotCorners(int y, int x, bool expectedResult)
    {
        var myGarden = new Garden(smallTestInput);
        myGarden.PrepareGardenForStep2();
        var result = myGarden.IsPlotCorner(y,x);
        Assert.Equal(expectedResult, result);
    }

    [Fact()]
    public void TestPlotCornerCounts()
    {
        var myGarden = new Garden(smallTestInput);
        myGarden.PrepareGardenForStep2();
        var plots = myGarden.FindPlots();
        var cornersForA = myGarden.CalculatePlotCorners(plots[0].coordinates);
        var cornersForB = myGarden.CalculatePlotCorners(plots[1].coordinates);
        var cornersForC = myGarden.CalculatePlotCorners(plots[2].coordinates);
        var cornersForD = myGarden.CalculatePlotCorners(plots[3].coordinates);
        var cornersForE = myGarden.CalculatePlotCorners(plots[4].coordinates);
        
        Assert.Equal(4,cornersForA);
        Assert.Equal(4,cornersForB);
        Assert.Equal(8,cornersForC);
        Assert.Equal(4,cornersForD);
        Assert.Equal(4,cornersForE);
    }
    
    
    [Fact]
    public void TestTotalPricePart2MediumInput()
    {
        
        var myGarden = new Garden(mediumTestInput);
        var originalPlots = myGarden.FindPlots();
        myGarden.PrepareGardenForStep2();
        var expandedPlots = myGarden.FindPlots();
        int sum = 0;
        for(int i = 0; i < expandedPlots.Count; i++)
        {
            var corners = myGarden.CalculatePlotCorners(expandedPlots[i].Item2);
            var area = originalPlots[i].Item2.Count;
            var price = corners * area;
            sum += price;
        }
        Assert.Equal(436, sum);
    }
    
    [Fact]
    public void TestTotalPricePart2EShapedInput()
    {
        
        var myGarden = new Garden(eShapedTestInput);
        var originalPlots = myGarden.FindPlots();
        myGarden.PrepareGardenForStep2();
        var expandedPlots = myGarden.FindPlots();
        int sum = 0;
        for(int i = 0; i < expandedPlots.Count; i++)
        {
            var corners = myGarden.CalculatePlotCorners(expandedPlots[i].Item2);
            var area = originalPlots[i].Item2.Count;
            var price = corners * area;
            sum += price;
        }
        Assert.Equal(236, sum);
    }
    
    [Fact]
    public void TestTotalPricePart2SneakyInput()
    {
        
        var myGarden = new Garden(sneakyTestInput);
        var originalPlots = myGarden.FindPlots();
        myGarden.PrepareGardenForStep2();
        var expandedPlots = myGarden.FindPlots();
        int sum = 0;
        for(int i = 0; i < expandedPlots.Count; i++)
        {
            var corners = myGarden.CalculatePlotCorners(expandedPlots[i].Item2);
            var area = originalPlots[i].Item2.Count;
            var price = corners * area;
            sum += price;
        }
        Assert.Equal(368, sum);
    }
    
    [Fact]
    public void TestTotalPricePart2()
    {
        var myGarden = new Garden(largeTestInput);
        var originalPlots = myGarden.FindPlots();
        myGarden.PrepareGardenForStep2();
        var expandedPlots = myGarden.FindPlots();
        int sum = 0;
        for(int i = 0; i < expandedPlots.Count; i++)
        {
            var corners = myGarden.CalculatePlotCorners(expandedPlots[i].Item2);
            var area = originalPlots[i].Item2.Count;
            var price = corners * area;
            sum += price;
        }
        Assert.Equal(1206, sum);
    }
}