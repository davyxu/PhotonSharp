using System;
using Photon.AST;
using Photon.OpCode;

namespace Photon.Compiler
{
    public partial class ScriptCompiler
    {
        Executable _exe = new Executable();

        CommandSet _globalSet;
        CommandSet _currSet;


        void Error(string str)
        {
            throw new Exception(str);
        }

        public Executable Walk( Chunk c )
        {
            _currSet = _globalSet = new CommandSet("global" );
            
            _exe.AddCmdSet(_currSet);

            c.Block.Compile(_exe, _currSet, false);            

            _currSet.Add(new Command(Opcode.Exit));

            return _exe;
        }
    }
}
