using System;
using Photon.AST;
using Photon.OpCode;
using Photon.Scanner;
using System.Collections.Generic;

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

        public Executable Walk( Chunk c, ScopeSet ss )
        {
            _currSet = _globalSet = new CommandSet("global", ss.Get(0));
            
            _exe.AddCmdSet(_currSet);

            ss.BuildRegbase();
            _exe.ScopeInfoSet = ss;

            c.Block.Compile(_exe, _currSet, false);            

            _currSet.Add(new Command(Opcode.Exit));

            return _exe;
        }
    }
}
