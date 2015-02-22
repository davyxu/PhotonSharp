using System.Diagnostics;
using Photon.Scanner;
using Photon.Parser;

namespace UnitTest
{
    partial class Program
    {
        static void LexerTest()
        {
            var lex = new Lexer();
            lex.AddMatcher(new KeywordMatcher(TokenType.Nil, "nil"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Assign, "="));
            lex.AddMatcher(new KeywordMatcher(TokenType.Equal, "=="));
            lex.AddMatcher(new KeywordMatcher(TokenType.NotEqual, "!="));
            lex.AddMatcher(new KeywordMatcher(TokenType.GreatThan, ">"));
            lex.AddMatcher(new KeywordMatcher(TokenType.LessThan, "<"));
            lex.AddMatcher(new KeywordMatcher(TokenType.GreatEqual, ">="));
            lex.AddMatcher(new KeywordMatcher(TokenType.LessEqual, "<="));
            lex.AddMatcher(new KeywordMatcher(TokenType.Add, "+"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Sub, "-"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Mul, "*"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Div, "/"));
            lex.AddMatcher(new KeywordMatcher(TokenType.LBracket, "("));
            lex.AddMatcher(new KeywordMatcher(TokenType.RBracket, ")"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Comma, ","));
            lex.AddMatcher(new KeywordMatcher(TokenType.Dot, "."));
            lex.AddMatcher(new KeywordMatcher(TokenType.Func, "func"));
            lex.AddMatcher(new KeywordMatcher(TokenType.Var, "var"));
            lex.AddMatcher(new KeywordMatcher(TokenType.LSqualBracket, "["));
            lex.AddMatcher(new KeywordMatcher(TokenType.RSqualBracket, "]"));


            lex.Start("func hello(1, 2 )");

            while (true)
            {
                var tk = lex.Read();

                if (tk.Type == TokenType.EOF)
                    break;


                Debug.WriteLine("{0} [{1}]", tk.Type, tk.Value);
            }

        }
    }
}
