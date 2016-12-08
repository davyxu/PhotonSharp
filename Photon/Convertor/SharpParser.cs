using SharpLexer;
using System.Collections.Generic;
using System.Text;

namespace Photon
{
    enum SharpTokenType
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

        Class,          // class
        Public,         // public
        Override,       // override
       
        Null,           // null
        Var,            // var
        Return,         // return
        If,             // if
        Else,           // else
        For,            // for
        Foreach,        // foreach
        While,          // while
        Break,          // break
        Continue,       // continue        
        Using,       // Using        
        
        New,            // new        
        Int,            // int
        Float,          // float
        
    }


    public class SharpParser
    {
        Lexer _lexer;

        Token _token;
        StringBuilder _sb = new StringBuilder();

        List<Matcher> _ignoreMatcher = new List<Matcher>();

        public SharpParser()
        {
            var lex = new Lexer();
            _lexer = lex;

            lex.AddMatcher(new PositiveNumeralMatcher(SharpTokenType.Number));
            lex.AddMatcher(new StringMatcher(SharpTokenType.QuotedString));

            _ignoreMatcher.Add(lex.AddMatcher(new WhitespaceMatcher(SharpTokenType.Whitespace)));
            _ignoreMatcher.Add(lex.AddMatcher(new LineEndMatcher(SharpTokenType.LineEnd)));
            _ignoreMatcher.Add(lex.AddMatcher(new CStyleCommentMatcher(SharpTokenType.Comment)));
            _ignoreMatcher.Add(lex.AddMatcher(new BlockCommentMatcher(SharpTokenType.Comment)));

            lex.AddMatcher(new SignMatcher(SharpTokenType.LBrace, "{"));
            lex.AddMatcher(new SignMatcher(SharpTokenType.RBrace, "}"));
            lex.AddMatcher(new SignMatcher(SharpTokenType.Equal, "=="));
            lex.AddMatcher(new SignMatcher(SharpTokenType.GreatEqual, ">="));
            lex.AddMatcher(new SignMatcher(SharpTokenType.LessEqual, "<="));
            lex.AddMatcher(new SignMatcher(SharpTokenType.NotEqual, "!="));
            lex.AddMatcher(new SignMatcher(SharpTokenType.Assign, "="));
            lex.AddMatcher(new SignMatcher(SharpTokenType.GreatThan, ">"));
            lex.AddMatcher(new SignMatcher(SharpTokenType.LessThan, "<"));
            lex.AddMatcher(new SignMatcher(SharpTokenType.Not, "!"));
            lex.AddMatcher(new SignMatcher(SharpTokenType.Add, "+"));
            lex.AddMatcher(new SignMatcher(SharpTokenType.Sub, "-"));
            lex.AddMatcher(new SignMatcher(SharpTokenType.Mul, "*"));
            lex.AddMatcher(new SignMatcher(SharpTokenType.Div, "/"));
            lex.AddMatcher(new SignMatcher(SharpTokenType.LBracket, "("));
            lex.AddMatcher(new SignMatcher(SharpTokenType.RBracket, ")"));
            lex.AddMatcher(new SignMatcher(SharpTokenType.Comma, ","));
            lex.AddMatcher(new SignMatcher(SharpTokenType.Dot, "."));
            lex.AddMatcher(new SignMatcher(SharpTokenType.Colon, ":"));
            lex.AddMatcher(new SignMatcher(SharpTokenType.SemiColon, ";"));
            lex.AddMatcher(new SignMatcher(SharpTokenType.LSqualBracket, "["));
            lex.AddMatcher(new SignMatcher(SharpTokenType.RSqualBracket, "]"));

            lex.AddMatcher(new KeywordMatcher(SharpTokenType.Null, "null"));            
            lex.AddMatcher(new KeywordMatcher(SharpTokenType.Var, "var"));
            lex.AddMatcher(new KeywordMatcher(SharpTokenType.Public, "public"));
            lex.AddMatcher(new KeywordMatcher(SharpTokenType.Override, "override"));
            lex.AddMatcher(new KeywordMatcher(SharpTokenType.Return, "return"));
            lex.AddMatcher(new KeywordMatcher(SharpTokenType.If, "if"));
            lex.AddMatcher(new KeywordMatcher(SharpTokenType.Else, "else"));
            lex.AddMatcher(new KeywordMatcher(SharpTokenType.For, "for"));
            lex.AddMatcher(new KeywordMatcher(SharpTokenType.Foreach, "foreach"));
            lex.AddMatcher(new KeywordMatcher(SharpTokenType.While, "while"));
            lex.AddMatcher(new KeywordMatcher(SharpTokenType.Break, "break"));
            lex.AddMatcher(new KeywordMatcher(SharpTokenType.Continue, "continue"));            
            lex.AddMatcher(new KeywordMatcher(SharpTokenType.Class, "class"));
            lex.AddMatcher(new KeywordMatcher(SharpTokenType.New, "new"));            
            lex.AddMatcher(new KeywordMatcher(SharpTokenType.Int, "int"));
            lex.AddMatcher(new KeywordMatcher(SharpTokenType.Float, "float"));
            lex.AddMatcher(new KeywordMatcher(SharpTokenType.Using, "using"));            

            lex.AddMatcher(new IdentifierMatcher(SharpTokenType.Identifier));
            lex.AddMatcher(new UnknownMatcher(SharpTokenType.Unknown));
        }


        public override string ToString()
        {
            return _token.ToString();
        }


        bool _ignoreWhiteSpace = true;
        void IgnoreWhiteSpace(bool ignore)
        {
            _ignoreWhiteSpace = ignore;
        }



        void Next()
        {
            while (true)
            {
                _token = _lexer.Read();

                if (_ignoreWhiteSpace)
                {
                    if (CurrTokenType == SharpTokenType.Whitespace ||
                    CurrTokenType == SharpTokenType.LineEnd ||
                    CurrTokenType == SharpTokenType.Comment)
                    {
                        continue;
                    }

                    
                }

                break;
            }
            


            if (CurrTokenType == SharpTokenType.Unknown)
            {
                throw new CompileException("unknown token", CurrTokenPos);
            }
        }

        SharpTokenType CurrTokenType
        {
            get { return (SharpTokenType)_token.MatcherID; }
        }

        internal TokenPos CurrTokenPos
        {
            get { return _token.Pos; }
        }

        string CurrTokenValue
        {
            get { return _token.Value; }
        }

        Token Expect(SharpTokenType t)
        {
            if (CurrTokenType != t)
            {
                throw new CompileException(string.Format("expect token: {0}", t.ToString()), CurrTokenPos);
            }

            var tk = _token;

            Next();

            return tk;
        }

        const string header = @"
class SkillFunc
{
	
}

func SkillFunc.Immhurt( self,skill,b1,b2){
	
}


func SkillFunc.Roundhurt( self,skill,b1,b2){
	
}

func OffsetMagic( a, b ){
	
}

func MagicDamage( a, b ){
	
}

func FindMagDefbuff( a ){
}

";

        public void Start(string src)
        {
            _lexer.Start(src, "file");

            WriteString(header);

            Next();

            ParseUsing();

            while( CurrTokenType != SharpTokenType.EOF )
            {
                ParseClass();

                WriteString("\n");
            }
            

            System.IO.File.WriteAllText("out.pho", _sb.ToString());
        }

        void ParseUsing()
        {
            while (CurrTokenType == SharpTokenType.Using)
            {
                Next();

                while (CurrTokenType != SharpTokenType.SemiColon)
                {
                    Expect(SharpTokenType.Identifier);

                    if (CurrTokenType == SharpTokenType.Dot)
                    {
                        Next();
                    }

                }

                Next();
            }


                        
        }

        void WriteToken()
        {
            _sb.Append(_token.Value);
        }

        void WriteString(string s )
        {
            _sb.Append(s);
        }

        void ParseClass()
        {
            WriteString("class ");
            Expect(SharpTokenType.Class);

            

            // 类名
            WriteToken(); WriteString(" ");
            var className = CurrTokenValue;
            Expect(SharpTokenType.Identifier);


            WriteToken(); WriteString(" ");
            Expect(SharpTokenType.Colon);
            

            // 父类
            WriteToken(); WriteString(" ");
            Expect(SharpTokenType.Identifier);
            WriteString("\n");

            Expect(SharpTokenType.LBrace);
            WriteString("{}\n");

            while( CurrTokenType == SharpTokenType.Public)
            {
                ParseFunction( className );
            }

            Expect(SharpTokenType.RBrace);

            
        }

        void ParseFunction(string className)
        {            
            Expect(SharpTokenType.Public);
         
            Expect(SharpTokenType.Override);

            WriteString("func ");
            WriteString(className);
            WriteString(".");

            Expect(SharpTokenType.Int);

            // 函数名
            WriteToken(); 
            Expect(SharpTokenType.Identifier);

            WriteToken(); WriteString(" self,");
            Expect(SharpTokenType.LBracket);            

            while (CurrTokenType != SharpTokenType.RBracket)
            {
                Expect(SharpTokenType.Identifier);

                WriteToken();
                Expect(SharpTokenType.Identifier);

                if (CurrTokenType == SharpTokenType.Comma)
                {
                    WriteToken();

                    Next();
                }
            }

            

            WriteToken(); WriteString("\n");
            Expect(SharpTokenType.RBracket);

            // 函数体
            IgnoreWhiteSpace(false);

            WriteToken(); WriteString("\n");
            Expect(SharpTokenType.LBrace);

            int level = 1;
            bool inElse = false;

            while (true) 
            {
  
               
                switch( CurrTokenType )
                {
                    case SharpTokenType.LBrace:
                        {
                            WriteToken();
                            level++;
                        }
                        break;
                    case SharpTokenType.RBrace:
                        {
                            
                            level--;

                            if (level > 0)
                            {
                                WriteToken();
                            }
                        }
                        break;
                    case SharpTokenType.Int:
                        WriteString("var");
                        break;
                    case SharpTokenType.SemiColon:
                        break;
                    case SharpTokenType.Number:
                        {
                            WriteToken();                            
                            Next();

                            if ( CurrTokenValue != "f" )
                            {
                                WriteToken();
                            }
                        }
                        break;
                    case SharpTokenType.Identifier:
                        {
                            if (CurrTokenValue == "SkillUtil")
                            {

                                Next();
                                Next();
                                continue;
                            }

                            WriteToken();
                            Next();

                            if ( CurrTokenType != SharpTokenType.Whitespace )
                            {
                                WriteToken();
                            }
           
                        }
                        break;
                    case SharpTokenType.Else:
                        {
                            inElse = true;
                            WriteToken();
                            WriteString("{\n");
                        }
                        break;
                    case SharpTokenType.LBracket:
                        {                            
                            Next();

                            if ( CurrTokenType == SharpTokenType.Int )
                            {
                                var varName = CurrTokenValue;
                                Next();

                                if ( CurrTokenType == SharpTokenType.RBracket )
                                {
                                    WriteString("int32");
                                }
                                else
                                {
                                    WriteString("(");
                                    WriteString(varName);
                                }
                            }
                            else
                            {
                                WriteString("(");
                                continue;
                            }
                        }
                        break;
                    default:
                        {
                            WriteToken();
                        }
                        break;
                }

                if (CurrTokenType == SharpTokenType.RBrace && level == 0)
                {
                    if (inElse)
                    {
                        WriteString("}\n");
                        inElse = false;
                    }

                    break;
                }

                Next();

                
                    
            }


            IgnoreWhiteSpace(true);
            WriteToken(); WriteString("\n");
            Expect(SharpTokenType.RBrace);
            
            
        }

    }
}
