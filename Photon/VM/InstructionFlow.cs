using Photon.Model;

namespace Photon.VM
{
    [Instruction(Cmd = Opcode.Jnz)]
    class CmdJnz
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            var targetPC = cmd.DataA;

            var d = vm.Stack.Pop();

            if (!VMachine.IsValueNoneZero(d))
            {
                vm.CurrFrame.PC = targetPC;                
                return true;
            }

            return true;
        }

        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("Condition: {0}, PC : {1}", vm.Stack.ValueToString(),  cmd.DataA);
        }
    }

    [Instruction(Cmd = Opcode.Jmp)]
    class CmdJmp
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            vm.CurrFrame.PC = cmd.DataA;

            return false;
        }

        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("PC : {0}", cmd.DataA);
        }
    }

    [Instruction(Cmd = Opcode.Call)]
    class CmdCall
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            var argCount = cmd.DataA;

            var funcIndex = VMachine.CastFuncIndex(vm.Stack.Get(-argCount - 1));

            // 更换当前上下文
            vm.EnterFrame(funcIndex);

            // 调用结束时需要平衡栈
            if (cmd.DataB != 0)
            {
                vm.CurrFrame.RestoreDataStack = true;
            }

            // 将栈转为被调用函数的寄存器
            for (int i = 0; i < argCount; i++)
            {
                var arg = vm.Stack.Get(-argCount + i);
                vm.LocalRegister.Set(i, arg);
            }

            // 清空栈
            vm.Stack.PopMulti(argCount + 1);

            // 记录当前的数据栈位置
            vm.CurrFrame.DataStackBase = vm.Stack.Count;


            // 马上跳到下个执行域            
            return false;
        }

        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("ArgCount : {0}  Func: {1}  BalanceStack: {2}", cmd.DataA,  vm.Stack.ValueToString(-cmd.DataA - 1), cmd.DataB );
        }
    }
    [Instruction(Cmd = Opcode.Ret)]
    class CmdRet
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            vm.LaveFrame();

            return true;
        }
    }
  
    [Instruction(Cmd = Opcode.Exit)]
    class CmdExit
    {
        public static bool Execute(VMachine vm, Command cmd)
        {
            vm.CurrFrame.PC = -1;

            return false;
        }
    }

}
