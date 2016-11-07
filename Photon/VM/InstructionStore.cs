using Photon.Model;

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
            Value v = vm.LocalRegister.Get(cmd.DataA);
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
            Value v = vm.GlobalRegister.Get(cmd.DataA);

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

    [Instruction(Cmd = Opcode.IndexR)]
    class CmdIndexR
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            var key = vm.Stack.Pop();
            var main = VMachine.CastObject( vm.Stack.Pop() );
            var result = main.Get(key);
            vm.Stack.Push(result);
                        
            return true;
        }

        public static string Print(VMachine vm, Command cmd)
        {

            return string.Format("Body[Key]         | Body: {0}, Key: {1}", vm.Stack.ValueToString(-2), vm.Stack.ValueToString(-1));
        }
    }

    [Instruction(Cmd = Opcode.SelectR)]
    class CmdSelectR
    {
        public static bool Execute(VMachine vm, Command cmd)
        {            
            var main = VMachine.CastObject(vm.Stack.Pop());

            var key = vm.Executable.Constants.Get(cmd.DataA);

            var result = main.Select(key);
            vm.Stack.Push(result);

            return true;
        }

        public static string Print(VMachine vm, Command cmd)
        {

            return string.Format("Body[Key]         | Body: {0}, Key: {1}", vm.Stack.ValueToString(-1), vm.Executable.Constants.ValueToString(cmd.DataA) );
        }
    }
}
