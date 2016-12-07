
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{

    internal class ReturnStmt : Stmt
    {
        public List<Expr> Results = new List<Expr>();
        public TokenPos RetPos;

        public ReturnStmt(List<Expr> list, TokenPos retpos)
        {
            Results = list;
            RetPos = retpos;

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            foreach (var s in Results)
            {
                yield return s;
            }
        }

        public override string ToString()
        {
            return "ReturnStmt";
        }

        internal override void Compile(CompileParameter param)
        {            
            for (int i = Results.Count - 1; i >= 0;i-- )
            {
                Results[i].Compile(param.SetLHS(false));
            }
            
            
            param.CS.Add(new Command(Opcode.RET)).SetCodePos(RetPos);
        }
    }
}
