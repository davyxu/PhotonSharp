using System.Collections.Generic;
using Photon.Scanner;
using Photon.OpCode;

namespace Photon.AST
{
    public class Node
    {
        public Node Parent;

        public virtual IEnumerable<Node> Child()
        {
            yield break;
        }

        public virtual void Compile(Executable exe, CommandSet cm, bool lhs)
        {

        }

        public void BuildRelation( )
        {
            foreach( var c in Child() )
            {
                c.Parent = this;
            }
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
