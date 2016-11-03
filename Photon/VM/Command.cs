using Photon.OpCode;

namespace Photon.VM
{
    public partial class VMachine
    {
        void ExecCommand(Command cmd)
        {
            switch (cmd.Op)
            {
                case Opcode.LoadC:
                    {
                        var c = _exe.Constants.Get(cmd.DataA);
                        _dataStack.Push(c);
                    }
                    break;
                case Opcode.LoadR:
                    {
                        DataValue r = GetFrameReg(cmd.DataA);

                        _dataStack.Push(r);
                    }
                    break;
                case Opcode.SetR:
                    {
                        var regIndex = cmd.DataA;
                        var d = _dataStack.Pop();
                        SetFrameReg(regIndex, d);
                    }
                    break;
                case Opcode.Jnz:
                    {
                        var targetPC = cmd.DataA;

                        var d = _dataStack.Pop();

                        if (!IsValueNoneZero(d))
                        {
                            _currFrame.PC = targetPC;
                            return;
                        }

                    }
                    break;
                case Opcode.Jmp:
                    {
                        var targetPC = cmd.DataA;


                        _currFrame.PC = targetPC;
                        return;
                    }
                case Opcode.Call:
                    {
                        var argCount = cmd.DataA;

                        var funcIndex = CastFuncIndex(_dataStack.Peek(-argCount - 1));

                        // 更换当前上下文
                        PushFrame(funcIndex);

                        // 调用结束时需要平衡栈
                        if ( cmd.DataB != 0 )
                        {
                            _currFrame.RestoreDataStack = true;
                        }
                        
                        // 将栈转为被调用函数的寄存器
                        for (int i = 0; i < argCount; i++)
                        {
                            var arg = _dataStack.Peek(-argCount + i);
                            SetFrameReg(i, arg);
                        }

                        // 清空栈
                        _dataStack.PopMulti(argCount + 1);

                        // 记录当前的数据栈位置
                        _currFrame.DataStackBase = _dataStack.Count;


                        // 马上跳到下个执行域
                        return;

                    }
                case Opcode.Ret:
                    {
                        PopFrame();


                    }
                    break;
                case Opcode.Add:
                    {
                        var a = CastNumber(_dataStack.Pop());
                        var b = CastNumber(_dataStack.Pop());

                        _dataStack.Push(new NumberValue(a + b));
                    }
                    break;
                case Opcode.Sub:
                    {
                        var b = CastNumber(_dataStack.Pop());
                        var a = CastNumber(_dataStack.Pop());

                        _dataStack.Push(new NumberValue(a - b));
                    }
                    break;
                case Opcode.Mul:
                    {
                        var a = CastNumber(_dataStack.Pop());
                        var b = CastNumber(_dataStack.Pop());

                        _dataStack.Push(new NumberValue(a * b));
                    }
                    break;
                case Opcode.Div:
                    {
                        var b = CastNumber(_dataStack.Pop());
                        var a = CastNumber(_dataStack.Pop());

                        _dataStack.Push(new NumberValue(a / b));
                    }
                    break;
                case Opcode.GT:
                    {
                        var b = CastNumber(_dataStack.Pop());
                        var a = CastNumber(_dataStack.Pop());

                        _dataStack.Push(new NumberValue(a > b ? 1 : 0));
                    }
                    break;
                case Opcode.GE:
                    {
                        var b = CastNumber(_dataStack.Pop());
                        var a = CastNumber(_dataStack.Pop());

                        _dataStack.Push(new NumberValue(a >= b ? 1 : 0));
                    }
                    break;
                case Opcode.LT:
                    {
                        var b = CastNumber(_dataStack.Pop());
                        var a = CastNumber(_dataStack.Pop());

                        _dataStack.Push(new NumberValue(a < b ? 1 : 0));
                    }
                    break;
                case Opcode.LE:
                    {
                        var b = CastNumber(_dataStack.Pop());
                        var a = CastNumber(_dataStack.Pop());

                        _dataStack.Push(new NumberValue(a <= b ? 1 : 0));
                    }
                    break;
                case Opcode.EQ:
                    {
                        var a = CastNumber(_dataStack.Pop());
                        var b = CastNumber(_dataStack.Pop());

                        _dataStack.Push(new NumberValue(a == b ? 1 : 0));
                    }
                    break;
                case Opcode.NE:
                    {
                        var a = CastNumber(_dataStack.Pop());
                        var b = CastNumber(_dataStack.Pop());

                        _dataStack.Push(new NumberValue(a != b ? 1 : 0));
                    }
                    break;
                case Opcode.Exit:
                    {
                        _currFrame.PC = -1;
                        return;
                    }

            }

            _currFrame.PC++;
        }
    }
}
