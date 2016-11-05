using Photon.Parser;
using Photon.VM;
using Photon.OpCode;
using System.Diagnostics;
using System.IO;

namespace Photon.API
{
    public class Script
    {
        ScriptParser _parser = new ScriptParser();        
        VMachine _vm = new VMachine();

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

        public Executable Compile( string src )
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

            var cmdSet = new CommandSet("global");

            exe.AddCmdSet(cmdSet);

            // 遍历AST,生成代码
            chunk.Block.Compile(exe, cmdSet, false);

            cmdSet.Add(new Command(Opcode.Exit));


            if (_debugMode)
            {
                exe.DebugPrint();
            }

            return exe;
        }

        public void Run( Executable exe )
        {
            if (_debugMode)
            {
                Debug.WriteLine("");
            }

            _vm.Run(exe);

            if ( _debugMode )
            {
                _vm.DebugPrint();
            }
            
        }
    }
}
