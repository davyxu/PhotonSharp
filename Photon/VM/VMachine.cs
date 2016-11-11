using System.Collections.Generic;
using Photon.Model;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Photon.VM
{


    public partial class VMachine
    {
        DataStack _dataStack = new DataStack(10);       

        Stack<RuntimeFrame> _frameStack = new Stack<RuntimeFrame>();

        RuntimeFrame _currFrame;        

        Register _localReg = new Register("R", 10);

        struct RegRange
        {
            public int Min;
            public int Max;
        }

        Stack<RegRange> _regBaseStack = new Stack<RegRange>();
        int _regBase = 0;

        Executable _exe;        

        Instruction[] _instruction = new Instruction[(int)Opcode.MAX];


        public int RegBase
        {
            get { return _regBase; }
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

        public Register LocalRegister
        {
            get { return _localReg; }
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

                var cmd = Activator.CreateInstance(t) as Instruction;
                cmd.vm = this;
                _instruction[(int)att.Cmd] = cmd;                
            }
        }

        string InstructToString( Command cmd )
        {
            var inc = _instruction[(int)cmd.Op];

            if (inc == null)
            {            
                return string.Empty;
            }

            return inc.Print( cmd );
        }

        bool ExecCode(Command cmd)
        {
            var inc = _instruction[(int)cmd.Op];

            if (inc == null)
            {
                throw new RuntimeExcetion("invalid instruction");                
            }


            return inc.Execute( cmd);
        }

        public void EnterFrame( int funcIndex )
        {
            var newFrame = new RuntimeFrame(_exe.CmdSet[funcIndex] );

            if ( _currFrame != null )
            {
                _frameStack.Push(_currFrame);
            }

            _currFrame = newFrame;

            // globa不用local寄存器


            // 第一层的reg是0, 不记录
            if (_regBaseStack.Count > 0)
            {
                _regBase = _regBaseStack.Peek().Max;
            }

            RegRange rr;
            rr.Min = _regBase;
            rr.Max = _regBase + newFrame.CmdSet.RegCount;

            _localReg.SetUsedCount(rr.Max);

            // 留作下次调用叠加时使用
            _regBaseStack.Push(rr);
        }

        public void LeaveFrame( )
        {
            if ( _currFrame.RestoreDataStack )
            {
                _dataStack.Count = _currFrame.DataStackBase;
            }

            _currFrame = _frameStack.Pop();

            _regBaseStack.Pop();

            var rr = _regBaseStack.Peek();
            _regBase = rr.Min;

            _localReg.SetUsedCount(rr.Max);
        }



        public void Run( Executable exe, SourceFile file )
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
                    Debug.WriteLine("{0}|{1}", cmd.CodePos.Line, file.GetLine(cmd.CodePos.Line));
                    Debug.WriteLine("{0,5} {1,2}| {2} {3}", _currFrame.CmdSet.Name, _currFrame.PC, cmd.Op.ToString(), InstructToString(cmd) );
                }

                if ( ExecCode(cmd) )
                {
                    _currFrame.PC++;
                }

                // 打印执行完后的信息
                if (DebugRun)
                {
                    // 寄存器信息
                    LocalRegister.DebugPrint();

                    // 数据栈信息
                    Stack.DebugPrint();
                    

                    Debug.WriteLine("");
                }

            }
        }
    }
}
