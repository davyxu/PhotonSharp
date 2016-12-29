using System.Collections.Generic;
using SharpLexer;

namespace Photon
{

    internal class ArrayExpr : Expr
    {
        List<Expr> Values = new List<Expr>();

        public TokenPos LBracketPos;
        public TokenPos RBracketPos;

        public ArrayExpr(List<Expr> values, TokenPos lbracket, TokenPos rbracket)
        {
            Values = values;
            LBracketPos = lbracket;
            RBracketPos = rbracket;
            
            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            foreach (var v in Values)
            {
                yield return v;
            }
        }

        public override string ToString()
        {
            return "ArrayExpr";
        }

        internal override void Compile(CompileParameter param)
        {
            var cmd = param.CS.Add(new Command(Opcode.NEW )).SetCodePos(LBracketPos).SetComment("Builtin.Array");
            cmd.EntryName = new ObjectName("Builtin", "Array");


            foreach (var v in Values)
            {
                v.Compile(param);
            }

            if (Values.Count > 0)
            {
                param.CS.Add(new Command(Opcode.SETA, Values.Count)).SetCodePos(LBracketPos);
            }
        }
    }
}
