using System.Diagnostics;
using SharpLexer;

namespace Photon
{
    public partial class Parser
    {
        Lexer _lexer;

        Token _token;

        Chunk _chunk = new Chunk();

        internal Executable Exe
        {
            get;
            set;
        }

        public Chunk Chunk
        {
            get { return _chunk; }
        }

        public Parser()
        {
            InitScope();
        }

        public void Import(SourceFile file)
        {
            _lexer = NewLexer(file);

            Next();

            ParseChunk();
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
                throw new ParseException("unknown token", CurrTokenPos);
            }
        }

        TokenType CurrTokenType
        {
            get { return (TokenType)_token.MatcherID; }
        }

        internal TokenPos CurrTokenPos
        {
            get { return _token.Pos; }
        }

        string CurrTokenValue
        {
            get { return _token.Value; }
        }

        Token Expect(TokenType t)
        {
            if (CurrTokenType != t)
            {
                throw new ParseException(string.Format("expect token: {0}", t.ToString()), CurrTokenPos);
            }

            var tk = _token;

            Next();

            return tk;
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
