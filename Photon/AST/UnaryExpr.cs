
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    internal class UnaryExpr : Expr
    {
        public TokenType Op;
        public Expr X;

        public TokenPos OpPos;

        public UnaryExpr(Expr x, TokenType t, TokenPos oppos)
        {
            X = x;
            Op = t;
            OpPos = oppos;

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            yield return X;
        }
    }

}
