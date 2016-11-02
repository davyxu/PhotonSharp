using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.AST
{
    public class IndexExpr : Expr
    {
        public Expr X;
        public Expr Index;

        public IndexExpr(Expr x, Expr index)
        {
            X = x;
            Index = index;
        }

        public override IEnumerable<Node> Child()
        {
            yield return X;

            yield return Index;
        }

        public override string ToString()
        {
            return "IndexExpr";
        }
    }
}
