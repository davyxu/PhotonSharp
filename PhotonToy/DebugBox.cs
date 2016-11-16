using Photon.API;
using Photon.Model;
using Photon.VM;
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

    class VMState
    {
        public AssemblyLocation Location;

    }


    class DebugBox
    {
        public Control _invoker;

        Script _script;
        Thread _thread;
        AutoResetEvent _debugSignal = new AutoResetEvent(false);
        AutoResetEvent _exitSignal = new AutoResetEvent(false);

        public Action<AssemblyLocation> OnBreak;
        public Action<VMachine> OnLoad;
       
        VarGuard<int> _expectCallDepth = new VarGuard<int>(-1);
        VarGuard<DebuggerMode> _mode = new VarGuard<DebuggerMode>(DebuggerMode.StepIn);
        
        object _stateGuard = new object();

        public State State
        {
            get {

                lock (_stateGuard)
                {
                    if (_script == null)
                        return Photon.VM.State.None;

                    return _script.VM.State;
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

            var file = new SourceFile(File.ReadAllText(filename));
            Source = file;

            _script = new Script();
            _script.Compile(file);
            _script.VM.ShowDebugInfo = true;

            if (OnLoad != null)
            {
                OnLoad(_script.VM);
            }


            _mode.Value = DebuggerMode.StepIn;
            _thread = new Thread(VMThread);
            _thread.Name = "DebugBox";
            _thread.IsBackground = true;
            _thread.Start();
        }



        public void Stop( )
        {
            switch( State )
            {
                case Photon.VM.State.Breaking:
                    {
                        _script.VM.Stop();

                        Operate(DebuggerMode.Continue);

                        _exitSignal.WaitOne();    
                    }
                    break;
                case Photon.VM.State.Running:
                    {
                        // 在跑, 直接停

                        _thread.Abort();
                    }
                    break;
            }


            
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
            if (_script == null)
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
                        _expectCallDepth.Value = _script.VM.CallStack.Count;
                    }
                    break;
                case DebuggerMode.StepOut:
                    {
                        _expectCallDepth.Value = _script.VM.CallStack.Count - 1;
                    }
                    break;

            }

            _debugSignal.Set();
        }

        void VMThread( )
        {
            _script.VM.SetHook(Photon.VM.DebugHook.ExecInstruction, (vm) =>
            {
                switch( _mode.Value )
                {
                    case DebuggerMode.Continue:
                        return;

                    case DebuggerMode.StepOver:
                        {
                            // 没有恢复到期望深度, 继续执行
                            if ( _expectCallDepth.Value != _script.VM.CallStack.Count )
                            {
                                return;
                            }
                        }
                        break;
                    case DebuggerMode.StepOut:
                        {
                            // 没有恢复到期望深度, 继续执行
                            if (_script.VM.CallStack.Count > _expectCallDepth.Value )
                            {
                                return;
                            }

                        }
                        break;
                }

                _expectCallDepth.Value = -1;
                _mode.Value = DebuggerMode.StepIn;

                var al = new AssemblyLocation(vm.CurrFrame.CmdSet.ID, vm.CurrFrame.PC);
                SafeCall(delegate
                {
                    if (OnBreak != null)
                    {
                        OnBreak(al);
                    }

                });

                _debugSignal.WaitOne();                
            });

            _script.Run();
            
            var existAL = new AssemblyLocation(_script.VM.CurrFrame.CmdSet.ID, _script.VM.CurrFrame.PC);

            SafeCall(delegate
            {
                if (OnBreak != null)
                {

                    OnBreak(existAL);
                }

            });

            _exitSignal.Set();
        }
    }
}
