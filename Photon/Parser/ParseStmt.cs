
using System.Collections.Generic;

namespace Photon
{
    public partial class CodeParser
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

        Chunk ParseChunk()
        {
            var lpos = CurrTokenPos;
            var list = ParseStatmentList();
            var rpos = CurrTokenPos;

            return new Chunk( new BlockStmt(list, lpos, rpos) );
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

            OpenScope(ScopeType.Block, defPos);

            var list = ParseStatmentList();

            CloseScope();

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
                case TokenType.LBracket:
                case TokenType.Add:
                case TokenType.Sub:
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
            }

            return new BadStmt();
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

            Declare( ident, _topScope, ident.Name, ident.DefinePos );

            var assignPos = CurrTokenPos;

            Expect( TokenType.Assign );

            var expr = ParseRHS();

            return new AssignStmt( ident, expr, assignPos );
        }

        ForStmt ParseForStmt()
        {
            var defPos = CurrTokenPos;
            Expect(TokenType.For);

            OpenScope( ScopeType.For, defPos );

            var init = ParseForInit();
           
            Expect(TokenType.SemiColon);


            var conStmt = ParseSimpleStmt();

            Expect(TokenType.SemiColon);

            var post = ParseSimpleStmt();

            var body = ParseBlockStmt();

            CloseScope();

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
    }
}
