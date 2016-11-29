using System.Collections.Generic;
using System;
using System.Diagnostics;

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

    public delegate int DelegateEntry(VMachine vm);

    public sealed class DelegateAttribute : Attribute
    {
        public DelegateAttribute(Type type)
        {
        }
    }

    public partial class VMachine
    {
        // 数据交换栈
        DataStack _dataStack = new DataStack(10);

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

        InstructionSet _insset;

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
            get { return _currFrame.Reg; }
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
            _insset = new InstructionSet(this);
        }

        public void SetHook(DebugHook hook, Action<VMachine> callback )
        {
            _hook[(int)hook] = callback;
        }

        internal void EnterFrame( CommandSet cmdSet )
        {
            CallHook(DebugHook.Call);

            var newFrame = new RuntimeFrame(cmdSet);

            _currFrame = newFrame;

            _callStack.Push(_currFrame);
            
            //if ( !_currFrame.CmdSet.IsGlobal)
            {
                // 第一层的reg是0, 不记录
                //if (_regBaseStack.Count > 0)
                //{
                //    _regBase = _regBaseStack.Peek().Max;
                //}

                RegRange rr;
                rr.Min = _regBase;
                rr.Max = _regBase + newFrame.CmdSet.RegCount;

                LocalReg.SetUsedCount(rr.Max);

                // 留作下次调用叠加时使用
                //_regBaseStack.Push(rr);
            }
  
        }

        internal void LeaveFrame()
        {
            if ( _currFrame.RestoreDataStack )
            {
                _dataStack.Count = _currFrame.DataStackBase;
            }
                        
            //if (!_currFrame.CmdSet.IsGlobal)
            //{
                //_regBaseStack.Pop();

                //if (_regBaseStack.Count > 0)
                //{
                //    var rr = _regBaseStack.Peek();
                //    _regBase = rr.Min;

                //    LocalReg.SetUsedCount(rr.Max);
                //}
                //else
                //{
                //    LocalReg.SetUsedCount(0);
                //}
                   
           // }

            _callStack.Pop();

            _currFrame = _callStack.Peek();


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

       
        public void Run(Executable exe, string startPkg = "main" )
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
                _package.Add(new RuntimePackage(pkg));
            }
            
            // 找到包入口
            var proc = exe.GetProcedureByName( new ProcedureName("main", "main") );
            if ( proc == null )
            {
                throw new RuntimeException("unknown start package name: " + startPkg);
            }

            var cs = proc as CommandSet;
            GetRuntimePackageByName("main").Reg.SetUsedCount(cs.RegCount);

            EnterFrame(cs);

            int currSrcLine = 0;

            _state = State.Running;
            
            while (true)
            {
                var cmd = CurrFrame.GetCurrCommand();
                if (cmd == null)
                    break;

                if (ShowDebugInfo)
                {
                    
                    Debug.WriteLine("{0}|{1}", cmd.CodePos.Line, _exe.QuerySourceLine(cmd.CodePos));
                    Debug.WriteLine("---------------------");
                    Debug.WriteLine("{0,5} {1,2}| {2} {3}", _currFrame.CmdSet.Name, _currFrame.PC, cmd.Op.ToString(), _insset.InstructToString(cmd) );
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

                if (_insset.ExecCode(cmd))
                {
                    _currFrame.PC++;
                }

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
