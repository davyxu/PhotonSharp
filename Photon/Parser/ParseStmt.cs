
using System.Collections.Generic;

namespace Photon
{
    internal partial class Parser
    {

        List<Stmt> ParseStatmentList()
        {
            var list = new List<Stmt>();

            while (CurrTokenType != TokenType.EOF &&
                 CurrTokenType != TokenType.RBrace)
            {
                list.Add(ParseStatement());
            }

            return list;
        }

        BlockStmt ParseBody(Scope s)
        {
            var lpos = CurrTokenPos;
            Expect(TokenType.LBrace);

            ScopeMgr.TopScope = s;

            var list = ParseStatmentList();

            ScopeMgr.CloseScope();

            var rpos = CurrTokenPos;
            Expect(TokenType.RBrace);

            return new BlockStmt(list, lpos, rpos);
        }

       
        ReturnStmt ParseReturnStmt()
        {
            var defpos = CurrTokenPos;
            Expect(TokenType.Return);

            List<Expr> results = new List<Expr>();

            if (CurrTokenType != TokenType.RBrace)
            {
                results = ParseRHSList();
            }

            return new ReturnStmt(results, defpos);
        }

        BlockStmt ParseBlockStmt()
        {
            var defPos = CurrTokenPos;
            Expect(TokenType.LBrace);

            ScopeMgr.OpenScope(ScopeType.Block, defPos);

            var list = ParseStatmentList();

            ScopeMgr.CloseScope();

            var rpos = CurrTokenPos;
            Expect(TokenType.RBrace);

            return new BlockStmt(list, defPos, rpos);
        }

        Stmt ParseStatement()
        {
            switch (CurrTokenType)
            {
                case TokenType.Identifier:
                case TokenType.Number:
                case TokenType.QuotedString:
                case TokenType.LParen:
                case TokenType.Add:
                case TokenType.Sub:
                case TokenType.Base:
                    return ParseSimpleStmt();
                case TokenType.Func:
                    return ParseFuncDecl();
                case TokenType.Return:
                    return ParseReturnStmt();
                case TokenType.If:
                    return ParseIfStmt();
                case TokenType.While:
                    return ParseWhileStmt();
                case TokenType.Break:
                    return ParseBreakStmt();
                case TokenType.Continue:
                    return ParseContinueStmt();
                case TokenType.For:
                    return ParseForStmt();
                case TokenType.Var:
                    return ParseVarDecl();
                case TokenType.Const:
                    return ParseConstDecl();
                case TokenType.Class:
                    return ParseClassDecl();
            }

            throw new CompileException("Invalid statement:" + CurrTokenType, CurrTokenPos);
        }

        IfStmt ParseIfStmt()
        {
            var defpos = CurrTokenPos;
            Expect(TokenType.If);

            var condition = ParseRHS();

            var body = ParseBlockStmt();

            BlockStmt elseBody;

            if (CurrTokenType == TokenType.Else)
            {
                Next();
                elseBody = ParseBlockStmt();
            }
            else
            {

                elseBody = new BlockStmt(CurrTokenPos, CurrTokenPos);
            }

            return new IfStmt(condition, body, elseBody, defpos);
        }

        Stmt ParseForStmt()
        {
            var defPos = CurrTokenPos;
            Expect(TokenType.For);

            var forscope = ScopeMgr.OpenScope(ScopeType.For, defPos);

            Stmt s1 = null;
            Stmt s2 = null;
            Stmt s3 = null;

            // 可能是foreach
            if (CurrTokenType != TokenType.SemiColon)
            {
                var idents = ParseIdentList();

                foreach (var i in idents)
                {
                    ScopeManager.Declare(i, ScopeMgr.TopScope, i.Name, i.DefinePos, SymbolUsage.Variable);
                }

                var opPos = CurrTokenPos;

                switch (CurrTokenType)
                {
                        // for numeral
                    case TokenType.Assign:
                        {
                            Next();

                            var values = ParseRHSList();

                            var lhs = new List<Expr>();
                            foreach (var id in idents)
                            {
                                lhs.Add(id);
                            }

                            s1 = new AssignStmt(lhs, values, opPos, TokenType.Assign);
                        }
                        break;
                    // for k, v in x {}
                    case TokenType.In:
                        {
                            Next();

                            var x = ParseRHS();

                            var body = ParseBlockStmt();

                            ScopeMgr.CloseScope();

                            if (idents.Count != 2)
                            {
                                throw new CompileException("Expect key & value in for-range", CurrTokenPos);
                            }

                            return new ForRangeStmt(idents[0], idents[1], x, opPos, defPos, forscope, body );
                        }
                    default:
                        throw new CompileException("Expect '=' or 'in' in for statement", CurrTokenPos);                        
                }
                
            }

            // s1和s2之间的分号
            Expect(TokenType.SemiColon);

            if ( CurrTokenType != TokenType.SemiColon )
            {
                s2 = ParseSimpleStmt();
            }

            // s2和s3之间的分号
            Expect(TokenType.SemiColon);

            if (CurrTokenType != TokenType.LBrace)
            {
                s3 = ParseSimpleStmt();
            }

            var body2 = ParseBlockStmt();

            ScopeMgr.CloseScope();

            Expr condition = null;

            if (s2 != null)
            {
                condition = (s2 as ExprStmt).X[0];
            }

            return new ForStmt(s1, condition, s3, defPos, forscope, body2);
        }

        BreakStmt ParseBreakStmt()
        {
            var defpos = CurrTokenPos;
            Expect(TokenType.Break);

            return new BreakStmt(defpos );
        }

        ContinueStmt ParseContinueStmt()
        {
            var defpos = CurrTokenPos;
            Expect(TokenType.Continue);

            return new ContinueStmt(defpos);
        }

        WhileStmt ParseWhileStmt()
        {
            var defpos = CurrTokenPos;
            Expect(TokenType.While);

            var condition = ParseRHS();

            var body = ParseBlockStmt();

            return new WhileStmt(condition, defpos, null, body);
        }

        List<ImportStmt> ParseImportStmt( )
        {
            var importList = new List<ImportStmt>();

            while (CurrTokenType == TokenType.Import &&
                 CurrTokenType != TokenType.EOF)
            {
                var defpos = CurrTokenPos;
                Expect(TokenType.Import);


                var tk = Expect(TokenType.QuotedString);

                var pkgName = new BasicLit(tk.Value, (TokenType)tk.MatcherID, tk.Pos);

                var n = new ImportStmt(pkgName, defpos);
                importList.Add(n);

                ScopeManager.Declare(n, ScopeMgr.PackageScope, tk.Value, defpos, SymbolUsage.Package);

                // 如果包存在, 就不会在定义
                var pkg = Exe.GetPackageByName(tk.Value);
                if (pkg == null)
                {
                    Compiler.Import(_exe, _loader, tk.Value, tk.Value, ImportMode.Directory);
                }
            }

            return importList;
        }
    }
}
