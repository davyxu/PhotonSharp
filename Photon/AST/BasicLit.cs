using SharpLexer;

namespace Photon
{
    public class BasicLit : Expr
    {
        public TokenType Type;
        public string Value;
        public TokenPos Pos;

        public BasicLit(string v, TokenType t, TokenPos pos )
        {
            Value = v;
            Type = t;
            Pos = pos;
        }

        public override string ToString()
        {
            return Value;
        }

        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            var c = Lit2Const( );
            var ci = exe.Constants.Add(c);

            cm.Add(new Command(Opcode.LoadC, ci)).SetComment( c.ToString() ).SetCodePos( Pos );
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
