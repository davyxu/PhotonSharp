using SharpLexer;

namespace Photon
{
    public partial class Parser
    {
        static Lexer NewLexer(SourceFile file)
        {
            var lex = new Lexer();

            lex.AddMatcher(new PositiveNumeralMatcher(TokenType.Number));
            lex.AddMatcher(new StringMatcher(TokenType.QuotedString));

            lex.AddMatcher(new WhitespaceMatcher(TokenType.Whitespace).Ignore());
            lex.AddMatcher(new LineEndMatcher(TokenType.LineEnd).Ignore());
            lex.AddMatcher(new CStyleCommentMatcher(TokenType.Comment).Ignore());

            lex.AddMatcher(new SignMatcher(TokenType.Nil, "nil"));
            lex.AddMatcher(new SignMatcher(TokenType.Equal, "=="));
            lex.AddMatcher(new SignMatcher(TokenType.GreatEqual, ">="));
            lex.AddMatcher(new SignMatcher(TokenType.LessEqual, "<="));
            lex.AddMatcher(new SignMatcher(TokenType.NotEqual, "!="));
            lex.AddMatcher(new SignMatcher(TokenType.Assign, "="));
            lex.AddMatcher(new SignMatcher(TokenType.GreatThan, ">"));
            lex.AddMatcher(new SignMatcher(TokenType.LessThan, "<"));
            lex.AddMatcher(new SignMatcher(TokenType.Add, "+"));
            lex.AddMatcher(new SignMatcher(TokenType.Sub, "-"));
            lex.AddMatcher(new SignMatcher(TokenType.Mul, "*"));
            lex.AddMatcher(new SignMatcher(TokenType.Div, "/"));
            lex.AddMatcher(new SignMatcher(TokenType.LBracket, "("));
            lex.AddMatcher(new SignMatcher(TokenType.RBracket, ")"));
            lex.AddMatcher(new SignMatcher(TokenType.Comma, ","));
            lex.AddMatcher(new SignMatcher(TokenType.Dot, "."));
            lex.AddMatcher(new SignMatcher(TokenType.SemiColon, ";"));
            lex.AddMatcher(new SignMatcher(TokenType.Func, "func"));
            lex.AddMatcher(new SignMatcher(TokenType.Var, "var"));
            lex.AddMatcher(new SignMatcher(TokenType.LSqualBracket, "["));
            lex.AddMatcher(new SignMatcher(TokenType.RSqualBracket, "]"));
            lex.AddMatcher(new SignMatcher(TokenType.LBrace, "{"));
            lex.AddMatcher(new SignMatcher(TokenType.RBrace, "}"));
            lex.AddMatcher(new SignMatcher(TokenType.Return, "return"));
            lex.AddMatcher(new SignMatcher(TokenType.If, "if"));
            lex.AddMatcher(new SignMatcher(TokenType.Else, "else"));
            lex.AddMatcher(new SignMatcher(TokenType.For, "for"));
            lex.AddMatcher(new SignMatcher(TokenType.Foreach, "foreach"));
            lex.AddMatcher(new SignMatcher(TokenType.While, "while"));
            lex.AddMatcher(new SignMatcher(TokenType.Break, "break"));
            lex.AddMatcher(new SignMatcher(TokenType.Continue, "continue"));
            lex.AddMatcher(new SignMatcher(TokenType.Import, "import"));

            lex.AddMatcher(new IdentifierMatcher(TokenType.Identifier));
            lex.AddMatcher(new UnknownMatcher(TokenType.Unknown));

            lex.Start(file.Source, file.SourceName);

            return lex;
        }
    }
}
