
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
        LBracket,       // (
        RBracket,       // )
        LSqualBracket,  // [
        RSqualBracket,  // ]
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
    }
   
}
