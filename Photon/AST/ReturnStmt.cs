
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
            // TODO 多返回值打到comment里
            Results[0].Compile(param.SetLHS(false));

            param.CS.Add(new Command(Opcode.RET)).SetCodePos(RetPos);
        }
    }
}
