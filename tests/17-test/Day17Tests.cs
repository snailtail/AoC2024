namespace _17_test;

public enum OpCodes{
    ADV = 0,
    BXL = 1,
    BST = 2,
    JNZ = 3,
    BXC = 4,
    OUT = 5,
    BDV = 6,
    CDV = 7,
}
public class ThreeBitComputer(string[] input)
{
    public readonly int[] Program = input.First(l=> l.StartsWith("Program:")).Replace("Program: ","").Split(',').Select(int.Parse).ToArray();
    // "The computer also has three registers named A, B, and C, but these registers aren't limited to 3 bits and can instead hold any integer."
    public int A { get; set; } = int.Parse(input.First(l=> l.StartsWith("Register A:")).Replace("Register A: ",""));
    public int B { get; set; } = int.Parse(input.First(l=> l.StartsWith("Register B:")).Replace("Register B: ",""));
    public int C { get; set; } = int.Parse(input.First(l=> l.StartsWith("Register C:")).Replace("Register C: ",""));
    
    // A number called the instruction pointer identifies the position in the program from which the next opcode will be read; it starts at 0, pointing at the first 3-bit number in the program. Except for jump instructions, the instruction pointer increases by 2 after each instruction is processed (to move past the instruction's opcode and its operand). If the computer tries to read an opcode past the end of the program, it instead halts.
    public int InstructionPointer { get; set; } = 0;

    private List<int> _out = [];
    public string OUTPUT => string.Join(',', _out);
    
    /*
     * This seems to be a 3-bit computer:
     *  its program is a list of 3-bit numbers (0 through 7), like 0,1,2,3.
     *
     * The computer also has three registers named A, B, and C,
     *  but these registers aren't limited to 3 bits and can instead hold any integer.
     *
     * The computer knows eight instructions, each identified by a 3-bit number (called the instruction's opcode).
     * Each instruction also reads the 3-bit number after it as an input; this is called its operand.

     */
    public (int OpCode, int Operand) GetInstruction()
    {
        if (InstructionPointer >= Program.Length)
            throw new IndexOutOfRangeException();
        
        int opcode = Program[InstructionPointer];
        int operand = Program[InstructionPointer+1];

        return (opcode, operand);
    }

    public void Run(bool part1 = true)
    {
        while (InstructionPointer < Program.Length)
        {
            _= ProcessNextInstruction();
        }
        Console.WriteLine(OUTPUT);
    }
    public bool ProcessNextInstruction()
    {
        // one could use the bools from the opcode-methods to increase IP by 2 or not perhaps?
        
        if (InstructionPointer >= Program.Length)
            throw new IndexOutOfRangeException();

        (int opCode, int operand) = GetInstruction();
        
        if (opCode == (int)OpCodes.ADV)
            return ADV(operand);

        if (opCode == (int)OpCodes.BXL)
            return BXL(operand);
        
        if (opCode == (int)OpCodes.BST)
            return BST(operand);
        
        if (opCode == (int)OpCodes.JNZ)
            return JNZ(operand);

        if (opCode == (int)OpCodes.BXC)
            return BXC(operand);

        if(opCode == (int)OpCodes.OUT)
            return OUT(operand);
        
        if(opCode == (int)OpCodes.BDV)
            return BDV(operand);
        
        if(opCode == (int)OpCodes.CDV)
            return CDV(operand);
        

        return false;
    }

    private int GetComboOperandValue(int operand)
    {
        if (operand is <= 3 and >= 0)
        {
            return 0 + operand;   
        }

        if (operand == 4)
        {
            return 0 + A;
        }
        
        if (operand == 5)
        {
            return 0 + B;
        }
        
        if (operand == 6)
        {
            return 0 + C;
        }
        else if (operand == 7)
        {
            throw new NotImplementedException();
        }
        
        return int.MinValue;
        
    }

    private bool CDV(int operand)
    {
        // // The cdv instruction (opcode 7) works exactly like the adv instruction except that the result is stored in the C register.
        // (The numerator is still read from the A register.)
        int numerator = A;
        int denominator = int.MinValue;
        int operandValue = GetComboOperandValue(operand);
        denominator = (int)Math.Pow(2, operandValue);
        
        C = numerator / denominator;
        InstructionPointer+=2;
        
        return true;
    }
    
