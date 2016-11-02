using System.Collections.Generic;
using Photon.OpCode;
using System;
using System.Diagnostics;
using Photon.AST;

namespace Photon.VM
{
    public class VMachine
    {
        DataStack _dataStack;
        Register _reg;
        Stack<Frame> _frameStack = new Stack<Frame>();

        Frame _currFrame;

        Executable _exe;

        public VMachine( int maxreg = 10, int maxstack = 10)
        {
            _dataStack = new DataStack(maxstack);
            _reg = new Register(maxreg);
        }

        public bool DebugRun
        {
            get;
            set;
        }

        public Register Reg
        {
            get { return _reg; }
        }

        public DataStack Stack
        {
            get { return _dataStack; }
        }

        public void Run( Executable exe )
        {
            _frameStack.Clear();
            _dataStack.Clear();
            _reg.Clear();
            _frameStack.Clear();

            _exe = exe;
            _currFrame.Reset(exe.CmdSet[0],0 );


            while (_currFrame.PC < _currFrame.CmdSet.Commands.Count && _currFrame.PC != -1 )
            {
                var cmd = _currFrame.CmdSet.Commands[_currFrame.PC];

                if (DebugRun)
                {
                    Debug.WriteLine("{0} {1}: {2}", _currFrame.CmdSet.Name, _currFrame.PC, cmd.ToString());
                }

                ExecCommand(cmd);

            }
        }

        public void DebugPrint( )
        {
            
            _dataStack.DebugPrint();

            _reg.DebugPrint();
        }

        int GetRegOffset( int regIndex, int scopeIndex )
        {
            // 当前作用域
            if (_currFrame.CmdSet.ScopeInfo.Index == scopeIndex)
            {
                return _currFrame.RegBase + regIndex;
            }
            else
            {
                var regbase = _exe.ScopeInfoSet.Get(scopeIndex).RegBase;

                return regbase + regIndex;
            }
        }

        bool IsValueNoneZero( DataValue d )
        {
            if (d == null)
                return false;

            var nv = d as NumberValue;

            if ( nv != null )
            {
                return nv.Number != 0;
            }

            var fv = d as FuncValue;

            if ( fv != null )
            {
                return true;
            }


            Error("unknown value type");


            return false;
        }

        void ExecCommand( Command cmd )
        {
            switch( cmd.Op )
            {
                case Opcode.LoadC:
                    {
                        var c = _exe.Constants.Get(cmd.DataA);
                        _dataStack.Push(c);
                    }
                    break;
                case Opcode.LoadR:
                    {
                        var offset = GetRegOffset(cmd.DataA, cmd.DataB);
                        DataValue r = _reg.Get(offset);

                        _dataStack.Push(r);
                    }
                    break;
                case Opcode.SetR:
                    {
                        var offset = GetRegOffset(cmd.DataA, cmd.DataB);
                        var d = _dataStack.Pop();
                        _reg.Set(offset, d);
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
                        var cs = _exe.GetCmdSet(funcIndex);

                        // 被调用函数的regbase=当前函数的最大分配量+当前基础偏移
                        int regbase = _currFrame.CmdSet.ScopeInfo.AllocatedReg + _currFrame.RegBase;

                        // 将栈转为被调用函数的寄存器
                        for( int i = 0;i< argCount ;i++)
                        {
                            var arg = _dataStack.Peek( -argCount + i);
                            _reg.Set( regbase + i , arg);
                        }

                        // 清空栈
                        _dataStack.PopMulti(argCount + 1);
                         
                        // 更换当前环境
                        _frameStack.Push(_currFrame);

                        _currFrame.Reset(cs, regbase );
                        return;
                        
                    }
                case Opcode.Ret:
                    {
                        // 调试功能, 清空寄存器, 看起来清爽
                        _reg.ClearTo(_currFrame.RegBase);
                        _currFrame = _frameStack.Pop();
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

                        _dataStack.Push(new NumberValue(a > b ? 1:0));
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




        float CastNumber( DataValue d )
        {
            var nv = d as NumberValue;
            if (nv == null)
            {
                Error("expect number");
                return 0;
            }

            return nv.Number;
        }

        int CastFuncIndex( DataValue d )
        {
            var fv = d as FuncValue;
            if ( fv == null )
            {
                Error("expect function");
                return -1;
            }

            return fv.Index;
        }

        void Error(string str)
        {
            throw new Exception(str);
        }
    }
}
