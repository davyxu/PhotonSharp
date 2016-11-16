using Photon.Parser;
using Photon.VM;
using Photon.Model;
using System.Diagnostics;
using System;
using SharpLexer;

namespace Photon.API
{
   
    public class Script
    {
        ScriptParser _parser = new ScriptParser();        
        VMachine _vm = new VMachine();
        Executable _exe = new Executable();
        SourceFile _file;

        bool _debugMode;
        public bool DebugMode
        {
            get { return _debugMode; }
            set
            {
                _debugMode = value;
                _vm.ShowDebugInfo = value;
            }
        }

        public VMachine VM
        {
            get { return _vm; }
        }


        public void Compile( SourceFile file )
        {
            _file = file;

            if (_debugMode)
            {
                file.DebugPrint();
            }

            // 编译生成AST
            var chunk = _parser.ParseSource(file.Source);

            if (_debugMode)
            {
                Debug.WriteLine("ast:");
                ScriptParser.PrintAST(chunk);
                Debug.WriteLine("");

                _parser.PrintScopeSymbol();
            }

            
            var exe = new Executable();

            var cmdSet = new CommandSet("global", TokenPos.Init, _parser.GlobalScope.CalcUsedReg(), true);

            _exe.AddCmdSet(cmdSet);

            // 遍历AST,生成代码
            chunk.Compile(_exe, cmdSet, false);

            cmdSet.Add(new Command(Opcode.Exit));

            _vm.Attach(_exe);

            InitAux();

            if (_debugMode)
            {
                _exe.DebugPrint(_file);
            }
        }

        void InitAux( )
        {
            RegisterDelegate("array", (vm) =>
            {
                vm.DataStack.Push(new ValueArray() );

                return 1;
            });
        }

        public void Run( )
        {
            if (_debugMode)
            {
                Debug.WriteLine("");
            }

            _vm.Run(_file);

            if ( _debugMode )
            {
                _vm.DataStack.DebugPrint();
            }
            
        }

        public void RegisterDelegate(string name, Func<Photon.VM.VMachine, int> callback)
        {
            ValueDelegate v = null;
            if (!_exe.DelegateMap.TryGetValue( name, out v ))            
            {
                Debug.WriteLine("extern func not define in code: " + name);
                return;
            }

            v.Entry = callback;       
        }

    }
}
