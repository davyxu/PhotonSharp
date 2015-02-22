using System.Collections.Generic;
using Photon.Scanner;

namespace Photon.AST
{

    public class Stmt : Node
    {

    }

    public class BadStmt : Stmt
    {

    }

    public class AssignStmt : Stmt
    {
        public Expr LHS;
        public Expr RHS;
        public AssignStmt(Expr lhs, Expr rhs)
        {
            LHS = lhs;
            RHS = rhs;
        }

        public override IEnumerable<Node> Child()
        {
            yield return LHS;

            yield return RHS;
        }

        public override string ToString()
        {
            return "AssignStmt";
        }
    }

    public class BlockStmt : Stmt
    {
        public List<Stmt> Stmts = new List<Stmt>();

        public BlockStmt(List<Stmt> list)
        {
            Stmts = list;
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
    }
}
