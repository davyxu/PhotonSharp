using System.Collections.Generic;
using System;

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
        DataStack _dataStack = new DataStack();

        // 包内存
        List<RuntimePackage> _package = new List<RuntimePackage>();

        // 运行帧栈
        Stack<RuntimeFrame> _callStack = new Stack<RuntimeFrame>();

        // 当前运行帧
        RuntimeFrame _currFrame;

        Action<VMachine>[] _hook = new Action<VMachine>[(int)DebugHook.MAX];

        
        // 运行状态
        State _state = State.Standby;

        Executable _exe;

        InstructionSet _insset;

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

        internal void MoveArgStack2Local(int argCount)
        {
            // 将栈转为被调用函数的寄存器
            for (int i = 0; i < argCount; i++)
            {
                var arg = DataStack.Get(-i - 1);
                LocalReg.Set(argCount - i - 1, arg);
            }
        }

        internal void EnterFrame( ValuePhoFunc func )
        {
            CallHook(DebugHook.Call);

            var newFrame = new RuntimeFrame(func);

            _currFrame = newFrame;

            _callStack.Push(_currFrame);
                               
            LocalReg.SetUsedCount(newFrame.Func.RegCount);

            LocalReg.AttachScope(func.Scope);  
        }

        internal void LeaveFrame()
        {
            // -1表示多返回值传递
            if ( _currFrame.ReceiverCount != -1 )
            {
                _dataStack.Adjust(_currFrame.DataStackBase, _currFrame.ReceiverCount );                
            }

            _callStack.Pop();

            if (_callStack.Count > 0) 
            {
                _currFrame = _callStack.Peek();
            }
            else
            {
                // 单独运行函数后, 结束
                _currFrame = null;
            }
            
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


        internal void ExecuteFunc(RuntimePackage rtpkg, ValuePhoFunc func, int argCount, int retValueCount)
        {
            if (ShowDebugInfo)
            {
                Logger.DebugLine(string.Format("============ Run entry{0} ============", func.Name));
            }

            rtpkg.Reg.SetUsedCount(func.RegCount);

            EnterFrame(func);

            CurrFrame.ReceiverCount = retValueCount;

            if (argCount > 0)
            {
                MoveArgStack2Local(argCount);
                _dataStack.Clear();
            }

            int currSrcLine = 0;

            _state = State.Running;
            
            while (true)
            {
                var cmd = CurrFrame.GetCurrCommand();
                if (cmd == null)
                    break;

                if (ShowDebugInfo)
                {
                    
                    Logger.DebugLine("{0}|{1}", cmd.CodePos, _exe.QuerySourceLine(cmd.CodePos));
                    Logger.DebugLine("---------------------");
                    Logger.DebugLine("{0,5} {1,2}| {2} {3}", _currFrame.Func.Name, _currFrame.PC, cmd.Op.ToString(), _insset.InstructToString(cmd) );
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

                if (_insset.ExecCode(this, cmd))
                {
                    if (_currFrame == null)
                        break;

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

                    Logger.DebugLine("");
                }

            }


            if (ShowDebugInfo)
            {
                Logger.DebugLine("============ VM End ============");
            }
        

        }

        public object[] Execute(Executable exe, string pkgname, string entryName, object[] paramToExec = null, int retValueCount = 0)
        {
            _exe = exe;
            _callStack.Clear();
            _dataStack.Clear();

            if (paramToExec != null)
            {
                foreach (var obj in paramToExec)
                {
                    var v = Convertor.NativeValueToValue(obj);
                    _dataStack.Push(v);
                }
            }
            

            // 包全局寄存器初始化
            // TODO 按import顺序, 顺序初始, 调用init
            for (int i = 0; i < exe.PackageCount; i++)
            {
                var pkg = exe.GetPackage(i);
                _package.Add(new RuntimePackage(pkg));
            }


            // 找到包入口
            var func = _exe.GetFuncByName(new ObjectName(pkgname, entryName)) as ValuePhoFunc;
            if (func == null)
            {
                throw new RuntimeException("unknown start package name: " + pkgname);
            }

            var rtpkg = GetRuntimePackageByName(pkgname);

            var argCount = paramToExec != null ? paramToExec.Length:0;            

            ExecuteFunc(rtpkg, func, argCount, retValueCount);

            if (retValueCount > 0 )
            {
                var retValue = new object[retValueCount];
                for (int i = 0; i < retValueCount; i++)
                {
                     retValue[i] = Convertor.ValueToNativeValue( DataStack.Get(-(i + 1)) );
                }

                return retValue;
            }

            return null;

            
        }  
    }
}
