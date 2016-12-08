using SharpLexer;

namespace Photon
{
    internal partial class Parser
    {
        static Lexer NewLexer(SourceFile file)
        {
            var lex = new Lexer();

            lex.AddMatcher(new PositiveNumeralMatcher(TokenType.Number));
            lex.AddMatcher(new StringMatcher(TokenType.QuotedString));

            lex.AddMatcher(new WhitespaceMatcher(TokenType.Whitespace).Ignore());
            lex.AddMatcher(new LineEndMatcher(TokenType.LineEnd).Ignore());
            lex.AddMatcher(new CStyleCommentMatcher(TokenType.Comment).Ignore());
            lex.AddMatcher(new BlockCommentMatcher(TokenType.Comment).Ignore());

            lex.AddMatcher(new SignMatcher(TokenType.LBrace, "{"));
            lex.AddMatcher(new SignMatcher(TokenType.RBrace, "}"));
            lex.AddMatcher(new SignMatcher(TokenType.Equal, "=="));
            lex.AddMatcher(new SignMatcher(TokenType.GreatEqual, ">="));
            lex.AddMatcher(new SignMatcher(TokenType.LessEqual, "<="));
            lex.AddMatcher(new SignMatcher(TokenType.NotEqual, "!="));
            lex.AddMatcher(new SignMatcher(TokenType.Assign, "="));
            lex.AddMatcher(new SignMatcher(TokenType.GreatThan, ">"));
            lex.AddMatcher(new SignMatcher(TokenType.LessThan, "<"));
            lex.AddMatcher(new SignMatcher(TokenType.Not, "!"));
            lex.AddMatcher(new SignMatcher(TokenType.Add, "+"));
            lex.AddMatcher(new SignMatcher(TokenType.Sub, "-"));
            lex.AddMatcher(new SignMatcher(TokenType.Mul, "*"));
            lex.AddMatcher(new SignMatcher(TokenType.Div, "/"));
            lex.AddMatcher(new SignMatcher(TokenType.LBracket, "("));
            lex.AddMatcher(new SignMatcher(TokenType.RBracket, ")"));
            lex.AddMatcher(new SignMatcher(TokenType.Comma, ","));
            lex.AddMatcher(new SignMatcher(TokenType.Dot, "."));
            lex.AddMatcher(new SignMatcher(TokenType.Colon, ":"));            
            lex.AddMatcher(new SignMatcher(TokenType.SemiColon, ";"));
            lex.AddMatcher(new SignMatcher(TokenType.LSqualBracket, "["));
            lex.AddMatcher(new SignMatcher(TokenType.RSqualBracket, "]"));


            lex.AddMatcher(new KeywordMatcher(TokenType.Nil, "nil"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Len, "len"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Func, "func"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Var, "var"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Return, "return"));
            lex.AddMatcher(new KeywordMatcher(TokenType.If, "if"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Else, "else"));
            lex.AddMatcher(new KeywordMatcher(TokenType.For, "for"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Foreach, "foreach"));
            lex.AddMatcher(new KeywordMatcher(TokenType.While, "while"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Break, "break"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Continue, "continue"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Import, "import"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Class, "class"));
            lex.AddMatcher(new KeywordMatcher(TokenType.New, "new"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Is, "is"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Int32, "int32"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Int64, "int64"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Float32, "float32"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Float64, "float64"));

            lex.AddMatcher(new IdentifierMatcher(TokenType.Identifier));
            lex.AddMatcher(new UnknownMatcher(TokenType.Unknown));

            lex.Start(file.Source, file.Name);

            return lex;
        }
    }
}
