using System.Collections.Generic;
using Photon.OpCode;
using System;
using System.Diagnostics;

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
            return string.Format("pc:{0} regbase:{1}",PC, RegBase);
        }
    }

    public class VMachine
    {
        const int MaxReg = 10;
        const int MaxStack = 10;
        DataStack _dataStack = new DataStack(MaxStack);
        Register _reg = new Register(MaxReg);
        Stack<RunEnv> _envStack = new Stack<RunEnv>();

        RunEnv _env;

        Executable _exe;

        public void Run( Executable exe )
        {
            _exe = exe;
            _env.Reset(exe.CmdSet[0], 0);


            while (_env.PC < _env.CmdSet.Commands.Count)
            {
                var cmd = _env.CmdSet.Commands[_env.PC];
                ExecCommand(cmd);
            }
        }

        public void DebugPrint( )
        {
            Debug.WriteLine("");
            _dataStack.DebugPrint();

            _reg.DebugPrint();
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
                        var r = _reg.Get(_env.RegBase + cmd.DataA);
                        _dataStack.Push(r);
                    }
                    break;
                case Opcode.SetR:
                    {
                        var d = _dataStack.Pop();
                        _reg.Set(_env.RegBase + cmd.DataA, d);
                    }
                    break;
                case Opcode.Call:
                    {
                        var argCount = cmd.DataA;
                        var regbase = cmd.DataB;

                        var funcIndex = CastFuncIndex(_dataStack.Peek(-argCount - 1));
                        var cs = _exe.GetCmdSet(funcIndex);


                        // 将栈转为被调用函数的寄存器
                        for( int i = 0;i< argCount ;i++)
                        {
                            var arg = _dataStack.Peek( -argCount + i);
                            _reg.Set( regbase + i , arg);
                        }

                        // 清空栈
                        _dataStack.PopMulti(argCount + 1);

                        _envStack.Push(_env);

                        _env.Reset(cs,  regbase);
                        return;
                        
                    }
                case Opcode.Ret:
                    {
                        // 调试功能, 清空寄存器, 看起来清爽
                        _reg.Clear(_env.RegBase);
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
                        var a = CastNumber(_dataStack.Pop());
                        var b = CastNumber(_dataStack.Pop());

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
                        var a = CastNumber(_dataStack.Pop());
                        var b = CastNumber(_dataStack.Pop());

                        _dataStack.Push(new NumberValue(a / b));
                    }
                    break;
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
