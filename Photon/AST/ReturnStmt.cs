
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{

    public class ReturnStmt : Stmt
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

        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            // TODO 多返回值打到comment里
            Results[0].Compile(exe, cm, false);

            cm.Add(new Command(Opcode.Ret)).SetCodePos(RetPos);
        }
    }
}
