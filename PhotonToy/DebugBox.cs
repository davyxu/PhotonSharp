using Photon;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace PhotonToy
{
    enum DebuggerMode
    {
        Continue,
        StepIn,
        StepOut,
        StepOver,
    }



    class DebugBox
    {
        public Control _invoker;

        VMachine _vm;
        Executable _exe;

        Thread _thread;
        AutoResetEvent _debugSignal = new AutoResetEvent(false);
        AutoResetEvent _exitSignal = new AutoResetEvent(false);

        public Action<VMState> OnBreak;
        public Action<Executable, string> OnLoad;
        public Action<string> OnError;
       
        VarGuard<int> _expectCallDepth = new VarGuard<int>(-1);
        VarGuard<int> _callDepth = new VarGuard<int>(0);
        VarGuard<string> _regPackageName = new VarGuard<string>(string.Empty);
        VarGuard<DebuggerMode> _mode = new VarGuard<DebuggerMode>(DebuggerMode.StepIn);
        
        object _stateGuard = new object();

        public State State
        {
            get {

                lock (_stateGuard)
                {
                    if (_vm == null)
                        return State.None;

                    return _vm.State;
                }

            }
        }

        public DebugBox( Control invoker )
        {
            _invoker = invoker;
        }

        public SourceFile Source
        {
            get;
            set;
        }


        public void Start(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return;

            var pureName = Path.GetFileName(filename);

            try
            {
                _exe = Compiler.Compile(filename);
            }
            catch( Exception e )
            {
                if (OnError != null)
                    OnError(e.ToString());

                return;
            }
            _exe.DebugPrint();

            if (OnLoad != null)
            {
                OnLoad(_exe, filename);
            }

            _vm = new VMachine();
           // _vm.ShowDebugInfo = true;

            _mode.Value = DebuggerMode.StepIn;
            _thread = new Thread(VMThread);
            _thread.Name = "DebugBox";
            _thread.IsBackground = true;
            _thread.Start();
        }

        public void Stop( )
        {            
            _thread.Abort();

            
        }

        delegate void InvokeHandler(Action callback);
        void SafeCall(Action callback)
        {

            if (_invoker != null && _invoker.InvokeRequired)
            {
                _invoker.BeginInvoke(new InvokeHandler(SafeCall), callback);
            }
            else
            {
                callback();
            }
        }

        public void Operate( DebuggerMode hookmode )
        {
            if (_vm == null)
                return;

            _mode.Value = hookmode;
            switch ( hookmode )
            {
                case DebuggerMode.Continue:
                case DebuggerMode.StepIn:
                    {
                        _expectCallDepth.Value = -1;
                    }
                    break;
                case DebuggerMode.StepOver:
                    {
                        _expectCallDepth.Value = _callDepth.Value;
                    }
                    break;
                case DebuggerMode.StepOut:
                    {
                        _expectCallDepth.Value = _callDepth.Value - 1;
                    }
                    break;

            }

            _debugSignal.Set();
        }

        void CallBreak( VMachine vm, bool manulSwichPkgReg)
        {
            var cmd = vm.CurrFrame.GetCurrCommand();

            string pkgReg;
            if ( manulSwichPkgReg )
            {
                pkgReg = _regPackageName.Value;
            }
            else
            {
                if (vm.CallStack.Count > 1)
                {
                    pkgReg = string.Empty;
                }
                else
                {
                    pkgReg = vm.CurrFrame.CmdSet.Name.PackageName;
                }
            }

            var vms = new VMState(vm, pkgReg);

            SafeCall(delegate
            {
                if (OnBreak != null)
                {
                    OnBreak(vms);
                }

            });
        }

        public void SwitchRegister( string packageName )
        {
            if (_vm == null)
                return;
            _regPackageName.Value = packageName;

            CallBreak(_vm, true);
        }

        void VMThread( )
        {
            _vm.SetHook(DebugHook.AssemblyLine, (vm) =>
            {
                switch( _mode.Value )
                {
                    case DebuggerMode.Continue:
                        return;

                    case DebuggerMode.StepOver:
                        {
                            // 没有恢复到期望深度, 继续执行
                            if (_expectCallDepth.Value != _callDepth.Value)
                            {
                                return;
                            }
                        }
                        break;
                    case DebuggerMode.StepOut:
                        {
                            // 没有恢复到期望深度, 继续执行
                            if (_callDepth.Value > _expectCallDepth.Value)
                            {
                                return;
                            }

                        }
                        break;
                }

                _expectCallDepth.Value = -1;
                _mode.Value = DebuggerMode.StepIn;

                CallBreak(vm, false);

                _debugSignal.WaitOne();                
            });

            _vm.SetHook(DebugHook.Call, (vm) =>
            {
                _callDepth.Value++;
            });

            _vm.SetHook(DebugHook.Return, (vm) =>
            {
                _callDepth.Value--;
            });


            _vm.Run(_exe );

            CallBreak(_vm, false);

            _exitSignal.Set();
        }
    }
}
