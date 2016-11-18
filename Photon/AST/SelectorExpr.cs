
using System.Collections.Generic;

namespace Photon
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

        internal override void Compile(CompileParameter param)
        {
            X.Compile(param);
            
            var c = new ValueString(Selector.Name);

            var ci = param.Pkg.Constants.Add(c);

            param.CS.Add(new Command(Opcode.SEL, ci));
        }
    }
}
