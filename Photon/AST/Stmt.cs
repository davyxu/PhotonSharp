using System.Collections.Generic;
using Photon.Scanner;

namespace Photon.AST
{
    public class Chunk : Node
    {
        public BlockStmt Block;

        public Chunk( BlockStmt block )
        {
            Block = block;
        }

        public override IEnumerable<Node> Child()
        {
             return Block.Child();
        }

        public override string ToString()
        {
            return "Chunk";
        }
    }

    public class Stmt : Node
    {

    }

    public class BadStmt : Stmt
    {

    }

    public class ExprStmt : Stmt
    {
        public List<Expr> X;
        public ExprStmt(List<Expr> x)
        {
            X = x;
        }

        public override IEnumerable<Node> Child()
        {
            foreach (var e in X)
            {
                yield return e;
            }
        }

        public override string ToString()
        {
            return "ExprStmt";
        }
    }

    public class AssignStmt : Stmt
    {
        public List<Expr> LHS;
        public List<Expr> RHS;
        public AssignStmt(List<Expr> lhs, List<Expr> rhs)
        {
            LHS = lhs;
            RHS = rhs;
        }

        public override IEnumerable<Node> Child()
        {
            foreach( var e in LHS )
            {
                yield return e;
            }


            foreach (var e in RHS)
            {
                yield return e;
            }
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


    public class ReturnStmt : Stmt
    {
        public List<Expr> Results = new List<Expr>();

        public ReturnStmt(List<Expr> list)
        {
            Results = list;
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
    }
}