    private bool BDV(int operand)
    {
        // The bdv instruction (opcode 6) works exactly like the adv instruction except that the result is stored in the B register.
        // (The numerator is still read from the A register.)
        int numerator = A;
        int denominator = int.MinValue;
        int operandValue = GetComboOperandValue(operand);
        denominator = (int)Math.Pow(2, operandValue);
        
        B = numerator / denominator;
        InstructionPointer+=2;
        
        return true;
    }
    private bool OUT(int operand)
    {
        // The out instruction (opcode 5) calculates the value of its combo operand modulo 8,
        // then outputs that value.
        // (If a program outputs multiple values, they are separated by commas.)
        var value = GetComboOperandValue(operand);
        value = value % 8;
        this._out.Add(value);
        InstructionPointer += 2;
        return true;
    }
    private bool BXC(int operand)
    {
        // The bxc instruction (opcode 4) calculates the bitwise XOR of register B and register C,
        // then stores the result in register B.
        // (For legacy reasons, this instruction reads an operand but ignores it.)
        int value1 = 0 + B;
        int value2 = 0 + C;
        B = value1 ^ value2;
        InstructionPointer += 2;
        return true;
    }
    private bool JNZ(int operand)
    {
        // The jnz instruction (opcode 3) does nothing if the A register is 0.
        // However, if the A register is not zero, it jumps by setting the instruction pointer
        // to the value of its literal operand;
        // if this instruction jumps, the instruction pointer is **not** increased by 2 after this instruction.
        
        if (A == 0)
        {
            InstructionPointer += 2;
        }
        else
        {
            InstructionPointer = operand;
        }

        return true;
    }
    private bool BST(int operand)
    {
        // The bst instruction (opcode 2) calculates
        //  the value of its combo operand modulo 8
        // (thereby keeping only its lowest 3 bits),
        // then writes that value to the B register.
        var operandValue = GetComboOperandValue(operand);
        operandValue = operandValue % 8;
        B = operandValue;
        InstructionPointer+=2;
        return true;
    }
    private bool BXL(int operand)
    {
        // The bxl instruction (opcode 1) calculates the bitwise XOR of register B and the instruction's literal operand, then stores the result in register B.
        // If register B contains 29, the program 1,7 would set register B to 26.
        var xor = B ^ operand;
        B = xor;
        InstructionPointer+=2;
        return true;
    }
    
    private bool ADV(int operand)
    {
        // The adv instruction (opcode 0) performs division.
        // The numerator is the value in the A register.
        // The denominator is found by raising 2 to the power of the instruction's combo operand.
        // (So, an operand of 2 would divide A by 4 (2^2);
        // an operand of 5 would divide A by 2^B.)
        // The result of the division operation is truncated to an integer and then written to the A register.
        int numerator = A;
        int denominator = int.MinValue;
        int operandValue = GetComboOperandValue(operand);
        denominator = (int)Math.Pow(2, operandValue);
        
        A = numerator / denominator;
        InstructionPointer+=2;
        
        return true;
    }
}

public class Day17Tests
{
    private string[] exampleInput = [
        "Register A: 729",
        "Register B: 0",
        "Register C: 0",
        "",
        "Program: 0,1,5,4,3,0"
        ];
    
    
    [Fact]
    public void TestParsing()
    {
        var myComputer = new ThreeBitComputer(exampleInput);
        Assert.Equal(6, myComputer.Program.Length);
        Assert.Equal(729, myComputer.A);
        Assert.Equal(0, myComputer.B);
        Assert.Equal(0, myComputer.C);
        Assert.Equal(0, myComputer.InstructionPointer);
    }
    
    [Fact]
    public void TestGetInstructions()
    {
        var myComputer = new ThreeBitComputer(exampleInput);
        var result = myComputer.GetInstruction();
        Assert.Equal(0, myComputer.InstructionPointer);
        Assert.Equal(0, result.OpCode);
        Assert.Equal(1, result.Operand);

        myComputer.InstructionPointer += 2;
        
        result = myComputer.GetInstruction();
        Assert.Equal(2, myComputer.InstructionPointer);
        Assert.Equal(5, result.OpCode);
        Assert.Equal(4, result.Operand);
        
        myComputer.InstructionPointer += 2;
        
        result = myComputer.GetInstruction();
        Assert.Equal(4, myComputer.InstructionPointer);
        Assert.Equal(3, result.OpCode);
        Assert.Equal(0, result.Operand);
        
        myComputer.InstructionPointer += 2;
        
        Assert.Equal(6, myComputer.InstructionPointer);
        Assert.Throws<IndexOutOfRangeException>(() => myComputer.GetInstruction());
    }
    
    
    [Fact]
    public void TestProcessNextInstruction()
    {
        var myComputer = new ThreeBitComputer(exampleInput);
        myComputer.ProcessNextInstruction();
        Assert.Equal(2, myComputer.InstructionPointer);
        Assert.Equal(364, myComputer.A);
    }
    
    
    [Fact]
    public void TestProcessAdvInstruction()
    {
        var myComputer = new ThreeBitComputer(exampleInput);
        myComputer.A=729;
        myComputer.Program[0] = 0;
        myComputer.Program[1] = 1;
        myComputer.ProcessNextInstruction();
        Assert.Equal(364, myComputer.A);
        Assert.Equal(2,myComputer.InstructionPointer);
    }
    
