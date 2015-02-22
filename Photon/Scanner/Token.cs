
namespace Photon.Scanner
{
    public enum TokenType
    {
        None,
        Unknown,
        Whitespace,
        EOF,

        Identifier,
        Number,
        QuotedString,
        
        Assign,         // =
        Equal,          // ==
        NotEqual,       // !=
        LessThan,       // <
        GreatThan,      // >
        LessEqual,      // <=
        GreatEqual,     // >=
        Add,            // +
        Sub,            // -
        Mul,            // *
        Div,            // /
        Comma,          // ,
        Dot,            // .
        LBracket,       // (
        RBracket,       // )
        LSqualBracket,  // [
        RSqualBracket,  // ]
        LBrace,         // {
        RBrace,         // }
        Func,           // func
        Nil,            // nil
        Var,            // var
        Return,         // return

    }
    public class Token
    {
        string _value;
        TokenType _type;

        public Token( TokenType type, string value )
        {
            _type = type;
            _value = value;
        }

        public TokenType Type
        { 
            get { return _type;  } 
        }

        public string Value
        {
            get { return _value;  }
        }

        public float ToNumber()
        {
            return float.Parse(_value);
        }

        public override string ToString()
        {            
            return _type.ToString() + " " + Value;
        }
    }

    public class TokenPos
    {
        public int Pos;

        public TokenPos(int line, int col)
        {

        }
    }
}
