using System.Collections.Generic;
using Photon.Model;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Photon.VM
{
    using InstructionExecFunc = Func<VMachine, Command, bool>;
    using InstructionPrintFunc = Func<VMachine, Command, string>;
    
    class InstructionAttribute : Attribute
    {
        public Opcode Cmd
        {
            get;
            set;
        }
    }


    public partial class VMachine
    {
        DataStack _dataStack = new DataStack(10);       

        Stack<RuntimeFrame> _frameStack = new Stack<RuntimeFrame>();

        RuntimeFrame _currFrame;

        Register _globalReg = new Register("G", 10);

        Executable _exe;

        InstructionExecFunc[] _instructionExec = new InstructionExecFunc[(int)Opcode.MAX];
        InstructionPrintFunc[] _instructionPrint = new InstructionPrintFunc[(int)Opcode.MAX];

        public bool DebugRun
        {
            get;
            set;
        }

        public DataStack Stack
        {
            get { return _dataStack; }
        }

        public Register GlobalRegister
        {
            get { return _globalReg; }
        }

        public Register LocalRegister
        {
            get { return _currFrame.Reg; }
        }

        public Executable Executable
        {
            get { return _exe; }
        }

        public RuntimeFrame CurrFrame
        {
            get { return _currFrame; }
        }

        public VMachine()
        {
            StaticRegisterAssemblyInstructions();
        }

        void StaticRegisterAssemblyInstructions()
        {
            var ass = Assembly.GetExecutingAssembly();

            foreach (var t in ass.GetTypes())
            {
                var att = t.GetCustomAttribute<InstructionAttribute>();
                if (att == null)
                    continue;

                MethodInfo exe = t.GetMethod("Execute", BindingFlags.Public | BindingFlags.Static);
                _instructionExec[(int)att.Cmd] = exe.CreateDelegate(typeof(InstructionExecFunc)) as InstructionExecFunc;

                MethodInfo pt = t.GetMethod("Print", BindingFlags.Public | BindingFlags.Static);
                if (pt != null )
                {
                    _instructionPrint[(int)att.Cmd] = pt.CreateDelegate(typeof(InstructionPrintFunc)) as InstructionPrintFunc;
                }
                
            }
        }

        string InstructToString( Command cmd )
        {
            var inc = _instructionPrint[(int)cmd.Op];

            if (inc == null)
            {            
                return string.Empty;
            }

            return inc( this, cmd );
        }

        bool ExecCode(Command cmd)
        {
            var inc = _instructionExec[(int)cmd.Op];

            if (inc == null)
            {
                Error("invalid instruction");
                return false;
            }


            return inc(this, cmd);
        }

        public void EnterFrame( int funcIndex )
        {
            var newFrame = new RuntimeFrame(_exe.CmdSet[funcIndex] );

            if ( _currFrame != null )
            {
                _frameStack.Push(_currFrame);
            }

            _currFrame = newFrame;
        }

        public void LaveFrame( )
        {
            if ( _currFrame.RestoreDataStack )
            {
                _dataStack.Count = _currFrame.DataStackBase;
            }

            _currFrame = _frameStack.Pop();
        }



        public void Run( Executable exe )
        {
            _frameStack.Clear();
            _dataStack.Clear();

            _exe = exe;

            EnterFrame(0);

            while (_currFrame.PC < _currFrame.CmdSet.Commands.Count && _currFrame.PC != -1 )
            {
                var cmd = _currFrame.CmdSet.Commands[_currFrame.PC];

                if (DebugRun)
                {
                    Debug.WriteLine("{0,5} {1,2}| {2} {3}", _currFrame.CmdSet.Name, _currFrame.PC, cmd.Op.ToString(), InstructToString(cmd) );
                }

                if ( ExecCode(cmd) )
                {
                    _currFrame.PC++;
                }

                // 打印执行完后的信息
                if (DebugRun)
                {
                    // 数据栈信息
                    _dataStack.DebugPrint();

                    // 寄存器信息
                    _currFrame.Reg.DebugPrint();

                    // 全局寄存器
                    _globalReg.DebugPrint();

                    Debug.WriteLine("");
                }

            }
        }


        public static bool IsValueNoneZero( Value d )
        {
            if (d == null)
                return false;

            var nv = d as ValueNumber;

            if ( nv != null )
            {
                return nv.Number != 0;
            }

            var fv = d as ValueFunc;

            if ( fv != null )
            {
                return true;
            }


            Error("unknown value type");


            return false;
        }


        public static float CastNumber( Value d )
        {
            var nv = d as ValueNumber;
            if (nv == null)
            {
                Error("expect number");
                return 0;
            }

            return nv.Number;
        }

        public static ValueObject CastObject(Value d)
        {
            var nv = d as ValueObject;
            if (nv == null)
            {
                Error("expect object");
                return null;
            }

            return nv;
        }

        public static int CastFuncIndex( Value d )
        {
            var fv = d as ValueFunc;
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
