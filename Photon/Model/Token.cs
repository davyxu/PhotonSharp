
namespace Photon
{
    enum TokenType
    {
        EOF,
        Unknown,
        Whitespace,
        LineEnd,        

        Comment,
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
        Not,            // !
        Add,            // +
        Sub,            // -
        Mul,            // *
        Div,            // /
        Comma,          // ,
        Dot,            // .        
        Colon,          // :
        SemiColon,      // ;
        LParen,         // (
        RParen,         // )
        LBracket,       // [
        RBracket,       // ]
        LBrace,         // {
        RBrace,         // }
        Len,            // len
        Func,           // func
        Nil,            // nil
        Var,            // var
        Return,         // return
        If,             // if
        Else,           // else
        For,            // for
        Foreach,        // foreach
        While,          // while
        Break,          // break
        Continue,       // continue
        Import,         // import
        Class,          // class
        New,            // new
        Is,             // is
        Base,           // base
        Int32,          // int32
        Int64,          // int64
        Float32,        // float32
        Float64,        // float64
    }
   
}
