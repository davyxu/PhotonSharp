using Photon.Model;
using Photon.AST;
using System;
using System.Diagnostics;
using SharpLexer;

namespace Photon.Parser
{
    public class ParseException : Exception
    {
        public TokenPos Pos;

        public ParseException(string msg, TokenPos pos)
            : base(msg)
        {
            Pos = pos;
        }
    }

    public partial class ScriptParser
    {
        Lexer _lexer = new Lexer();

        Token _token;

        public ScriptParser()
        {
            _lexer.AddMatcher(new PositiveNumeralMatcher(TokenType.Number));
            _lexer.AddMatcher(new StringMatcher(TokenType.QuotedString));

            _lexer.AddMatcher(new WhitespaceMatcher(TokenType.Whitespace).Ignore());
            _lexer.AddMatcher(new LineEndMatcher(TokenType.LineEnd).Ignore());
            _lexer.AddMatcher(new CStyleCommentMatcher(TokenType.Comment).Ignore());

            _lexer.AddMatcher(new SignMatcher(TokenType.Nil, "nil"));
            _lexer.AddMatcher(new SignMatcher(TokenType.Equal, "=="));
            _lexer.AddMatcher(new SignMatcher(TokenType.GreatEqual, ">="));
            _lexer.AddMatcher(new SignMatcher(TokenType.LessEqual, "<="));
            _lexer.AddMatcher(new SignMatcher(TokenType.NotEqual, "!="));
            _lexer.AddMatcher(new SignMatcher(TokenType.Assign, "="));
            _lexer.AddMatcher(new SignMatcher(TokenType.GreatThan, ">"));
            _lexer.AddMatcher(new SignMatcher(TokenType.LessThan, "<"));
            _lexer.AddMatcher(new SignMatcher(TokenType.Add, "+"));
            _lexer.AddMatcher(new SignMatcher(TokenType.Sub, "-"));
            _lexer.AddMatcher(new SignMatcher(TokenType.Mul, "*"));
            _lexer.AddMatcher(new SignMatcher(TokenType.Div, "/"));
            _lexer.AddMatcher(new SignMatcher(TokenType.LBracket, "("));
            _lexer.AddMatcher(new SignMatcher(TokenType.RBracket, ")"));
            _lexer.AddMatcher(new SignMatcher(TokenType.Comma, ","));
            _lexer.AddMatcher(new SignMatcher(TokenType.Dot, "."));
            _lexer.AddMatcher(new SignMatcher(TokenType.SemiColon, ";"));
            _lexer.AddMatcher(new SignMatcher(TokenType.Func, "func"));
            _lexer.AddMatcher(new SignMatcher(TokenType.Var, "var"));
            _lexer.AddMatcher(new SignMatcher(TokenType.LSqualBracket, "["));
            _lexer.AddMatcher(new SignMatcher(TokenType.RSqualBracket, "]"));
            _lexer.AddMatcher(new SignMatcher(TokenType.LBrace, "{"));
            _lexer.AddMatcher(new SignMatcher(TokenType.RBrace, "}"));
            _lexer.AddMatcher(new SignMatcher(TokenType.Return, "return"));
            _lexer.AddMatcher(new SignMatcher(TokenType.If, "if"));
            _lexer.AddMatcher(new SignMatcher(TokenType.Else, "else"));
            _lexer.AddMatcher(new SignMatcher(TokenType.For, "for"));
            _lexer.AddMatcher(new SignMatcher(TokenType.Foreach, "foreach"));
            _lexer.AddMatcher(new SignMatcher(TokenType.While, "while"));
            _lexer.AddMatcher(new SignMatcher(TokenType.Break, "break"));
            _lexer.AddMatcher(new SignMatcher(TokenType.Continue, "continue"));

            _lexer.AddMatcher(new IdentifierMatcher(TokenType.Identifier));
            _lexer.AddMatcher(new UnknownMatcher(TokenType.Unknown));
        }

        public Chunk ParseSource(string source)
        {
            InitScope();

            _lexer.Start(source);

            Next();

            return ParseChunk();
        }

        public override string ToString()
        {
            return _token.ToString();
        }

        void Next()
        {
            _token = _lexer.Read();

            if (CurrTokenType == TokenType.Unknown)
            {
                Error("unknown token");
            }
        }

        TokenType CurrTokenType
        {
            get { return (TokenType)_token.MatcherID; }
        }

        TokenPos CurrTokenPos
        {
            get { return _token.Pos; }
        }

        void Expect(TokenType t)
        {
            if (CurrTokenType != t)
            {
                Error(string.Format("expect token: {0}", t.ToString()));
            }

            Next();
        }

        void Error(string str)
        {
            throw new ParseException(str, _token.Pos);
        }

        void Error(string str, TokenPos pos)
        {
            throw new ParseException(str, pos);
        }


        public static void PrintAST(Node n, string indent = "")
        {
            Debug.WriteLine(indent + n.ToString());

            foreach (var c in n.Child())
            {
                PrintAST(c, indent + "\t");
            }
        }
    }
}
