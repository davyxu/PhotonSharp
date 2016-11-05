using Photon.OpCode;

namespace Photon.VM
{
    [Instruction(Cmd = Opcode.LoadC)]
    class CmdLoadC
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            var c = vm.Executable.Constants.Get(cmd.DataA);
            vm.Stack.Push(c);

            return true;
        }

        public static string Print( VMachine vm, Command cmd )
        {
            return string.Format("S <- C{0}     | C{1}: {2}", cmd.DataA, cmd.DataA, vm.Executable.Constants.ValueToString(cmd.DataA));
        }
    }

    [Instruction(Cmd = Opcode.LoadR)]
    class CmdLoadR
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            DataValue v = vm.LocalRegister.Get(cmd.DataA);
            vm.Stack.Push(v);

            return true;  
        }

        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("S <- R{0}     | R{1}: {2}", cmd.DataA, cmd.DataA, vm.LocalRegister.ValueToString(cmd.DataA));
        }
    }

    [Instruction(Cmd = Opcode.SetR)]
    class CmdSetR
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            var regIndex = cmd.DataA;
            var d = vm.Stack.Pop();
            vm.LocalRegister.Set(regIndex, d);

            

            return true;
        }
        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("R{0} <- S(Top)     | S(Top): {1}", cmd.DataA, vm.Stack.ValueToString());
        }

    }


    [Instruction(Cmd = Opcode.LoadG)]
    class CmdLoadG
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            DataValue v = vm.GlobalRegister.Get(cmd.DataA);

            vm.Stack.Push(v);

            return true;
        }
        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("S <- G{0}     | G{1}: {2}", cmd.DataA, cmd.DataA, vm.GlobalRegister.ValueToString(cmd.DataA));
        }
    }

    [Instruction(Cmd = Opcode.SetG)]
    class CmdSetG
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            var regIndex = cmd.DataA;
            var d = vm.Stack.Pop();
            vm.GlobalRegister.Set(regIndex, d);

            return true;
        }

        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("G{0} <- S(Top)     | S(Top): {1}", cmd.DataA, vm.Stack.ValueToString());
        }
    }
}
