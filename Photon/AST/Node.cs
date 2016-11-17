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

        internal virtual void Compile(Package pkg, CommandSet cm, bool lhs)
        {
            throw new Exception("'Compile' not implement: " + this.ToString());
        }

        internal virtual void Resolve()
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
