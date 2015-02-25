using System.Collections.Generic;
using Photon.OpCode;
using System;
using System.Diagnostics;
using Photon.AST;

namespace Photon.VM
{
    struct RunEnv
    {
        public int PC;
        public int RegBase;
        public CommandSet CmdSet;
        public void Reset( CommandSet cs, int regbase )
        {
            PC = 0;
            CmdSet = cs;
            RegBase = regbase;
        }

        public override string ToString()
        {
            return string.Format("pc:{0}",PC );
        }
    }

    public class VMachine
    {
        DataStack _dataStack;
        Register _reg;
        Stack<RunEnv> _envStack = new Stack<RunEnv>();

        RunEnv _env;

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
            _envStack.Clear();
            _dataStack.Clear();
            _reg.Clear();
            _envStack.Clear();

            _exe = exe;
            _env.Reset(exe.CmdSet[0],0 );


            while (_env.PC < _env.CmdSet.Commands.Count && _env.PC != -1 )
            {
                var cmd = _env.CmdSet.Commands[_env.PC];

                if (DebugRun)
                {
                    Debug.WriteLine("{0} {1}: {2}", _env.CmdSet.Name, _env.PC, cmd.ToString());
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
            if (_env.CmdSet.ScopeInfo.Index == scopeIndex)
            {
                return _env.RegBase + regIndex;
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
                            _env.PC = targetPC;
                            return;
                        }

                    }
                    break;
                case Opcode.Jmp:
                    {
                        var targetPC = cmd.DataA;


                        _env.PC = targetPC;
                        return;
                    }
                case Opcode.Call:
                    {
                        var argCount = cmd.DataA;
                       

                        var funcIndex = CastFuncIndex(_dataStack.Peek(-argCount - 1));
                        var cs = _exe.GetCmdSet(funcIndex);

                        // 被调用函数的regbase=当前函数的最大分配量+当前基础偏移
                        int regbase = _env.CmdSet.ScopeInfo.AllocatedReg + _env.RegBase;

                        // 将栈转为被调用函数的寄存器
                        for( int i = 0;i< argCount ;i++)
                        {
                            var arg = _dataStack.Peek( -argCount + i);
                            _reg.Set( regbase + i , arg);
                        }

                        // 清空栈
                        _dataStack.PopMulti(argCount + 1);
                         
                        // 更换当前环境
                        _envStack.Push(_env);

                        _env.Reset(cs, regbase );
                        return;
                        
                    }
                case Opcode.Ret:
                    {
                        // 调试功能, 清空寄存器, 看起来清爽
                        _reg.ClearTo(_env.RegBase);
                        _env = _envStack.Pop();
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
                        _env.PC = -1;
                        return;
                    }
                    
            }

            _env.PC++;
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
