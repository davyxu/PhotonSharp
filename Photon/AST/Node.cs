using System.Collections.Generic;
using Photon.Scanner;
using Photon.OpCode;

namespace Photon.AST
{
    public class Node
    {
        public virtual IEnumerable<Node> Child()
        {
            yield break;
        }

        public virtual void Compile(Executable exe, CommandSet cm, bool lhs)
        {

        }
    }


    public class Stmt : Node
    {

    }

    public class BadStmt : Stmt
    {

    }

    public class Expr : Node
    {

    }

    public class BadExpr : Expr
    {

    }




   






    


   
}
