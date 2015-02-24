using Photon.Scanner;
using Photon.AST;
using System;
using System.Diagnostics;

namespace Photon.Parser
{
    public partial class Parser
    {
        Lexer _lexer = new Lexer();

        Token _token;

        public void Init( string source )
        {
            _lexer.AddMatcher(new NumeralMatcher());
           
            _lexer.AddMatcher(new WhitespaceMatcher().Ignore());
            _lexer.AddMatcher(new CommentMatcher().Ignore());

            _lexer.AddMatcher(new KeywordMatcher(TokenType.Nil, "nil"));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.Equal, "=="));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.GreatEqual, ">="));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.LessEqual, "<="));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.NotEqual, "!="));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.Assign, "="));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.GreatThan, ">"));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.LessThan, "<"));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.Add, "+"));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.Sub, "-"));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.Mul, "*"));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.Div, "/"));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.LBracket, "("));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.RBracket, ")"));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.Comma, ","));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.Dot, "."));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.Func, "func"));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.Var, "var"));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.LSqualBracket, "["));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.RSqualBracket, "]"));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.LBrace, "{"));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.RBrace, "}"));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.Return, "return"));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.If, "if"));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.Else, "else"));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.For, "for"));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.Break, "break"));
            _lexer.AddMatcher(new KeywordMatcher(TokenType.Continue, "continue"));

            _lexer.AddMatcher(new IdentifierMatcher());
            _lexer.AddMatcher(new UnknownMatcher());

            _lexer.Start(source);

            Next();

            InitScope();
        }

        public override string ToString()
        {
            return _token.ToString();
        }

        void Next()
        {
            _token = _lexer.Read();
        }

      

        void Expect( TokenType t )
        {
            if ( _token.Type != t )
            {
                throw new Exception(string.Format("expect token: {0}", t.ToString()));
            }

            Next();
        }

        void Error( string str )
        {
            throw new Exception(str);
        }

        public static void DebugPrint( Node n)
        {
            PrintAST(n, "");
        }

        static void PrintAST(Node n, string indent)
        {
            Debug.WriteLine(indent + n.ToString());

            foreach (var c in n.Child())
            {
                PrintAST(c, indent + "\t");
            }
        }
    }
}
