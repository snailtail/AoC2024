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

    

    public bool IsValid => isValid(_data,false);
    public bool IsAlmostValid => isValid(_data,true);

    private bool isValid(int[] arr, bool allowErrors = false)
    {    
        bool isIncreasing = arr[1] > arr[0];
        for(int i = 0; i < arr.Length-1; i++)
        {

            // Check if increasing or decreasing breaks
            if(
                (isIncreasing && arr[i+1] < arr[i]) 
                || 
                (!isIncreasing && arr[i+1] > arr[i]) 
                || 
                arr[i+1] == arr[i]
                ||
                (Math.Abs(arr[i+1] - arr[i]) < 1 || Math.Abs(arr[i+1] - arr[i]) > 3)
            )
            {
                // so we have an error
                // for step 2 - we can check if this array would be valid if we remove any value
                if(allowErrors){
                for(int ii = 0; ii < arr.Length; ii++)
                {
                    int[] newArr = arr.Where((_, index) => index != ii).ToArray();
                    bool isAlmostValid = isValid(newArr,false);
                    if(isAlmostValid)
                    {
                        return true;
                    }
                }

                }
                return false;
            }

            
        }

        return true;
    }    
}