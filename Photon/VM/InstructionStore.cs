﻿using SharpLexer;

namespace Photon
{
    [Instruction(Cmd = Opcode.LOADC)]
    class CmdLoadC : Instruction
    {
        public override bool Execute( Command cmd)
        {
            var pkg = vm.GetRuntimePackage(cmd.DataA);

            var c = pkg.Constants.Get(cmd.DataB);

            vm.DataStack.Push(c);

            return true;
        }

        public static string Print( VMachine vm, Command cmd )
        {
            var pkg = vm.GetRuntimePackage(cmd.DataA);

            return string.Format("S <- C{0}     | {1} C{2}: {3}", cmd.DataB, pkg.Name, cmd.DataB, pkg.Constants.Get(cmd.DataB));
        }
    }

    [Instruction(Cmd = Opcode.LOADR)]
    class CmdLoadR : Instruction
    {
        public override bool Execute( Command cmd)
        {
            int regIndex = cmd.DataA + vm.RegBase;

            Value v = vm.LocalReg.Get(regIndex);

            vm.DataStack.Push(v);

            return true;  
        }

        public override string Print( Command cmd)
        {
            int regIndex = cmd.DataA + vm.RegBase;

            return string.Format("S <- R{0}     | R{1}: {2}", regIndex, regIndex, vm.LocalReg.Get(regIndex));
        }
    }

    [Instruction(Cmd = Opcode.SETR)]
    class CmdSetR : Instruction
    {
        public override bool Execute( Command cmd)
        {
            int regIndex = cmd.DataA + vm.RegBase;

            var d = vm.DataStack.Pop();

            vm.LocalReg.Set(regIndex, d);

            return true;
        }
        public override string Print( Command cmd)
        {
            int regIndex = cmd.DataA + vm.RegBase;

            return string.Format("R{0} <- S(Top)     | S(Top): {1}", regIndex, vm.DataStack.Get(-1));
        }

    }


    [Instruction(Cmd = Opcode.LOADG)]
    class CmdLoadG : Instruction
    {
        public override bool Execute( Command cmd)
        {

            Value v = vm.GetRuntimePackage(cmd.DataA).Reg.Get(cmd.DataB);

            vm.DataStack.Push(v);

            return true;
        }
        public override string Print( Command cmd)
        {
            return string.Format("S <- G{0}     | G{1}: {2}", cmd.DataA, cmd.DataA, vm.GetRuntimePackage(cmd.DataA).Reg.Get(cmd.DataB));
        }
    }

    [Instruction(Cmd = Opcode.SETG)]
    class CmdSetG : Instruction
    {
        public override bool Execute( Command cmd)
        {            
            var d = vm.DataStack.Pop();

            vm.GetRuntimePackage(cmd.DataA).Reg.Set(cmd.DataB, d);            

            return true;
        }

        public override string Print( Command cmd)
        {
            return string.Format("G{0} <- S(Top)     | S(Top): {1}", cmd.DataA, vm.DataStack.Get(-1));
        }
    }

    [Instruction(Cmd = Opcode.IDX)]
    class CmdIndex : Instruction
    {
        public override bool Execute( Command cmd)
        {
            var key = vm.DataStack.Pop();

            var main = vm.DataStack.Pop().CastObject();

            var result = main.Get(key);

            vm.DataStack.Push(result);
                        
            return true;
        }

        public override string Print( Command cmd)
        {

            return string.Format("Body[Key]         | Body: {0}, Key: {1}", vm.DataStack.Get(-2), vm.DataStack.Get(-1));
        }
    }

    [Instruction(Cmd = Opcode.SEL)]
    class CmdSelect : Instruction
    {
        public override bool Execute( Command cmd)
        {            
            var main = vm.DataStack.Pop().CastObject();

            var key = cmd.Pkg.Constants.Get(cmd.DataA);

            var result = main.Select(key);

            vm.DataStack.Push(result);

            return true;
        }

        public override string Print( Command cmd)
        {

            return string.Format("Body[Key]         | Body: {0}, Key: {1}", vm.DataStack.Get(-1), cmd.Pkg.Constants.Get(cmd.DataA));
        }
    }


    [Instruction(Cmd = Opcode.LINKU)]
    class CmdLinkU : Instruction
    {
        public override bool Execute( Command cmd)
        {
            // 使用寄存器的地方和引用地方scope之差
            int mode = cmd.DataA;

            switch( mode )
            {
                    // 创建快照
                case 0:
                    {
                        int Index = cmd.DataB;

                        var closure = vm.DataStack.Get(-1).CastClosure();

                        closure.AddUpValue(vm.LocalReg, Index);
                    }
                    break;
                    // 直接引用
                case 1:
                    {
                        int Index = cmd.DataB;

                        var closure = vm.DataStack.Get(-1).CastClosure();

                        // 当前函数执行体也是个闭包
                        var v = vm.CurrFrame.Closure.GetUpValue(Index);
                        
                        closure.AddUpValue(v.Reg, v.Index);
                    }
                    break;
            }


            return true;
        }
        public override string Print( Command cmd)
        {
            int upIndex = cmd.DataA;

            int regIndex = cmd.DataB;

            return string.Format("U{0} <- &R{1}     | R{2}: {3}", upIndex, regIndex, regIndex, vm.LocalReg.Get(regIndex));
        }
    }


    [Instruction(Cmd = Opcode.LOADU)]
    class CmdLoadU : Instruction
    {
        public override bool Execute( Command cmd)
        {
            int regIndex =  cmd.DataA;

            var v = vm.CurrFrame.Closure.GetValue(regIndex);            

            vm.DataStack.Push(v);

            return true;
        }
        public override string Print( Command cmd)
        {
            return string.Format("S <- R{0}     | R{1}: {2}", cmd.DataA, cmd.DataA, vm.LocalReg.Get(cmd.DataA));
        }
    }

    [Instruction(Cmd = Opcode.SETU)]
    class CmdSetU : Instruction
    {
        public override bool Execute( Command cmd)
        {
            
            var regIndex = cmd.DataA;

            var d = vm.DataStack.Pop();

            vm.CurrFrame.Closure.SetValue(regIndex, d );            

            return true;
        }

        public override string Print( Command cmd)
        {
            return string.Format("R{0} <- S(Top)     | S(Top): {1}", cmd.DataA, vm.DataStack.Get(-1));
        }
    }

    [Instruction(Cmd = Opcode.LOADF)]
    class CmdLoadF : Instruction
    {
        public override bool Execute(Command cmd)
        {
            var proc = vm.Exec.GetProcedure(cmd.DataA );

            vm.DataStack.Push(new ValueFunc(proc));

            return true;
        }

        public static string Print(VMachine vm, Command cmd)
        {
            return string.Format("S <- C{0}     | C{1}: {2}", cmd.DataA, cmd.DataA, cmd.Pkg.Constants.Get(cmd.DataA));
        }
    }

    [Instruction(Cmd = Opcode.CLOSURE)]
    class CmdClosure : Instruction
    {
        public override bool Execute(Command cmd)
        {
            var proc = vm.Exec.GetProcedure(cmd.DataA );

            vm.DataStack.Push(new ValueClosure(proc));

            return true;
        }
    }
}
