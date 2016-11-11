using Photon.Model;

namespace Photon.VM
{
    [Instruction(Cmd = Opcode.LoadC)]
    class CmdLoadC : Instruction
    {
        public override bool Execute( Command cmd)
        {
            var c = vm.Exec.Constants.Get(cmd.DataA);
            vm.Stack.Push(c);

            return true;
        }

        public static string Print( VMachine vm, Command cmd )
        {
            return string.Format("S <- C{0}     | C{1}: {2}", cmd.DataA, cmd.DataA, vm.Exec.Constants.Get(cmd.DataA));
        }
    }

    [Instruction(Cmd = Opcode.LoadR)]
    class CmdLoadR : Instruction
    {
        public override bool Execute( Command cmd)
        {
            int regIndex = cmd.DataA + vm.RegBase;
            Value v = vm.Reg.Get(regIndex);
            vm.Stack.Push(v);

            return true;  
        }

        public override string Print( Command cmd)
        {
            int regIndex = cmd.DataA + vm.RegBase;

            return string.Format("S <- R{0}     | R{1}: {2}", regIndex, regIndex, vm.Reg.Get(regIndex));
        }
    }

    [Instruction(Cmd = Opcode.SetR)]
    class CmdSetR : Instruction
    {
        public override bool Execute( Command cmd)
        {
            int regIndex = cmd.DataA + vm.RegBase;
            var d = vm.Stack.Pop();
            vm.Reg.Set(regIndex, d);

            

            return true;
        }
        public override string Print( Command cmd)
        {
            int regIndex = cmd.DataA + vm.RegBase;

            return string.Format("R{0} <- S(Top)     | S(Top): {1}", regIndex, vm.Stack.Get());
        }

    }


    [Instruction(Cmd = Opcode.LoadG)]
    class CmdLoadG : Instruction
    {
        public override bool Execute( Command cmd)
        {
            Value v = vm.Reg.Get(cmd.DataA);

            vm.Stack.Push(v);

            return true;
        }
        public override string Print( Command cmd)
        {
            return string.Format("S <- R{0}     | R{1}: {2}", cmd.DataA, cmd.DataA, vm.Reg.Get(cmd.DataA));
        }
    }

    [Instruction(Cmd = Opcode.SetG)]
    class CmdSetG : Instruction
    {
        public override bool Execute( Command cmd)
        {
            var regIndex = cmd.DataA;
            var d = vm.Stack.Pop();
            vm.Reg.Set(regIndex, d);

            return true;
        }

        public override string Print( Command cmd)
        {
            return string.Format("R{0} <- S(Top)     | S(Top): {1}", cmd.DataA, vm.Stack.Get());
        }
    }

    [Instruction(Cmd = Opcode.Index)]
    class CmdIndexR : Instruction
    {
        public override bool Execute( Command cmd)
        {
            var key = vm.Stack.Pop();
            var main = vm.Stack.Pop().CastObject();
            var result = main.Get(key);
            vm.Stack.Push(result);
                        
            return true;
        }

        public override string Print( Command cmd)
        {

            return string.Format("Body[Key]         | Body: {0}, Key: {1}", vm.Stack.Get(-2), vm.Stack.Get(-1));
        }
    }

    [Instruction(Cmd = Opcode.Select)]
    class CmdSelectR : Instruction
    {
        public override bool Execute( Command cmd)
        {            
            var main = vm.Stack.Pop().CastObject();

            var key = vm.Exec.Constants.Get(cmd.DataA);

            var result = main.Select(key);
            vm.Stack.Push(result);

            return true;
        }

        public override string Print( Command cmd)
        {

            return string.Format("Body[Key]         | Body: {0}, Key: {1}", vm.Stack.Get(-1), vm.Exec.Constants.Get(cmd.DataA) );
        }
    }


    [Instruction(Cmd = Opcode.LinkU)]
    class CmdLinkU : Instruction
    {
        public override bool Execute( Command cmd)
        {
            int upIndex = cmd.DataA;
            int regIndex = cmd.DataB + vm.RegBase;

            var closure = vm.Stack.Get() as ValueClosure;

            Slot slot = vm.Reg.GetSlot(regIndex);
            closure.AddUpValue(slot);
            

            return true;
        }
        public override string Print( Command cmd)
        {
            int upIndex = cmd.DataA;
            int regIndex = cmd.DataB;

            return string.Format("U{0} <- &R{1}     | R{2}: {3}", upIndex, regIndex, regIndex, vm.Reg.Get(regIndex));
        }
    }


    [Instruction(Cmd = Opcode.LoadU)]
    class CmdLoadU : Instruction
    {
        public override bool Execute( Command cmd)
        {
            int regIndex =  cmd.DataA;

            var v = vm.CurrFrame.Closure.GetUpValue(regIndex);            

            vm.Stack.Push(v);

            return true;
        }
        public override string Print( Command cmd)
        {
            return string.Format("S <- R{0}     | R{1}: {2}", cmd.DataA, cmd.DataA, vm.Reg.Get(cmd.DataA));
        }
    }

    [Instruction(Cmd = Opcode.SetU)]
    class CmdSetU : Instruction
    {
        public override bool Execute( Command cmd)
        {
            
            var regIndex = cmd.DataA;
            var d = vm.Stack.Pop();
            vm.CurrFrame.Closure.SetUpValue(regIndex, d );            

            return true;
        }

        public override string Print( Command cmd)
        {
            return string.Format("R{0} <- S(Top)     | S(Top): {1}", cmd.DataA, vm.Stack.Get());
        }
    }
}
