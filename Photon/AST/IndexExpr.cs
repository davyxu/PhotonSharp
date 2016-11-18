
using SharpLexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon
{
    // a.b   x=a  index=b

    public class IndexExpr : Expr
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
            X.Compile(param);

            Index.Compile(param);

            param.CS.Add(new Command(Opcode.IDX));
        }
    }
}
