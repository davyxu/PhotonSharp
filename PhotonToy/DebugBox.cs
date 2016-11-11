using Photon.API;
using Photon.Model;
using Photon.VM;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace PhotonToy
{
    class DebugBox
    {
        public Control _invoker;

        Script _script = new Script();
        Thread _thread;
        AutoResetEvent _debugSignal = new AutoResetEvent(false);
        AutoResetEvent _exitSignal = new AutoResetEvent(false);

        public Action<VMachine> OnBreak;
        public Action<VMachine> OnLoad;
        
        object _stateGuard = new object();

        public State State
        {
            get {

                lock (_stateGuard)
                {
                    return _script.VM.State;
                }

            }
        }

        public DebugBox( Control invoker )
        {
            _invoker = invoker;
        }

        public void Load(string filename)
        {
            var file = new SourceFile(File.ReadAllText(filename) );
            Source = file;

            _script.Compile(file);

            if ( OnLoad != null )
            {
                OnLoad(_script.VM);
            }
        }

        public SourceFile Source
        {
            get;
            set;
        }


        public void Start()
        {
            _thread = new Thread(VMThread);
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

                        Resume();

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

        public void Resume( )
        {
            _debugSignal.Set();
        }


        void VMThread( )
        {
            _script.VM.SetHook(Photon.VM.DebugHook.ExecInstruction, (vm) =>
            {
                SafeCall(delegate
                {
                    if (OnBreak != null)
                    {                           
                        OnBreak(vm);
                    }
                    
                });

                _debugSignal.WaitOne();
            });

            _script.Run();

            _exitSignal.Set();
        }
    }
}
