using Photon.Model;

namespace Photon.VM
{
    class InstructionMath : Instruction
    {
        public override bool Execute(Command cmd)
        {
            var a = vm.Stack.Pop().CastNumber();
            var b = vm.Stack.Pop().CastNumber();

            float c;

            // 栈顺序是反的, 需要倒过来
            var result = ExecuteOn2Value(cmd, b, a, out c );

            vm.Stack.Push(new ValueNumber(c));

            return result;
        }


        public virtual bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = 0;

            return true;
        }

        public override string Print(Command cmd)
        {
            return string.Format("A: {0}, B: {1}", vm.Stack.ValueToString(-1), vm.Stack.ValueToString(-2));
        }
    }


    [Instruction(Cmd = Opcode.Add)]
    class CmdAdd : InstructionMath
    {
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a + b;

            return true;
        }
    }

    [Instruction(Cmd = Opcode.Sub)]
    class CmdSub : InstructionMath
    {
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a - b;

            return true;
        }
    }

    [Instruction(Cmd = Opcode.Mul)]
    class CmdMul : InstructionMath
    {
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a * b ;

            return true;
        }
    }

    [Instruction(Cmd = Opcode.Div)]
    class CmdDiv : InstructionMath
    {
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a / b ;

            return true;
        }
    }

    [Instruction(Cmd = Opcode.GT)]
    class CmdGT : InstructionMath
    {
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a > b ? 1 : 0;

            return true;
        }
    }

    [Instruction(Cmd = Opcode.GE)]
    class CmdGE : InstructionMath
    {            
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a >= b ? 1 : 0;

            return true;
        }

    }

    [Instruction(Cmd = Opcode.LT)]
    class CmdLT : InstructionMath
    {
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a < b ? 1 : 0;

            return true;
        }
    }

    [Instruction(Cmd = Opcode.LE)]
    class CmdLE : InstructionMath
    {
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a <= b ? 1 : 0;

            return true;
        }

    }

    [Instruction(Cmd = Opcode.EQ)]
    class CmdEQ : InstructionMath
    {
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a == b ? 1 : 0;

            return true;
        }
    }

    [Instruction(Cmd = Opcode.NE)]
    class CmdNE : InstructionMath
    {
        public override bool ExecuteOn2Value(Command cmd, float a, float b, out float x)
        {
            x = a != b ? 1 : 0;

            return true;
        }
    }



}
