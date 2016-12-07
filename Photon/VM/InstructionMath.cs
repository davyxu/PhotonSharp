using System;

namespace Photon
{
    class InstructionBinaryMath : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {
            var b = vm.DataStack.Pop();
            var a = vm.DataStack.Pop();

            var x = a.BinaryOperate(cmd.Op, b);

            vm.DataStack.Push(x);

            return true;
        }
    }


    [Instruction(Cmd = Opcode.ADD)]
    class CmdAdd : InstructionBinaryMath
    {
    }

    [Instruction(Cmd = Opcode.SUB)]
    class CmdSub : InstructionBinaryMath
    {
    }

    [Instruction(Cmd = Opcode.MUL)]
    class CmdMul : InstructionBinaryMath
    {
    }

    [Instruction(Cmd = Opcode.DIV)]
    class CmdDiv : InstructionBinaryMath
    {
    }

    [Instruction(Cmd = Opcode.GT)]
    class CmdGT : InstructionBinaryMath
    {
    }

    [Instruction(Cmd = Opcode.GE)]
    class CmdGE : InstructionBinaryMath
    {
    }

    [Instruction(Cmd = Opcode.LT)]
    class CmdLT : InstructionBinaryMath
    {
    }

    [Instruction(Cmd = Opcode.LE)]
    class CmdLE : InstructionBinaryMath
    {
    }

    [Instruction(Cmd = Opcode.EQ)]
    class CmdEQ : InstructionBinaryMath
    {
    }

    [Instruction(Cmd = Opcode.NE)]
    class CmdNE : InstructionBinaryMath
    {
    }


    class InstructionUnaryMath : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {
            var a = vm.DataStack.Pop();

            var x = a.UnaryOperate(cmd.Op);

            vm.DataStack.Push(x);

            return true;
        }
    }

    [Instruction(Cmd = Opcode.MINUS)]
    class CmdMinus : InstructionUnaryMath
    {
    }

    [Instruction(Cmd = Opcode.INT32)]
    class CmdInt32 : InstructionUnaryMath
    {
    }

    [Instruction(Cmd = Opcode.INT64)]
    class CmdInt64 : InstructionUnaryMath
    {
    }

    [Instruction(Cmd = Opcode.FLOAT32)]
    class CmdFloat32 : InstructionUnaryMath
    {
    }

    [Instruction(Cmd = Opcode.FLOAT64)]
    class CmdFloat64 : InstructionUnaryMath
    {
    }
}
