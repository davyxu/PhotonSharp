using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Photon
{
    public enum State
    {
        None = 0,
        Standby ,
        Running,
        Breaking,
    }

    public enum DebugHook
    {        
        AssemblyLine,           // 每个指令运行前
        SourceLine,             // 源码行执行前
        Call,                   // 函数执行前
        Return,                 // 返回函数后
        MAX,
    }

    public partial class VMachine
    {
        // 数据交换栈
        DataStack _dataStack = new DataStack(10);

        // 数据寄存器
        Register _localReg = new Register("R", 10);

        // 包内存
        List<RuntimePackage> _package = new List<RuntimePackage>();

        // 运行帧栈
        Stack<RuntimeFrame> _callStack = new Stack<RuntimeFrame>();

        // 当前运行帧
        RuntimeFrame _currFrame;

        Action<VMachine>[] _hook = new Action<VMachine>[(int)DebugHook.MAX];

        
        // 运行状态
        State _state = State.Standby;

        struct RegRange
        {
            public int Min;
            public int Max;
        }

        // 寄存器使用范围栈
        Stack<RegRange> _regBaseStack = new Stack<RegRange>();
        int _regBase = 0;

        Executable _exe;        

        // 指令集
        Instruction[] _instruction = new Instruction[(int)Opcode.MAX];

        // 当前寄存器最小使用位置
        internal int RegBase
        {
            get { return _regBase; }
        }

        public State State 
        {
            get { return _state; }
        }

        public bool ShowDebugInfo
        {
            get;
            set;
        }

        public DataStack DataStack
        {
            get { return _dataStack; }
        }

        public Register LocalReg
        {
            get { return _localReg; }
        }

        public Executable Exec
        {
            get { return _exe; }
        }

        public RuntimeFrame CurrFrame
        {
            get { return _currFrame; }
        }

        public Stack<RuntimeFrame> CallStack
        {
            get { return _callStack; }
        }

        public Command GetCurrCommand( )
        {

            int pc = _currFrame.PC;
            if (pc >= _currFrame.CmdSet.Commands.Count || pc < 0)
            {
                return null;
            }

            return _currFrame.CmdSet.Commands[pc];
        }

        public RuntimePackage GetRuntimePackage(int pkgid)
        {
            return _package[pkgid];
        }

        public RuntimePackage GetRuntimePackageByName(string name)
        {
            foreach( var pkg in _package )
            {
                if (pkg.Name == name)
                    return pkg;
            }

            return null;
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

        void ExecCode(Command cmd)
        {
            var inc = _instruction[(int)cmd.Op];

            if (inc == null)
            {
                throw new RuntimeExcetion("invalid instruction");                
            }


            if( inc.Execute( cmd) )
            {
                _currFrame.PC++;
            }
        }

        public void SetHook(DebugHook hook, Action<VMachine> callback )
        {
            _hook[(int)hook] = callback;
        }

        internal void EnterFrame( CommandSet cmdSet )
        {
            CallHook(DebugHook.Call);

            var newFrame = new RuntimeFrame(cmdSet);

            if ( _currFrame != null )
            {
                _callStack.Push(_currFrame);
            }

            _currFrame = newFrame;
            
            if ( !_currFrame.CmdSet.IsGlobal)
            {
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
  
        }

        internal void LeaveFrame()
        {
            if ( _currFrame.RestoreDataStack )
            {
                _dataStack.Count = _currFrame.DataStackBase;
            }
                        
            if (!_currFrame.CmdSet.IsGlobal)
            {
                _regBaseStack.Pop();

                if (_regBaseStack.Count > 0)
                {
                    var rr = _regBaseStack.Peek();
                    _regBase = rr.Min;

                    _localReg.SetUsedCount(rr.Max);
                }
                else
                {
                    _localReg.SetUsedCount(0);
                }
                   
            }

            _currFrame = _callStack.Pop();


            CallHook(DebugHook.Return);
        }

        void CallHook( DebugHook hook )
        {
            var func = _hook[(int)hook];
            if (func != null)
            {
                _state = State.Breaking;

                func(this);

                _state = State.Running;
            }
        }

        public void Stop( )
        {
            _currFrame.PC = -1;
        }

        

        public void Run(Executable exe)
        {
            if ( ShowDebugInfo )
            {
                Debug.WriteLine("============ VM Start ============");
            }

            _exe = exe;
            _callStack.Clear();
            _dataStack.Clear();

            // 包全局寄存器初始化
            for( int i = 0;i<exe.PackageCount ;i++)
            {
                var pkg = exe.GetPackage(i);                
                _package.Add(new RuntimePackage(pkg.Name));
            }

            var cs = exe.GetProcedure(0, 0) as CommandSet;
            GetRuntimePackage(0).Reg.SetUsedCount(cs.RegCount);                 

            EnterFrame(cs);

            int currSrcLine = 0;

            _state = State.Running;
            
            while (true)
            {
                var cmd = GetCurrCommand();
                if (cmd == null)
                    break;

                if (ShowDebugInfo)
                {
                    
                   // Debug.WriteLine("{0}|{1}", cmd.CodePos.Line, exe.Source.GetLine(cmd.CodePos.Line));
                    Debug.WriteLine("---------------------");
                    Debug.WriteLine("{0,5} {1,2}| {2} {3}", _currFrame.CmdSet.Name, _currFrame.PC, cmd.Op.ToString(), InstructToString(cmd) );
                }

                // 源码行有变化时
                if (currSrcLine == 0 || currSrcLine != cmd.CodePos.Line)
                {
                    if ( currSrcLine != 0 )
                    {
                        CallHook(DebugHook.SourceLine);
                    }
                    
                    currSrcLine = cmd.CodePos.Line;
                }


                // 每条指令执行前
                CallHook(DebugHook.AssemblyLine);


                ExecCode(cmd);

                // 打印执行完后的信息
                if (ShowDebugInfo)
                {

                    GetRuntimePackage(0).Reg.DebugPrint();

                    // 寄存器信息
                    LocalReg.DebugPrint();

                    
                    // 数据栈信息
                    DataStack.DebugPrint();

                    Debug.WriteLine("");
                }

            }



            if (ShowDebugInfo)
            {
                Debug.WriteLine("============ VM End ============");
            }
        }
    }
}
