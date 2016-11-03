using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.AST
{
    // a.b   x=a  selector=b
    public class SelectorExpr : Expr
    {
        public Ident Selector;
        public Expr X;
        public SelectorExpr(Expr x, Ident i)
        {
            X = x;
            Selector = i;
            BuildRelation();

        }

        public override IEnumerable<Node> Child()
        {
            yield return X;

            yield return Selector;
        }

        public override string ToString()
        {
            return "SelectorExpr";
        }
    }
}
