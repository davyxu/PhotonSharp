using System.Collections.Generic;
using Photon.OpCode;
using System;
using System.Diagnostics;
using Photon.AST;

namespace Photon.VM
{
    public partial class VMachine
    {
        DataStack _dataStack;

        Stack<RuntimeFrame> _frameStack = new Stack<RuntimeFrame>();

        RuntimeFrame _currFrame;

        Executable _exe;

        public VMachine( int maxstack = 10)
        {
            _dataStack = new DataStack(maxstack);            
        }

        public bool DebugRun
        {
            get;
            set;
        }

        public DataStack Stack
        {
            get { return _dataStack; }
        }

        void PushFrame( int funcIndex )
        {
            var newFrame = new RuntimeFrame(_exe.CmdSet[funcIndex] );

            if ( _currFrame != null )
            {
                _frameStack.Push(_currFrame);
            }

            _currFrame = newFrame;
        }

        void PopFrame( )
        {
            if ( _currFrame.RestoreDataStack )
            {
                _dataStack.Set(_currFrame.DataStackBase);
            }

            _currFrame = _frameStack.Pop();
        }

        void SetFrameReg(int regIndex, DataValue v)
        {
            _currFrame.Reg.Set(regIndex, v);
        }

        public DataValue GetFrameReg( int regIndex )
        {
            return _currFrame.Reg.Get(regIndex);
        }

        public void Run( Executable exe )
        {
            _frameStack.Clear();
            _dataStack.Clear();

            _exe = exe;

            PushFrame(0);

            while (_currFrame.PC < _currFrame.CmdSet.Commands.Count && _currFrame.PC != -1 )
            {
                var cmd = _currFrame.CmdSet.Commands[_currFrame.PC];

                if (DebugRun)
                {
                    Debug.WriteLine("{0} {1}: {2}", _currFrame.CmdSet.Name, _currFrame.PC, cmd.ToString());
                }

                ExecCommand(cmd);

                // 打印执行完后的信息
                if (DebugRun)
                {
                    // 数据栈信息
                    _dataStack.DebugPrint();

                    // 寄存器信息
                    _currFrame.Reg.DebugPrint();
                    Debug.WriteLine("");
                }

            }
        }

        public void DebugPrint( )
        {   
            _dataStack.DebugPrint();            
        }

        static bool IsValueNoneZero( DataValue d )
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


        static float CastNumber( DataValue d )
        {
            var nv = d as NumberValue;
            if (nv == null)
            {
                Error("expect number");
                return 0;
            }

            return nv.Number;
        }

        static int CastFuncIndex( DataValue d )
        {
            var fv = d as FuncValue;
            if ( fv == null )
            {
                Error("expect function");
                return -1;
            }

            return fv.Index;
        }

        static void Error(string str)
        {
            throw new Exception(str);
        }
    }
}
