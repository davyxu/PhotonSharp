
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

        Stmt ParseForInit( )
        {
            var ident = ParseIdent();

            ScopeManager.Declare(ident, ScopeMgr.TopScope, ident.Name, ident.DefinePos, SymbolUsage.Variable);

            var assignPos = CurrTokenPos;

            Expect( TokenType.Assign );

            var expr = ParseRHS();

            return new AssignStmt(ident, expr, assignPos, TokenType.Assign);
        }

        ForStmt ParseForStmt()
        {
            var defPos = CurrTokenPos;
            Expect(TokenType.For);

            ScopeMgr.OpenScope( ScopeType.For, defPos );

            var init = ParseForInit();
           
            Expect(TokenType.SemiColon);


            var conStmt = ParseSimpleStmt();

            Expect(TokenType.SemiColon);

            var post = ParseSimpleStmt();

            var body = ParseBlockStmt();

            ScopeMgr.CloseScope();

            var condtion = conStmt as ExprStmt;

            return new ForStmt(init, condtion.X[0], post, body, defPos);
            
        }

        WhileStmt ParseWhileStmt()
        {
            var defpos = CurrTokenPos;
            Expect(TokenType.While);

            var condition = ParseRHS();

            var body = ParseBlockStmt();

            return new WhileStmt(condition, body, defpos);
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
