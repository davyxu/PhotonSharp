using System;
using Photon.AST;
using Photon.OpCode;
using Photon.Scanner;


namespace Photon.Compiler
{
    public partial class Compiler
    {

        bool CompileStmt(CommandSet cm, Node n, bool lhs)
        {
            if (n is ReturnStmt)
            {
                var v = n as ReturnStmt;

                CompileNode(cm, v.Results[0], false);
                cm.Add(new Command(Opcode.Ret));
                return true;
            }


            if (n is BlockStmt)
            {
                var v = n as BlockStmt;


                foreach (var b in v.Stmts)
                {
                    CompileNode(cm, b, false);
                }

                return true;
            }

            return false;
        }
    }
}
