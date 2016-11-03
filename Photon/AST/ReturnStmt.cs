using Photon.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.AST
{

    public class ReturnStmt : Stmt
    {
        public List<Expr> Results = new List<Expr>();

        public ReturnStmt(List<Expr> list)
        {
            Results = list;

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
            Results[0].Compile(exe, cm, false);

            cm.Add(new Command(Opcode.Ret));
        }
    }
}
