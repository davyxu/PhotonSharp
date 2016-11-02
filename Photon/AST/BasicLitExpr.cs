
using Photon.OpCode;
using Photon.Scanner;
namespace Photon.AST
{
    public class BasicLitExpr : Expr
    {
        public TokenType Type;
        public string Value;
        public BasicLitExpr(string v, TokenType t)
        {
            Value = v;
            Type = t;
        }

        public override string ToString()
        {
            return Value;
        }

        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            var c = Lit2Const( );
            var ci = exe.Constants.Add(c);

            cm.Add(new Command(Opcode.LoadC, ci)).Comment = c.GetDesc();
        }

        DataValue Lit2Const()
        {
            DataValue c = null;

            switch (Type)
            {
                case TokenType.Number:
                    {
                        float v;
                        if (!float.TryParse(Value, out v))
                            return null;

                        c = new NumberValue(v);
                    }
                    break;
            }


            return c;
        }
    }
}
