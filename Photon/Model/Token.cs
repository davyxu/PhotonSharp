
namespace Photon
{
    public enum TokenType
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
        Add,            // +
        Sub,            // -
        Mul,            // *
        Div,            // /
        Comma,          // ,
        Dot,            // .
        SemiColon,      // ;
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
        If,             // if
        Else,           // else
        For,            // for
        Foreach,        // foreach
        While,          // while
        Break,          // break
        Continue,       // continue
    }
   
}
