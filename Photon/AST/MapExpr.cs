using System.Collections.Generic;
using SharpLexer;

namespace Photon
{

    internal class MapExpr : Expr
    {
        Dictionary<BasicLit, Expr> Values = new Dictionary<BasicLit, Expr>();

        public TokenPos LBracePos;
        public TokenPos RBracePos;

        public MapExpr(Dictionary<BasicLit, Expr> values, TokenPos lbrace, TokenPos rbrace)
        {
            Values = values;
            LBracePos = lbrace;
            RBracePos = rbrace;
            
            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            foreach (var v in Values)
            {
                yield return v.Key;
                yield return v.Value;
            }
        }

        public override string ToString()
        {
            return "MapExpr";
        }

        internal override void Compile(CompileParameter param)
        {

            var cmd = param.CS.Add(new Command(Opcode.NEW )).SetCodePos(LBracePos).SetComment("Builtin.Map");
            cmd.EntryName = new ObjectName("Builtin", "Map");


            var kvParam = param.SetLHS(false);
            foreach (var kv in Values)
            {
                kv.Key.Compile(kvParam);
                kv.Value.Compile(kvParam);
            }

            param.CS.Add(new Command(Opcode.SETD, Values.Count)).SetCodePos(LBracePos);
            
            
        }
    }
}
