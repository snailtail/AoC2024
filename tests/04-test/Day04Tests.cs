
namespace _04_test;

public class WordSearcher
{
    public char[][] CharGrid { get; private set; }

    public WordSearcher(string[] input)
    {
        CharGrid = new char[input.Length][];
        
        for(int i = 0; i < input.Length; i++)
        {   
            CharGrid[i] = input[i].ToCharArray();
        }
    }
    public int SolvePart1()
    {
        const string wordToSearchFor = "XMAS";
        var resultStep1 = 0;
        for (var row = 0; row < CharGrid.Length; row++)
        {
            for (var col = 0; col < CharGrid[row].Length; col++)
            {
                if (CharGrid[row][col] == 'X')
                {
                    resultStep1 += Search(row,col,0,1, wordToSearchFor);
                    resultStep1 += Search(row,col,0,-1, wordToSearchFor);
                    resultStep1 += Search(row,col,1,0, wordToSearchFor);
                    resultStep1 += Search(row,col,-1,0, wordToSearchFor);
                    resultStep1 += Search(row,col,1,1, wordToSearchFor);
                    resultStep1 += Search(row,col,-1,1, wordToSearchFor);
                    resultStep1 += Search(row,col,1,-1, wordToSearchFor);
                    resultStep1 += Search(row,col,-1,-1, wordToSearchFor);
                }
            }
        }

        return resultStep1;
    }

    public int SolvePart2()
    {
        HashSet<string> xmasCenterPoints = new(); // Store the center point coordinate of every confirmed X-MAS
        for (var row = 0; row < CharGrid.Length; row++)
        {
            var aIndices = CharGrid[row]
                .Select((c, index) => new { Char = c, Index = index })
                .Where(x => x.Char == 'A')
                .Select(x => x.Index)
                .ToArray();
            foreach (var col in aIndices)
            {
                if(IsValidCenterPoint(row,col)){
                    xmasCenterPoints.Add($"{row}x{col}");
                }
            }
        }

        return xmasCenterPoints.Count;
    }
    
    private bool IsValidCenterPoint(int r, int c)
    {
        // If any corner is outside the grid, return false and go on with life.
        if (r <= 0 || c <= 0 || r >= CharGrid.Length - 1 || c >= CharGrid[r].Length - 1 || r + 1 >= CharGrid.Length || c > CharGrid[r].Length)
        {
            return false;
        }
        
        (int r,int c) tlCorner = (r - 1, c - 1); // Top left corner
        (int r, int c) trCorner= (r - 1, c + 1); // Top right corner
        (int r, int c) blCorner = (r + 1, c - 1); // Bottom left corner
        (int r, int c) brCorner = (r + 1, c + 1); // Bottom right corner
        
        // Fetch the chars in the corners
        var tlChar = CharGrid[tlCorner.r][tlCorner.c];
        var brChar = CharGrid[brCorner.r][brCorner.c];
        var blChar = CharGrid[blCorner.r][blCorner.c];
        var trChar = CharGrid[trCorner.r][trCorner.c];
        
        // Check for valid combos of chars relative to the A in the center point.
        if(((tlChar == 'M' && brChar=='S') || (tlChar == 'S' && brChar == 'M')) 
           && 
           ((blChar == 'M' && trChar == 'S') || (blChar == 'S' && trChar == 'M'))
           )
        {
            return true;
        }
        return false;
    }

    private int Search(int r, int c, int rDir, int cDir, string word, bool allowWrapAround = false)
    {
        int rowindex = 0;
        int colindex = 0;
        
        foreach (char letter in word)
        {
            int row;
            int col;
            if (allowWrapAround)
            {
                row = (r + rowindex + CharGrid.Length) % CharGrid.Length; // Ensure row wraps correctly
                col = (c + colindex + CharGrid[r].Length) % CharGrid[r].Length; // Ensure col wraps correctly
            }
            else
            {
                row = r + rowindex;
                if (row < 0 || row >= CharGrid.Length)
                {
                    return 0;
                }
                col = c + colindex;
                if (col < 0 || col >= CharGrid[row].Length)
                {
                    return 0;
                }
            }
               
            if (CharGrid[row][col] != letter)
            {
                return 0;
            }
            
            rowindex += rDir; // Vertical movement (up/down)
            colindex += cDir; // Horizontal movement (left/right)
        }

        return 1;
    }
    
}

public class Day04Tests
{
    string[] _testInput = @"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX".Split().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();


    [Fact]
    public void Part1_Check_Size_Of_CharGrid()
    {
        var mySearcher = new WordSearcher(_testInput);
        Assert.Equal(_testInput.Length, mySearcher.CharGrid.Length);
    }
    [Fact]
    public void Part1_TestInput_ResultShouldBeEqualTo18()
    {
        
        var mySearcher = new WordSearcher(_testInput);
        int result = mySearcher.SolvePart1();
        Assert.Equal(18, result);
    }

    [Fact]
    public void Part2_TestInput_ResultShouldBeEqualTo9()
    {
        var mySearcher = new WordSearcher(_testInput);
        int result = mySearcher.SolvePart2();
        Assert.Equal(9,result);
    }
}