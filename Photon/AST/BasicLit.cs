
using Photon.Model;

namespace Photon.AST
{
    public class BasicLit : Expr
    {
        public TokenType Type;
        public string Value;
        public BasicLit(string v, TokenType t)
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

            cm.Add(new Command(Opcode.LoadC, ci)).Comment = c.ToString();
        }

        Value Lit2Const()
        {
            Value c = null;

            switch (Type)
            {
                case TokenType.Number:
                    {
                        float v;
                        if (!float.TryParse(Value, out v))
                            return null;

                        c = new ValueNumber(v);
                    }
                    break;
                case TokenType.QuotedString:
                    {
                        c = new ValueString(Value);
                    }
                    break;
                default:
                    break;
            }


            return c;
        }
    }
}
