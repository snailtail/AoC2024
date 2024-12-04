namespace _04.Model;

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
    public int SolveStep1()
    {
        string wordToSearchFor = "XMAS";
        int resultStep1 = 0;
        for (int row = 0; row < CharGrid.Length; row++)
        {
            for (int col = 0; col < CharGrid[row].Length; col++)
            {
                if (CharGrid[row][col] == 'X')
                {
                    Console.WriteLine($"FOUND X @ Row: {row}, Col: {col}");
                    
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

    private int Search(int r, int c, int rdir, int cdir, string word, bool allowWrapAround = false)
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
            
            rowindex += rdir; // Vertical movement (up/down)
            colindex += cdir; // Horizontal movement (left/right)
        }
        Console.WriteLine($"Search found a hit! @ {r}, {c}");
        return 1;
    }
    
}