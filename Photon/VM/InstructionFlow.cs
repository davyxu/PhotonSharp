

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
                return true;
            }

            return true;
        }

        public override string Print( Command cmd)
        {
            return string.Format("Condition: {0}, PC : {1}", vm.DataStack.Get(),  cmd.DataA);
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

    [Instruction(Cmd = Opcode.CALLD)]
    class CmdCallD : Instruction
    {
        public override bool Execute(Command cmd)
        {
            var argCount = cmd.DataA;

            var obj = vm.DataStack.Pop();

            var dg = obj as ValueDelegate;

            // 外部调用不进行栈调整
            var stackBeforeCall = vm.DataStack.Count;

            int retValueCount = 0;

            if (dg.Entry != null)
            {
                retValueCount = dg.Entry(vm);
            }

            // 调用结束时需要平衡栈( 返回值没有被用到 )
            if (cmd.DataB != 0)
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
            return string.Format("ArgCount : {0}  Func: {1}  BalanceStack: {2}", cmd.DataA, vm.DataStack.Get(), cmd.DataB);
        }
    }


    [Instruction(Cmd = Opcode.CALLF)]
    class CmdCallF : Instruction
    {
        public override bool Execute(Command cmd)
        {
            var argCount = cmd.DataA;

            var obj = vm.DataStack.Pop();

            var func = obj.CastFunc();            

            var cmdSet = func.Proc as CommandSet;

            // 更换当前上下文
            vm.EnterFrame(cmdSet);

            vm.CurrFrame.Closure = obj as ValueClosure;

            // 调用结束时需要平衡栈
            if (cmd.DataB != 0)
            {
                vm.CurrFrame.RestoreDataStack = true;
            }

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



            // 马上跳到下个执行域            
            return false;
        }


        public override string Print(Command cmd)
        {
            return string.Format("ArgCount : {0}  Func: {1}  BalanceStack: {2}", cmd.DataA, vm.DataStack.Get(), cmd.DataB);
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