    [Fact]
    public void TestProcessBxlInstruction()
    {
        var myComputer = new ThreeBitComputer(exampleInput);
        // If register B contains 29, the program 1,7 would set register B to 26.
        myComputer.B=29;
        myComputer.Program[0] = 1;
        myComputer.Program[1] = 7;
        myComputer.ProcessNextInstruction();
        Assert.Equal(26, myComputer.B);
        Assert.Equal(2,myComputer.InstructionPointer);
    }
    
    [Fact]
    public void TestProcessBstInstruction()
    {
        var myComputer = new ThreeBitComputer(exampleInput);
        // If register C contains 9, the program 2,6 would set register B to 1.
        myComputer.C=9;
        myComputer.Program[0] = 2;
        myComputer.Program[1] = 6;
        myComputer.ProcessNextInstruction();
        Assert.Equal(1, myComputer.B);
        Assert.Equal(2,myComputer.InstructionPointer);
    }
    
    [Fact]
    public void TestProcessJnzInstruction()
    {
        var myComputer = new ThreeBitComputer(exampleInput);
        // JNZ will do nothing if a = 0 (only increase InstructionPointer)
        myComputer.A = 0;
        myComputer.Program[0] = 3;
        myComputer.Program[1] = 6;
        myComputer.ProcessNextInstruction();
        Assert.Equal(2,myComputer.InstructionPointer);
        
        // reset and test with A != 0 - should jump instructionpointer to literal operand value
        myComputer.A = 3;
        myComputer.InstructionPointer=0;
        myComputer.Program[0] = 3;
        myComputer.Program[1] = 6;
        myComputer.ProcessNextInstruction();
        Assert.Equal(6,myComputer.InstructionPointer);
    }
    
    [Fact]
    public void TestProcessBxcInstruction()
    {
        var myComputer = new ThreeBitComputer(exampleInput);
        // If register B contains 2024 and register C contains 43690, the program 4,0 would set register B to 44354.
        myComputer.B=2024;
        myComputer.C=43690;
        myComputer.Program[0] = 4;
        myComputer.Program[1] = 0;
        myComputer.ProcessNextInstruction();
        Assert.Equal(44354, myComputer.B);
        Assert.Equal(2,myComputer.InstructionPointer);
    }
    
    [Fact]
    public void TestProcessOutInstruction()
    {
        var myComputer = new ThreeBitComputer(exampleInput);
        // If register A contains 10, the program 5,0,5,1,5,4 would output 0,1,2.
        myComputer.A=10;
        myComputer.Program[0] = 5;
        myComputer.Program[1] = 0;
        myComputer.Program[2] = 5;
        myComputer.Program[3] = 1;
        myComputer.Program[4] = 5;
        myComputer.Program[5] = 4;
        myComputer.ProcessNextInstruction();
        Assert.Equal("0", myComputer.OUTPUT);
        Assert.Equal(2,myComputer.InstructionPointer);
        
        myComputer.ProcessNextInstruction();
        Assert.Equal("0,1", myComputer.OUTPUT);
        Assert.Equal(4,myComputer.InstructionPointer);
        
        myComputer.ProcessNextInstruction();
        Assert.Equal("0,1,2", myComputer.OUTPUT);
        Assert.Equal(6,myComputer.InstructionPointer);
    }
    
    
    [Fact]
    public void TestProcessBdvInstruction()
    {
        var myComputer = new ThreeBitComputer(exampleInput);
        myComputer.A=729;
        myComputer.B = 0;
        myComputer.Program[0] = 6;
        myComputer.Program[1] = 1;
        myComputer.ProcessNextInstruction();
        Assert.Equal(364, myComputer.B);
        Assert.Equal(2,myComputer.InstructionPointer);
    }
    
    [Fact]
    public void TestProcessCdvInstruction()
    {
        var myComputer = new ThreeBitComputer(exampleInput);
        myComputer.A=729;
        myComputer.C = 0;
        myComputer.Program[0] = 7;
        myComputer.Program[1] = 1;
        myComputer.ProcessNextInstruction();
        Assert.Equal(364, myComputer.C);
        Assert.Equal(2,myComputer.InstructionPointer);
    }

    [Fact]
    public void TestRunPart1Example()
    {
        var myComputer = new ThreeBitComputer(exampleInput);
        myComputer.Run();
        Assert.Equal("4,6,3,5,6,3,5,2,1,0",myComputer.OUTPUT);
    }
}
