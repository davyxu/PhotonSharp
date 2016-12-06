using SharpLexer;

namespace Photon
{
    [Instruction(Cmd = Opcode.JZ)]
    class CmdJZ : Instruction
    {
        public override bool Execute( Command cmd)
        {
            var targetPC = cmd.DataA;

            var d = Convertor.ValueToFloat32(vm.DataStack.Pop());

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
    class CmdCall : Instruction
    {
        public override bool Execute(Command cmd)
        {
            var argCount = cmd.DataA;

            var obj = vm.DataStack.Pop();

            var func = Convertor.CastFunc(obj);

            return func.Invoke(vm, argCount, cmd.DataB, obj as ValueClosure);

            throw new RuntimeException("expect function or delegate");
        }

        public override string Print(Command cmd)
        {
            return string.Format("ArgCount : {0}  ReceiverCount: {1}", cmd.DataA, cmd.DataB);
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
