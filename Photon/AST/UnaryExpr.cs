using Photon.Scanner;
using System.Collections.Generic;

namespace Photon.AST
{
    public class UnaryExpr : Expr
    {
        public TokenType Op;
        public Expr X;
        public UnaryExpr(Expr x, TokenType t)
        {
            X = x;
            Op = t;
        }

        public override IEnumerable<Node> Child()
        {
            yield return X;
        }
    }

}
