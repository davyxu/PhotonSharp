

namespace Photon
{
    class InstructionBinaryMath : Instruction
    {
        public override bool Execute(Command cmd)
        {
            var a = vm.DataStack.Pop().CastNumber();
            var b = vm.DataStack.Pop().CastNumber();

            float c;

            // 栈顺序是反的, 需要倒过来
            var result = ExecuteOn2Value(cmd, b, a, out c );

            vm.DataStack.Push(new ValueNumber(c));

            return result;
        }


        public virtual bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = 0;

            return true;
        }

    }

    class InstructionUnaryMath : Instruction
    {
        public override bool Execute(Command cmd)
        {
            var a = vm.DataStack.Pop().CastNumber();            

            float c;

            // 栈顺序是反的, 需要倒过来
            var result = ExecuteOnValue(cmd, a, out c);

            vm.DataStack.Push(new ValueNumber(c));

            return result;
        }


        public virtual bool ExecuteOnValue(Command cmd, float a, out float x)
        {
            x = 0;

            return true;
        }
    }

    [Instruction(Cmd = Opcode.MINUS)]
    class CmdMinus : InstructionUnaryMath
    {
        public override bool ExecuteOnValue(Command cmd, float a, out float x)
        {
            x = -a;

            return true;
        }
    }




    [Instruction(Cmd = Opcode.ADD)]
    class CmdAdd : InstructionBinaryMath
    {
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a + b;

            return true;
        }
    }

    [Instruction(Cmd = Opcode.SUB)]
    class CmdSub : InstructionBinaryMath
    {
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a - b;

            return true;
        }
    }

    [Instruction(Cmd = Opcode.MUL)]
    class CmdMul : InstructionBinaryMath
    {
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a * b ;

            return true;
        }
    }

    [Instruction(Cmd = Opcode.DIV)]
    class CmdDiv : InstructionBinaryMath
    {
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a / b ;

            return true;
        }
    }

    [Instruction(Cmd = Opcode.GT)]
    class CmdGT : InstructionBinaryMath
    {
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a > b ? 1 : 0;

            return true;
        }
    }

    [Instruction(Cmd = Opcode.GE)]
    class CmdGE : InstructionBinaryMath
    {            
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a >= b ? 1 : 0;

            return true;
        }

    }

    [Instruction(Cmd = Opcode.LT)]
    class CmdLT : InstructionBinaryMath
    {
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a < b ? 1 : 0;

            return true;
        }
    }

    [Instruction(Cmd = Opcode.LE)]
    class CmdLE : InstructionBinaryMath
    {
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a <= b ? 1 : 0;

            return true;
        }

    }

    [Instruction(Cmd = Opcode.EQ)]
    class CmdEQ : InstructionBinaryMath
    {
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a == b ? 1 : 0;

            return true;
        }
    }

    [Instruction(Cmd = Opcode.NE)]
    class CmdNE : InstructionBinaryMath
    {
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a != b ? 1 : 0;

            return true;
        }
    }



}
