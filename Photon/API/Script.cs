using Photon.Parser;
using Photon.Compiler;
using Photon.VM;
using Photon.OpCode;
using System.Diagnostics;

namespace Photon.API
{
    public class Script
    {
        ScriptParser _parser = new ScriptParser();
        ScriptCompiler _compiler = new ScriptCompiler();
        VMachine _vm = new VMachine();

        bool _debugMode;
        public bool DebugMode{
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

        public Executable Compile( string src )
        {
            if (_debugMode)
            {
                Debug.WriteLine(src);
            }
            var chunk = _parser.ParseSource(src);

            if (_debugMode)
            {
                ScriptParser.DebugPrint(chunk);
            }

            var exe = _compiler.Walk(chunk, _parser.ScopeInfoSet);

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
