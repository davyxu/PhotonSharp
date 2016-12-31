using SharpLexer;

namespace Photon
{
    [Instruction(Cmd = Opcode.LOADK)]
    class CmdLoadK : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {            
            var c = vm.Constants.Get(cmd.DataA);

            vm.DataStack.Push(c);

            return true;
        }

        public override string Print( Command cmd)
        {            
            return string.Format("Const: {0}", cmd.DataA);
        }
    }

    [Instruction(Cmd = Opcode.LOADR)]
    class CmdLoadR : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {
            int regIndex = cmd.DataA;

            Value v = vm.LocalReg.Get(regIndex);

            vm.DataStack.Push(v);

            return true;  
        }

        public override string Print( Command cmd)
        {
            int regIndex = cmd.DataA;

            return string.Format("Reg: {0}", regIndex);
        }
    }

    [Instruction(Cmd = Opcode.LOADG)]
    class CmdLoadG : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {

            Value v = vm.GetRuntimePackage(cmd.DataA).Reg.Get(cmd.DataB);

            vm.DataStack.Push(v);

            return true;
        }
        public override string Print(Command cmd)
        {
            return string.Format("Pkg: {0}  Reg: {1}", cmd.DataA, cmd.DataB);
        }
    }

    [Instruction(Cmd = Opcode.LOADI)]
    class CmdLoadI : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {
            var obj = vm.DataStack.Pop();

            var key = vm.DataStack.Pop();

            var value = obj.OperateGetKeyValue(key);

            vm.DataStack.Push(value);

            return true;
        }

        public override string Print(Command cmd)
        {
            return string.Empty;
        }
    }

    [Instruction(Cmd = Opcode.LOADU)]
    class CmdLoadU : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {
            int regIndex = cmd.DataA;

            var v = vm.CurrFrame.Closure.GetValue(regIndex);

            vm.DataStack.Push(v);

            return true;
        }
        public override string Print(Command cmd)
        {
            return string.Format("UpValue: {0}", cmd.DataA);
        }
    }

    [Instruction(Cmd = Opcode.LOADM)]
    class CmdLoadM : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {
            var ci = vm.DataStack.Pop();

            vm.DataStack.Push(ci.OperateGetMemberValue(cmd.DataA));

            return true;
        }

        public override string Print(Command cmd)
        {
            return string.Format("MemberKey: {0}", cmd.DataA);
        }
    }

    [Instruction(Cmd = Opcode.LOADB)]
    class CmdLoadB : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {
            var ci = Convertor.CastClassInstance(vm.DataStack.Pop());

            vm.DataStack.Push(ci.GetBaseValue(cmd.DataA));

            return true;
        }

        public override string Print(Command cmd)
        {
            return string.Format("BaseMemberKey: {0}", cmd.DataA);
        }
    }


    [Instruction(Cmd = Opcode.SETR)]
    class CmdSetR : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {
            int regIndex = cmd.DataA;

            var d = vm.DataStack.Pop();

            vm.LocalReg.Set(regIndex, d);

            return true;
        }
        public override string Print( Command cmd)
        {
            int regIndex = cmd.DataA;

            return string.Format("Reg: {0}", regIndex);
        }

    }




    [Instruction(Cmd = Opcode.SETG)]
    class CmdSetG : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {            
            var d = vm.DataStack.Pop();

            vm.GetRuntimePackage(cmd.DataA).Reg.Set(cmd.DataB, d);            

            return true;
        }

        public override string Print( Command cmd)
        {
            return string.Format("Pkg: {0}  Reg: {1}", cmd.DataA, cmd.DataB);
        }
    }

    [Instruction(Cmd = Opcode.SETI)]
    class CmdSetI : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {
            var obj = vm.DataStack.Pop();

            var key = vm.DataStack.Pop();

            var value = vm.DataStack.Pop();

            obj.OperateSetKeyValue(key, value);            

            return true;
        }

        public override string Print(Command cmd)
        {
            return string.Empty;
        }
    }

    [Instruction(Cmd = Opcode.SETU)]
    class CmdSetU : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {

            var regIndex = cmd.DataA;

            var d = vm.DataStack.Pop();

            vm.CurrFrame.Closure.SetValue(regIndex, d);

            return true;
        }

        public override string Print(Command cmd)
        {
            return string.Format("UpValue: {0}", cmd.DataA);
        }
    }

    [Instruction(Cmd = Opcode.SETM)]
    class CmdSetM : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {
            var obj = vm.DataStack.Pop();

            var v = vm.DataStack.Pop();

            obj.OperateSetMemberValue(cmd.DataA, v);

            return true;
        }

        public override string Print(Command cmd)
        {
            return string.Format("MemberKey: {0}", cmd.DataA);
        }
    }


    [Instruction(Cmd = Opcode.SETA)]
    class CmdSetA : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {
            int valueCount = cmd.DataA;

            var arr = Convertor.CastObject(vm.DataStack.Get(-valueCount - 1)).Raw as Array;
            if (arr == null)
            {
                throw new RuntimeException("Expect 'Builtin.Array' value");
            }

            

            arr.Raw.Clear();

            arr.Raw.Capacity = valueCount;

            var argBegin = vm.DataStack.Count - valueCount;

            for( int i = 0;i<valueCount;i++)
            {
                arr.Raw.Add(vm.DataStack.Get(argBegin + i ));
            }

            vm.DataStack.PopMulti(valueCount);

            return true;
        }

        public override string Print(Command cmd)
        {
            return string.Format("MemberKey: {0}", cmd.DataA);
        }
    }


    [Instruction(Cmd = Opcode.SETD)]
    class CmdSetD : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {
            int valueCount = cmd.DataA;

            var map = Convertor.CastObject(vm.DataStack.Get(-valueCount*2 - 1)).Raw as Map;
            if (map == null)
            {
                throw new RuntimeException("Expect 'Builtin.Map' value");
            }

            map.Raw.Clear();            

            var argBegin = vm.DataStack.Count - valueCount * 2;

            for (int i = 0; i < valueCount * 2; i+=2 )
            {
                var key = vm.DataStack.Get(argBegin + i);
                var value = vm.DataStack.Get(argBegin + i + 1);

                map.Raw.Add(key, value);
            }

            vm.DataStack.PopMulti(valueCount* 2);

            return true;
        }

        public override string Print(Command cmd)
        {
            return string.Format("MemberKey: {0}", cmd.DataA);
        }
    }


    [Instruction(Cmd = Opcode.SEL)]
    class CmdSelect : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {
           // var c = Convertor.CastObject(vm.DataStack.Pop());

           // vm.DataStack.Push(c.GetValue(cmd.DataA));

            return true;
        }

        public override string Print( Command cmd)
        {
            return string.Format("NameKey: {0}", cmd.DataA);
        }
    }


    [Instruction(Cmd = Opcode.LINKU)]
    class CmdLinkU : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {
            // 使用寄存器的地方和引用地方scope之差
            int mode = cmd.DataA;

            switch( mode )
            {
                    // 创建快照
                case 0:
                    {
                        int Index = cmd.DataB;

                        var closure = Convertor.CastClosure(vm.DataStack.Get(-1));

                        closure.AddUpValue(vm.LocalReg, Index);
                    }
                    break;
                    // 直接引用
                case 1:
                    {
                        int Index = cmd.DataB;

                        var closure = Convertor.CastClosure(vm.DataStack.Get(-1));

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
            return string.Format("Mode: {0} UpValue: {1}", cmd.DataA, cmd.DataB);
        }
    }


    [Instruction(Cmd = Opcode.LOADF)]
    class CmdLoadF : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {
            var proc = Convertor.CastFunc( vm.GetEntry(cmd.DataA ) );

            vm.DataStack.Push(proc);

            return true;
        }

        public override string Print(Command cmd)
        {
            return string.Format("Proc: {0}", cmd.DataA);
        }
    }

    [Instruction(Cmd = Opcode.CLOSURE)]
    class CmdClosure : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {
            var proc = Convertor.CastFunc(vm.GetEntry(cmd.DataA));

            vm.DataStack.Push(new ValueClosure(proc));

            return true;
        }

        public override string Print(Command cmd)
        {
            return string.Format("Proc: {0}", cmd.DataA);
        }
    }

    [Instruction(Cmd = Opcode.NEW)]
    class CmdNew : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {
            var ins = Convertor.CastClassType(vm.GetEntry(cmd.DataA)).CreateInstance();

            vm.DataStack.Push(ins);

            return true;
        }

        public override string Print(Command cmd)
        {
            return string.Format("ClassNameKey : {0}", cmd.DataA);
        }
    }



    [Instruction(Cmd = Opcode.VISIT)]
    class CmdVisit : Instruction
    {
        public override bool Execute(VMachine vm, Command cmd)
        {
            var iterObj = vm.DataStack.Pop();
            var x = vm.DataStack.Pop();

            var arr = Convertor.CastObject(x).Raw as Array;
            if ( arr != null )
            {
                ValueArrayIterator iter;
                
                if ( iterObj.Equals(Value.Nil))
                {
                    iter = new ValueArrayIterator(arr);
                }
                else
                {
                    iter = iterObj as ValueArrayIterator;
                    if ( iter == null )
                    {
                        throw new RuntimeException("Require array iterator");
                    }

                    iter.Next();
                }

                if ( !iter.Iterate(vm.DataStack) )
                {
                    vm.CurrFrame.PC = cmd.DataA;
                    return false;
                }
            }

            return true;
        }

        public override string Print(Command cmd)
        {
            return string.Format("ClassNameKey : {0}", cmd.DataA);
        }
    }

}
