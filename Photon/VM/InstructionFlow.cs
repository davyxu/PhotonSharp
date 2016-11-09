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

            var obj = vm.Stack.Pop();

            var func = obj as ValueFunc;

            if ( func != null )
            {                

                // 更换当前上下文
                vm.EnterFrame(func.Index);

                // 调用结束时需要平衡栈
                if (cmd.DataB != 0)
                {
                    vm.CurrFrame.RestoreDataStack = true;
                }

                // 将栈转为被调用函数的寄存器
                for (int i = 0; i < argCount; i++)
                {
                    var arg = vm.Stack.Get( -i - 1 );
                    vm.LocalRegister.Set(argCount - i - 1 + vm.RegBase, arg);
                }

                // 清空栈
                vm.Stack.PopMulti(argCount);

                // 记录当前的数据栈位置
                vm.CurrFrame.DataStackBase = vm.Stack.Count;

                // 马上跳到下个执行域            
                return false;
            }

            var dg = obj as ValueDelegate;
            if ( dg != null )
            {
               
                // 外部调用不进行栈调整
                var stackBeforeCall = vm.Stack.Count;

                var retValueCount = dg.Entry(vm);

                // 调用结束时需要平衡栈( 返回值没有被用到 )
                if (cmd.DataB != 0 )
                {
                    // 调用前(包含参数+ delegate)
                    vm.Stack.Count = stackBeforeCall - argCount;
                }
                else
                {
                    vm.Stack.Cut(stackBeforeCall - argCount, retValueCount);
                }

            }

            return true;

        }


        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("ArgCount : {0}  Func: {1}  BalanceStack: {2}", cmd.DataA,  vm.Stack.ValueToString( ), cmd.DataB );
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
