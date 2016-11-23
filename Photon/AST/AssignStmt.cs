using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    internal class AssignStmt : Stmt
    {
        public List<Expr> LHS = new List<Expr>();
        public List<Expr> RHS = new List<Expr>();

        public TokenPos AssignPos;

        public AssignStmt(List<Expr> lhs, List<Expr> rhs, TokenPos assignPos )
        {
            LHS = lhs;
            RHS = rhs;
            AssignPos = assignPos;

            BuildRelation();
        }

        public AssignStmt(Expr lhs, Expr rhs, TokenPos assignPos)
        {
            AssignPos = assignPos;

            LHS.Add(lhs);
         
            RHS.Add(rhs);

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            foreach (var e in LHS)
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

        internal override void Compile(CompileParameter param)
        {
            foreach (var e in RHS)
            {             
                e.Compile(param.SetLHS(false));
            }

            foreach (var e in LHS)
            {                
                e.Compile(param.SetLHS(true));
            }
        }
    }

}
