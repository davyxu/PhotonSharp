using SharpLexer;

namespace Photon
{
    [Instruction(Cmd = Opcode.JZ)]
    class CmdJZ : Instruction
    {
        public override bool Execute( Command cmd)
        {
            var targetPC = cmd.DataA;

            var d = vm.DataStack.Pop().CastNumber();

            if ( d == 0 )
            {
                vm.CurrFrame.PC = targetPC;                
                return false;
            }

            return true;
        }

        public override string Print( Command cmd)
        {
            return string.Format("PC : {0}", cmd.DataA);
        }
    }

    [Instruction(Cmd = Opcode.JMP)]
    class CmdJmp : Instruction
    {
        public override bool Execute( Command cmd)
        {
            vm.CurrFrame.PC = cmd.DataA;

            return false;
        }

        public override string Print( Command cmd)
        {
            return string.Format("PC : {0}", cmd.DataA);
        }
    }


    [Instruction(Cmd = Opcode.CALL)]
    class CmdCallF : Instruction
    {
        public override bool Execute(Command cmd)
        {
            var argCount = cmd.DataA;

            var obj = vm.DataStack.Pop();

            var func = obj.CastFunc();

            var cmdSet = func.Proc as CommandSet;
            if ( cmdSet != null )
            {
                return InvokeFunction(cmdSet, argCount, cmd.DataB != 0, obj as ValueClosure);
            }
            
            var dg = func.Proc as Delegate;
            if ( dg != null )
            {
                return InvokeDelegate(dg, argCount, cmd.DataB != 0);
            }

            throw new RuntimeException("expect function or delegate");
        }

        bool InvokeFunction(CommandSet cmdSet, int argCount, bool balanceStack, ValueClosure closure )
        {
            // 更换当前上下文
            vm.EnterFrame(cmdSet);

            vm.CurrFrame.RestoreDataStack = balanceStack;
            vm.CurrFrame.Closure = closure;

            // 将栈转为被调用函数的寄存器
            for (int i = 0; i < argCount; i++)
            {
                var arg = vm.DataStack.Get(-i - 1);
                vm.LocalReg.Set(argCount - i - 1 + vm.RegBase, arg);
            }

            // 清空栈
            vm.DataStack.PopMulti(argCount);

            // 记录当前的数据栈位置
            vm.CurrFrame.DataStackBase = vm.DataStack.Count;

            return false;
        }

        bool InvokeDelegate(Delegate dg, int argCount, bool balanceStack)
        {            
            // 外部调用不进行栈调整
            var stackBeforeCall = vm.DataStack.Count;

            int retValueCount = 0;

            if (dg.Entry != null)
            {
                retValueCount = dg.Entry(vm);
            }

            // 调用结束时需要平衡栈( 返回值没有被用到 )
            if (balanceStack)
            {
                // 调用前(包含参数+ delegate)
                vm.DataStack.Count = stackBeforeCall - argCount;
            }
            else
            {
                vm.DataStack.Cut(stackBeforeCall - argCount, retValueCount);
            }

            return true;
        }


        public override string Print(Command cmd)
        {
            return string.Format("ArgCount : {0}  BalanceStack: {1}", cmd.DataA, cmd.DataB);
        }
    }


    [Instruction(Cmd = Opcode.RET)]
    class CmdRet : Instruction
    {
        public override bool Execute( Command cmd)
        {
            vm.LeaveFrame();

            return true;
        }
    }
  
    [Instruction(Cmd = Opcode.EXIT)]
    class CmdExit : Instruction
    {
        public override bool Execute( Command cmd)
        {            
            vm.CurrFrame.PC = -1;

            return false;
        }
    }
    [Instruction(Cmd = Opcode.NOP)]
    class CmdNop : Instruction
    {
        public override bool Execute(Command cmd)
        {            

            return true;
        }
    }

}
