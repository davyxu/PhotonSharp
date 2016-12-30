using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    internal class AssignStmt : Stmt
    {
        public List<Expr> LHS = new List<Expr>();
        public List<Expr> RHS = new List<Expr>();

        public TokenPos AssignPos;
        public TokenType Op;

        public AssignStmt(List<Expr> lhs, List<Expr> rhs, TokenPos assignPos, TokenType op )
        {
            LHS = lhs;
            RHS = rhs;
            Op = op;
            AssignPos = assignPos;

            BuildRelation();
        }

        public AssignStmt(Expr lhs, Expr rhs, TokenPos assignPos, TokenType op)
        {
            AssignPos = assignPos;
            Op = op;
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
        static Opcode Token2OpCode(TokenType tk)
        {
            switch (tk)
            {
                case TokenType.AddAssign:
                    return Opcode.ADD;
                case TokenType.MulAssign:
                    return Opcode.MUL;
                case TokenType.SubAssign:
                    return Opcode.SUB;
                case TokenType.QuoAssign:
                    return Opcode.DIV;
            }

            return Opcode.NOP;
        }


        internal override void Compile(CompileParameter param)
        {

            switch (Op)
            {
                case TokenType.Assign:
                    {
                        foreach (var e in RHS)
                        {
                            e.Compile(param.SetLHS(false));
                        }
                    }
                    break;
                case TokenType.AddAssign:
                case TokenType.SubAssign:
                case TokenType.MulAssign:
                case TokenType.QuoAssign:
                    {
                        // 这种操作只允许一个一个来
                        if ( LHS.Count != 1 || RHS.Count != 1 )
                        {
                            throw new CompileException("assignment require 1 operand", AssignPos);
                        }

                        LHS[0].Compile(param.SetLHS(false));

                        RHS[0].Compile(param.SetLHS(false));

                        param.CS.Add(new Command(Token2OpCode(Op)))
                        .SetCodePos(AssignPos);


                    }
                    break;
            }

            foreach (var e in LHS)
            {                
                e.Compile(param.SetLHS(true));
            }
        }
    }

}
