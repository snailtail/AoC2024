public class unusualData
{
    private int[] _data;
    public int[] Data
    {
        get { return _data; }
        set { _data = value; }
    }

    public unusualData(int[] data)
    {
        _data = data;
    }

    public bool IsIncreasing => _data[1] > _data[0];

    public bool IsValid => isValid();

    private bool isValid()
    {    
        for(int i = 0; i < _data.Length-1; i++)
        {

            // Check if increasing or decreasing breaks
            if((this.IsIncreasing && _data[i+1] < _data[i]) || (!this.IsIncreasing && _data[i+1] > _data[i]))
            {
                // 
                return false;
            }

            // Check if gaps between elements is larger than allowed
            int gap = Math.Abs(_data[i+1] - _data[i]);
            if(gap < 1 || gap > 3)
            {
                return false;
            }
        }

        return true;
    }

    

    
}