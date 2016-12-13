using SharpLexer;

namespace Photon
{
    internal partial class Parser
    {
        Lexer _lexer;

        Token _token;        

        Executable _exe;

        ContentLoader _loader;

        ScopeManager _scopemgr;

        internal Executable Exe
        {
            get { return _exe; }
        }

        internal ContentLoader Loader
        {
            get { return _loader; }
        }

        internal ScopeManager ScopeMgr
        {
            get { return _scopemgr; }
        }

        public Parser(Executable exe, ContentLoader loader, ScopeManager scopemgr)
        {
            _exe = exe;
            _loader = loader;
            _scopemgr = scopemgr;
        }

        public FileNode ParseFile(SourceFile file)
        {
            _lexer = NewLexer(file);

            Next();

            var importList = ParseImportStmt( );

            var lpos = CurrTokenPos;
            var list = ParseStatmentList();
            var rpos = CurrTokenPos;

            return new FileNode(new BlockStmt(list, lpos, rpos), importList);            
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
                throw new CompileException("unknown token", CurrTokenPos);
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
                throw new CompileException(string.Format("expect token: {0}", t.ToString()), CurrTokenPos);
            }

            var tk = _token;

            Next();

            return tk;
        }


        public static void PrintAST(Node n, string indent = "")
        {
            Logger.DebugLine(indent + n.ToString());

            foreach (var c in n.Child())
            {
                PrintAST(c, indent + "\t");
            }
        }
    }
}
