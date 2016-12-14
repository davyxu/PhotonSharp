using System.Collections.Generic;
using SharpLexer;

namespace Photon
{

    internal class ArrayExpr : Expr
    {
        List<BasicLit> Values = new List<BasicLit>();

        public TokenPos LBracketPos;
        public TokenPos RBracketPos;

        public ArrayExpr(List<BasicLit> values, TokenPos lbracket, TokenPos rbracket)
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


            var c = param.Exe.GetClassTypeByName(new ObjectName("Builtin", "Array"));
            if (c == null)
            {
                throw new CompileException("'Builtin.Array' not exists", LBracketPos);
            }

            param.CS.Add(new Command(Opcode.NEW, c.ID)).SetCodePos(LBracketPos).SetComment("Builtin.Array");


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
