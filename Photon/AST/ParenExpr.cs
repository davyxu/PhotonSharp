
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    internal class ParenExpr : Expr
    {   
        public TokenPos LParenPos;
        public Expr X;
        public TokenPos RParenPos;


        public ParenExpr(Expr x, TokenPos lparen, TokenPos rparen)
        {
            X = x;
            LParenPos = lparen;
            RParenPos = rparen;

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            yield return X;
        }

        public override string ToString()
        {
            return string.Format("ParenExpr");
        }


        internal override void Compile(CompileParameter param)
        {
            X.Compile(param);
        }
    }

}
