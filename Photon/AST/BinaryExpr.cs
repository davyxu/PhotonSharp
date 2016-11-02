using Photon.OpCode;
using Photon.Scanner;
using System.Collections.Generic;

namespace Photon.AST
{
    public class BinaryExpr : Expr
    {
        public Expr X;
        public TokenType Op;
        public Expr Y;

        public BinaryExpr(Expr x, Expr y, TokenType t)
        {
            X = x;
            Y = y;
            Op = t;
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

        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            X.Compile(exe, cm, lhs);
            Y.Compile(exe, cm, lhs);

            switch (Op)
            {
                case TokenType.Add:
                    cm.Add(new Command(Opcode.Add));
                    break;
                case TokenType.Mul:
                    cm.Add(new Command(Opcode.Mul));
                    break;
                case TokenType.Sub:
                    cm.Add(new Command(Opcode.Sub));
                    break;
                case TokenType.Div:
                    cm.Add(new Command(Opcode.Div));
                    break;
                case TokenType.GreatThan:
                    cm.Add(new Command(Opcode.GT));
                    break;
                case TokenType.GreatEqual:
                    cm.Add(new Command(Opcode.GE));
                    break;
                case TokenType.LessThan:
                    cm.Add(new Command(Opcode.LT));
                    break;
                case TokenType.LessEqual:
                    cm.Add(new Command(Opcode.LE));
                    break;
                case TokenType.Equal:
                    cm.Add(new Command(Opcode.EQ));
                    break;
                case TokenType.NotEqual:
                    cm.Add(new Command(Opcode.NE));
                    break;
            }
        }
        
    }
}
