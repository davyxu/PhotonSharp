
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    // a.b   x=a  index=b

    internal class IndexExpr : Expr
    {
        public Expr X;
        public Expr Index;

        public TokenPos LBrackPos;
        public TokenPos RBrackPos;

        public IndexExpr(Expr x, Expr index, TokenPos lpos, TokenPos rpos)
        {
            X = x;
            Index = index;
            LBrackPos = lpos;
            RBrackPos = rpos;

            BuildRelation();
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

        internal override void Compile(CompileParameter param)
        {
            Index.Compile(param);

            X.Compile(param.SetLHS(false));

            if ( param.LHS )
            {
                // 赋值
                param.CS.Add(new Command(Opcode.SETI)).SetCodePos(LBrackPos);
            }
            else
            {
                // 取值
                param.CS.Add(new Command(Opcode.LOADI)).SetCodePos(LBrackPos);
            }

            
        }
    }
}
