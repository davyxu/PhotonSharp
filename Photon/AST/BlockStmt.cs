using Photon.Model;
using System.Collections.Generic;

namespace Photon.AST
{
    public class BlockStmt : Stmt
    {
        public List<Stmt> Stmts = new List<Stmt>();

        public BlockStmt()
        {

        }

        public BlockStmt(List<Stmt> list)
        {
            Stmts = list;

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            foreach (var s in Stmts)
            {
                yield return s;
            }
        }

        public override string ToString()
        {
            return "BlockStmt";
        }

        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {

            foreach (var b in Stmts)
            {
                b.Compile(exe, cm, false);
            }
        }
    }
}
