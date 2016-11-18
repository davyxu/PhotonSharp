using SharpLexer;
using System.Collections.Generic;

namespace Photon
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
                    return Opcode.ADD;
                case TokenType.Mul:
                    return Opcode.MUL;                    
                case TokenType.Sub:
                    return Opcode.SUB;                    
                case TokenType.Div:
                    return Opcode.DIV;                    
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

            return Opcode.NOP;
        }

        internal override void Compile(CompileParameter param)
        {
            X.Compile(param);
            Y.Compile(param);


            param.CS.Add(new Command(Token2OpCode(Op)))
                .SetCodePos(OpPos);
           
        }
        
    }
}
