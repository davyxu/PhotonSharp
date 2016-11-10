using Photon.Parser;
using Photon.VM;
using Photon.Model;
using System.Diagnostics;
using System;

namespace Photon.API
{
   
    public class Script
    {
        ScriptParser _parser = new ScriptParser();        
        VMachine _vm = new VMachine();
        Executable _exe = new Executable();

        bool _debugMode;
        public bool DebugMode
        {
            get { return _debugMode; }
            set
            {
                _debugMode = value;
                _vm.DebugRun = value;
            }
        }

        public VMachine VM
        {
            get { return _vm; }
        }

        public static void PrintSource( string src )
        {
            Debug.WriteLine("source:");
            var lines = src.Split('\r');
            int lineCount  = 1;
            foreach( var line in lines )
            {
                string trimedLine;
                if (line.Length > 0 && line[0] == '\n' )
                {
                    trimedLine  = line.Substring(1);
                }else{
                    trimedLine  = line;
                }

                Debug.Print("{0} {1}", lineCount, trimedLine);
                lineCount ++;
            }

            Debug.WriteLine("");
        }

        public void Compile( string src )
        {
            if (_debugMode)
            {
                PrintSource(src);
            }

            // 编译生成AST
            var chunk = _parser.ParseSource(src);

            if (_debugMode)
            {
                Debug.WriteLine("ast:");
                ScriptParser.PrintAST(chunk);
                Debug.WriteLine("");

                _parser.PrintScopeSymbol();
            }

            
            var exe = new Executable();

            var cmdSet = new CommandSet("global", _parser.GlobalScope.CalcUsedReg(), true);

            _exe.AddCmdSet(cmdSet);

            // 遍历AST,生成代码
            chunk.Compile(_exe, cmdSet, false);

            cmdSet.Add(new Command(Opcode.Exit));


            InitAux();

            if (_debugMode)
            {
                _exe.DebugPrint();
            }
        }

        void InitAux( )
        {
            RegisterDelegate("array", (vm) =>
            {
                vm.Stack.Push(new ValueArray() );

                return 1;
            });
        }

        public void Run( )
        {
            if (_debugMode)
            {
                Debug.WriteLine("");
            }

            _vm.Run(_exe);

            if ( _debugMode )
            {
                _vm.Stack.DebugPrint();
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
