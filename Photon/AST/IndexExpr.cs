using Photon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.AST
{
    // a.b   x=a  index=b

    public class IndexExpr : Expr
    {
        public Expr X;
        public Expr Index;

        public IndexExpr(Expr x, Expr index)
        {
            X = x;
            Index = index;

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

        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            X.Compile(exe, cm, lhs);

            Index.Compile(exe, cm, lhs);            

            cm.Add(new Command(Opcode.Index ));
        }
    }
}
