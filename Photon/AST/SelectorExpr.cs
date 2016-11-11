using Photon.Model;
using System.Collections.Generic;

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

        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            X.Compile(exe, cm, lhs );
            
            var c = new ValueString(Selector.Name);

            var ci = exe.Constants.Add( c );

            cm.Add(new Command(Opcode.Select, ci ) );
        }
    }
}
