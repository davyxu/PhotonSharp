
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    internal class UnaryExpr : Expr
    {
        public TokenType Op;
        public Expr X;

        public TokenPos OpPos;

        public UnaryExpr(Expr x, TokenType t, TokenPos oppos)
        {
            X = x;
            Op = t;
            OpPos = oppos;

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            yield return X;
        }

        public override string ToString()
        {
            return string.Format("UnaryExpr {0}", Op.ToString());
        }

        static Opcode Token2OpCode(TokenType tk)
        {
            switch (tk)
            {          
                case TokenType.Sub:
                    return Opcode.MINUS;
                case TokenType.Not:
                    return Opcode.NOT;
                case TokenType.Len:
                    return Opcode.LEN;                
            }

            return Opcode.NOP;
        }

        internal override void Compile(CompileParameter param)
        {
            var opcode = Token2OpCode(Op);

            if ( opcode != Opcode.NOP )
            {
                X.Compile(param);
            }
            else
            {
                throw new ParseException("Unknown unary operator", OpPos);
            }

            param.CS.Add(new Command(opcode)).SetCodePos(OpPos);
        }
    }

}
