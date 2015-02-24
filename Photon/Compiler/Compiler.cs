using System;
using Photon.AST;
using Photon.OpCode;
using Photon.Scanner;

namespace Photon.Compiler
{
    public partial class Compiler
    {
        Executable _exe = new Executable();

        CommandSet _globalSet;
        CommandSet _currSet;

        public Compiler()
        {
           
        }


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

            CompileNode(_currSet, c.Block, false);

            _currSet.Add(new Command(Opcode.Exit));

            return _exe;
        }

        void CompileNode(CommandSet cm, Node n, bool lhs)
        {
            if (CompileExpr(cm, n, lhs))
                return;

            if (CompileDeclare(cm, n, lhs))
                return;

            if (CompileStmt(cm, n, lhs))
                return;
  
            Error("unsolved ast node: " + n.ToString());
            
        }


        static DataValue Lit2Const(BasicLit lite)
        {
            DataValue c = null;

            switch( lite.Type )
            {
                case TokenType.Number:
                    {
                        float v;
                        if ( !float.TryParse( lite.Value, out v) )
                            return null;

                        c = new NumberValue(v);
                    }
                    break;
            }


            return c;
        }
        
    }
}
