namespace _09_test;

public class Defragmenter
{
    private readonly string _input;
    private string[] _sectors;
    private string _diskData;
    public string DiskData => _diskData;
    public Dictionary<int,(int,int)> fileAllocationTable = new (); //fileID -> (startIndex, Len)
    public Dictionary<int,(int,int)> freeSpaceAllocationTable = new (); //someID -> (startIndex, Len)
    

    public Defragmenter(string input)
    {
        _input = input;
        int[] nums = _input.Select(c => int.Parse(c.ToString())).ToArray();
        int diskLength = nums.Sum();
        _sectors = new string[diskLength];
        bool freeSpace = false; // first char is a file
        int fileId = 0;
        int sectorIndex = 0;
        foreach (char c in input)
        {
            int size = int.Parse(c.ToString());
            string sectorValue;
            if (freeSpace)
            {
                sectorValue = ".";
                freeSpaceAllocationTable.Add(-(fileId+1) ,(sectorIndex, size));
                fileId++;
            }
            else
            {
                sectorValue = fileId.ToString();
                fileAllocationTable.Add(fileId,(sectorIndex, size));
            }
            for (int i = 0; i < size; i++)
            {
                _sectors[sectorIndex++] = sectorValue;
            }

            freeSpace = !freeSpace;

        }
        _diskData = string.Join("",_sectors);
    }

    public bool IsFragmented => isFragmented();

    private int findFirstFreeSpace()
    {
        for (int i = 0; i < _sectors.Length; i++)
        {
            if (_sectors[i] == ".")
            {
                return i;
            }
        }

        return -1;
    }

    private int findLastNonFreeSector()
    {
        for (int i = _sectors.Length - 1; i >= 0; i--)
        {
            if (_sectors[i] != ".")
            {
                return i;
            }
        }

        return -1;
    }
    private bool isFragmented()
    {

        int firstFreeSpace = findFirstFreeSpace();
        for (int i = firstFreeSpace + 1; i < _sectors.Length; i++)
        {
            if (_sectors[i] != ".")
            {
                return true;
            }
        }
        return false;
    }

    public void Defragment()
    {
        while (isFragmented())
        {
            int freeSpace = findFirstFreeSpace();
            int sectorToMove = findLastNonFreeSector();
            _sectors[freeSpace]=_sectors[sectorToMove];
            _sectors[sectorToMove] = ".";
        }
        _diskData = string.Join("",_sectors);
    }

    public void MoveFilesForPart2()
    {
        var FilesToMove = fileAllocationTable.OrderByDescending(kvp => kvp.Key).ToList();
        foreach (var kvp in FilesToMove)
        {
            int fileId = kvp.Key;
            int sectorIndex = kvp.Value.Item1;
            int size = kvp.Value.Item2;
            int? firstFreeSpaceId = freeSpaceAllocationTable
                .OrderByDescending(kvp => kvp.Key)
                .Where(fsat => fsat.Value.Item2 >= size)
                .Select(f => (int?)f.Key) // Cast to nullable int
                .FirstOrDefault();
            if (firstFreeSpaceId.HasValue)
            {
                // get index for this free-space-id
                var freeSpaceIndex = freeSpaceAllocationTable[firstFreeSpaceId.Value].Item1;
                if (freeSpaceIndex > sectorIndex)
                {
                    continue;
                }
                for (int offset = 0; offset < size; offset++)
                {
                    _sectors[freeSpaceIndex + offset] = _sectors[sectorIndex + offset];
                    _sectors[sectorIndex + offset] = ".";
                }
                //decrease the free space, since it's now being used.
                var freeSpaceItem = freeSpaceAllocationTable[firstFreeSpaceId.Value];
                freeSpaceItem.Item2 -= size;
                if (freeSpaceItem.Item2 <= 0)
                {
                    //if size == 0 remove the entry
                    freeSpaceAllocationTable.Remove(firstFreeSpaceId.Value);
                }
                else
                {
                    //if size still larger than 0, increase the index by size so the "pointer" points to the correct starting index.
                    freeSpaceItem.Item1 += size;
                    freeSpaceAllocationTable[firstFreeSpaceId.Value] = freeSpaceItem;
                }
            }
        }
        _diskData = string.Join("",_sectors);
    }

    public long CheckSum => GetCheckSum();
    public long CheckSumPart2 => GetCheckSum(true);

    private long GetCheckSum(bool ignoreFragmentation=false)
    {
        if (!ignoreFragmentation && isFragmented())
        {
            throw new InvalidOperationException("Checksum can't be calculated for a fragmented disk!");
        }
        
        long sum = 0;
        int dataLength = _sectors.Length;
        for (int i = 0; i < dataLength; i++)
        {
            if (_sectors[i] != ".")
            {
                sum += i * int.Parse(_sectors[i]);
            }
        }
        return sum;
    }

}

public class DefragmenterTest
{
    [Theory]
    [InlineData("12345","0..111....22222")]
    [InlineData("2333133121414131402","00...111...2...333.44.5555.6666.777.888899")]
    [InlineData("233313312141413140211","00...111...2...333.44.5555.6666.777.888899.10")]
    public void Test_DiskData(string input, string expectedData)
    {
        var defragmenter = new Defragmenter(input);
        var result = string.Join("",defragmenter.DiskData);
        Assert.Equal(expectedData, result);
    }
    
    [Theory]
    [InlineData("12345",15)]
    [InlineData("2333133121414131402",42)]
    public void Test_Disk_Length(string input, int expectedLength)
    {
        var defragmenter = new Defragmenter(input);
        var result = defragmenter.DiskData.Length;
        Assert.Equal(expectedLength, result);
    }
    
    [Theory]
    [InlineData("12345",true)]
    [InlineData("2333133121414131402",true)]
    public void Test_Disk_Is_Defragmented(string input, bool expectedStatus)
    {
        var defragmenter = new Defragmenter(input);
        var result = defragmenter.IsFragmented;
        Assert.Equal(expectedStatus, result);
    }
    
    
    [Theory]
    [InlineData("12345","022111222......")]
    [InlineData("2333133121414131402","0099811188827773336446555566..............")]
    [InlineData("233313312141413140211","001099111888287733374465555666...............")]
    public void Test_Defragment_Disk(string input, string expectedResult)
    {
        var defragmenter = new Defragmenter(input);
        defragmenter.Defragment();
        var result = new string(defragmenter.DiskData);
        Assert.Equal(expectedResult, result);
    }
    
    [Theory]
    [InlineData("2333133121414131402",1928)]
    public void Test_Defragment_Disk_And_Calculate_Checksum(string input, int expectedResult)
    {
        var defragmenter = new Defragmenter(input);
        defragmenter.Defragment();
        var result = defragmenter.CheckSum;
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Test_FileMovementForPart2()
    {
        var input = "2333133121414131402";
        var defragmenter = new Defragmenter(input);
        defragmenter.MoveFilesForPart2();
        var result = defragmenter.CheckSumPart2;
        Assert.Equal(2858, result);
    }
}
public class Day09Tests
{

    private string testInput = "2333133121414131402";
        
    [Fact]
    public void Part1()
    {   
        var defragmenter = new Defragmenter(testInput);
        defragmenter.Defragment();
        var result = defragmenter.CheckSum;
        Assert.Equal(1928, result);
    }
    
    [Fact]
    public void Part2()
    {
        var defragmenter = new Defragmenter(testInput);
        defragmenter.MoveFilesForPart2();
        var result = defragmenter.CheckSumPart2;
        Assert.Equal(2858, result);
    }
}