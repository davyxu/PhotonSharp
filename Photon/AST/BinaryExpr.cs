using Photon.Model;
using SharpLexer;
using System.Collections.Generic;

namespace Photon.AST
{
    public class BinaryExpr : Expr
    {
        public Expr X;
        public TokenType Op;
        public Expr Y;
        public TokenPos OpPos;


        public BinaryExpr(Expr x, Expr y, TokenType t,TokenPos oppos )
        {
            X = x;
            Y = y;
            Op = t;
            OpPos = oppos;

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            yield return X;

            yield return Y;
        }

        public override string ToString()
        {
            return string.Format("BinaryExpr {0}", Op.ToString());
        }

        static Opcode Token2OpCode( TokenType tk )
        {
            switch (tk)
            {
                case TokenType.Add:
                    return Opcode.Add;
                case TokenType.Mul:
                    return Opcode.Mul;                    
                case TokenType.Sub:
                    return Opcode.Sub;                    
                case TokenType.Div:
                    return Opcode.Div;                    
                case TokenType.GreatThan:
                    return Opcode.GT;                    
                case TokenType.GreatEqual:
                    return Opcode.GE;                    
                case TokenType.LessThan:
                    return Opcode.LT;                    
                case TokenType.LessEqual:
                    return Opcode.LE;                    
                case TokenType.Equal:
                    return Opcode.EQ;                    
                case TokenType.NotEqual:
                    return Opcode.NE;                    
            }

            return Opcode.Nop;
        }

        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            X.Compile(exe, cm, lhs);
            Y.Compile(exe, cm, lhs);


            cm.Add(new Command(Token2OpCode(Op)))
                .SetCodePos(OpPos);
           
        }
        
    }
}
