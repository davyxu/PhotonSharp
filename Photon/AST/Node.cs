using System.Collections.Generic;
using System;

namespace Photon
{



    public class Node
    {
        public Node Parent;

        public virtual IEnumerable<Node> Child()
        {
            yield break;
        }

        internal virtual void Compile(CompileParameter param)
        {
            throw new Exception("'Compile' not implement: " + this.ToString());
        }

        internal virtual void Resolve(CompileParameter param)
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
