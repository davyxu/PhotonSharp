using Photon.OpCode;

namespace Photon.VM
{
    [Instruction(Cmd = Opcode.Add)]
    class CmdAdd
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            var a = VMachine.CastNumber(vm.Stack.Pop());
            var b = VMachine.CastNumber(vm.Stack.Pop());

            vm.Stack.Push(new NumberValue(a + b));

            return true;
        }
        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("A: {0}, B: {1}", vm.Stack.ValueToString(-1), vm.Stack.ValueToString(-2));
        }
    }

    [Instruction(Cmd = Opcode.Sub)]
    class CmdSub
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            var a = VMachine.CastNumber(vm.Stack.Pop());
            var b = VMachine.CastNumber(vm.Stack.Pop());

            vm.Stack.Push(new NumberValue(b - a));

            return true;
        }

        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("A: {0}, B: {1}", vm.Stack.ValueToString(-1), vm.Stack.ValueToString(-2));
        }
    }

    [Instruction(Cmd = Opcode.Mul)]
    class CmdMul
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            var a = VMachine.CastNumber(vm.Stack.Pop());
            var b = VMachine.CastNumber(vm.Stack.Pop());

            vm.Stack.Push(new NumberValue(a * b));

            return true;
        }

        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("A: {0}, B: {1}", vm.Stack.ValueToString(-1), vm.Stack.ValueToString(-2));
        }
    }

    [Instruction(Cmd = Opcode.Div)]
    class CmdDiv
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            var a = VMachine.CastNumber(vm.Stack.Pop());
            var b = VMachine.CastNumber(vm.Stack.Pop());

            vm.Stack.Push(new NumberValue(b / a));

            return true;
        }

        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("A: {0}, B: {1}", vm.Stack.ValueToString(-1), vm.Stack.ValueToString(-2));
        }
    }

    [Instruction(Cmd = Opcode.GT)]
    class CmdGT
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            var a = VMachine.CastNumber(vm.Stack.Pop());
            var b = VMachine.CastNumber(vm.Stack.Pop());

            vm.Stack.Push(new NumberValue(b > a ? 1 : 0));

            return true;
        }

        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("A: {0}, B: {1}", vm.Stack.ValueToString(-1), vm.Stack.ValueToString(-2));
        }
    }

    [Instruction(Cmd = Opcode.GE)]
    class CmdGE
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            var a = VMachine.CastNumber(vm.Stack.Pop());
            var b = VMachine.CastNumber(vm.Stack.Pop());

            vm.Stack.Push(new NumberValue(b >= a ? 1 : 0));

            return true;
        }

        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("A: {0}, B: {1}", vm.Stack.ValueToString(-1), vm.Stack.ValueToString(-2));
        }
    }

    [Instruction(Cmd = Opcode.LT)]
    class CmdLT
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            var a = VMachine.CastNumber(vm.Stack.Pop());
            var b = VMachine.CastNumber(vm.Stack.Pop());

            vm.Stack.Push(new NumberValue(b < a ? 1 : 0));

            return true;
        }

        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("A: {0}, B: {1}", vm.Stack.ValueToString(-1), vm.Stack.ValueToString(-2));
        }
    }

    [Instruction(Cmd = Opcode.LE)]
    class CmdLE
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            var a = VMachine.CastNumber(vm.Stack.Pop());
            var b = VMachine.CastNumber(vm.Stack.Pop());

            vm.Stack.Push(new NumberValue(b <= a ? 1 : 0));

            return true;
        }

        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("A: {0}, B: {1}", vm.Stack.ValueToString(-1), vm.Stack.ValueToString(-2));
        }
    }

    [Instruction(Cmd = Opcode.EQ)]
    class CmdEQ
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            var a = VMachine.CastNumber(vm.Stack.Pop());
            var b = VMachine.CastNumber(vm.Stack.Pop());

            vm.Stack.Push(new NumberValue(a == b ? 1 : 0));

            return true;
        }

        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("A: {0}, B: {1}", vm.Stack.ValueToString(-1), vm.Stack.ValueToString(-2));
        }
    }

    [Instruction(Cmd = Opcode.NE)]
    class CmdNE
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            var a = VMachine.CastNumber(vm.Stack.Pop());
            var b = VMachine.CastNumber(vm.Stack.Pop());

            vm.Stack.Push(new NumberValue(a != b ? 1 : 0));

            return true;
        }

        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("A: {0}, B: {1}", vm.Stack.ValueToString(-1), vm.Stack.ValueToString(-2));
        }
    }



}
