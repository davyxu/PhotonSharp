using SharpLexer;
using System;

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
            _constIndex = param.Constants.Add(c);

            param.CS.Add(new Command(Opcode.LOADK, _constIndex)).SetComment(c.ToString()).SetCodePos(Pos);
        }

        static bool IsFloat( string s )
        {
            return s.IndexOf('.') != -1;
        }

        Value Lit2Const()
        {
            Value c = null;

            switch (Type)
            {
                case TokenType.Number:
                    {
                        if (IsFloat(Value))
                        {
                            float v;
                            if (!float.TryParse(Value, out v))
                                return null;

                            c = new ValueFloat32(v);
                        }
                        else
                        {
                            Int32 v;
                            if (!Int32.TryParse(Value, out v))
                                return null;

                            c = new ValueInteger32(v);
                        }
                        
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
