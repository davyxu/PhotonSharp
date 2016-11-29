using SharpLexer;

namespace Photon
{
    internal class BasicLit : Expr
    {
        public TokenType Type;
        public string Value;
        public TokenPos Pos;

        int _constIndex;

        public BasicLit(string v, TokenType t, TokenPos pos )
        {
            Value = v;
            Type = t;
            Pos = pos;
        }

        public override string ToString()
        {
            return string.Format("'{0}' ({1}) {2} C{3}", Value, Type, Pos, _constIndex);
        }

        internal override void Compile(CompileParameter param)
        {
            var c = Lit2Const( );
            _constIndex = param.Pkg.Constants.Add(c);

            param.CS.Add(new Command(Opcode.LOADC, param.Pkg.ID, _constIndex)).SetComment(c.ToString()).SetCodePos(Pos);
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
