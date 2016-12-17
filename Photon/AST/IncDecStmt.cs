using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    internal class IncDecStmt : Stmt
    {
        public TokenType Op;
        public Expr X;

        public TokenPos OpPos;

        public IncDecStmt(Expr x, TokenType t, TokenPos oppos)
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
            return string.Format("IncDecStmt {0}", Op.ToString());
        }

        static Opcode Token2OpCode(TokenType tk)
        {
            switch (tk)
            {
                case TokenType.Increase:
                    return Opcode.INC;
                case TokenType.Decrease:
                    return Opcode.DEC;
            }

            return Opcode.NOP;
        }

        internal override void Compile(CompileParameter param)
        {
            var opcode = Token2OpCode(Op);

            if (opcode != Opcode.NOP)
            {
                X.Compile(param);
            }
            else
            {
                throw new CompileException("Unknown unary operator", OpPos);
            }

            param.CS.Add(new Command(opcode)).SetCodePos(OpPos);
        }
    }

}
