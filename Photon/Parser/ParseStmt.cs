using Photon.AST;
using Photon.Scanner;
using System.Collections.Generic;

namespace Photon.Parser
{
    public partial class ScriptParser
    {

        List<Stmt> ParseStatmentList()
        {
            var list = new List<Stmt>();

            while (_token.Type != TokenType.EOF &&
                 _token.Type != TokenType.RBrace)
            {
                list.Add(ParseStatement());
            }

            return list;
        }

        Chunk ParseChunk()
        {
            var list = ParseStatmentList();

            return new Chunk( new BlockStmt(list) );
        }

        ReturnStmt ParseReturnStmt()
        {
            Expect(TokenType.Return);

            List<Expr> results = new List<Expr>();

            if (_token.Type != TokenType.RBrace)
            {
                results = ParseRHSList();
            }

            return new ReturnStmt(results);
        }

        BlockStmt ParseBlockStmt()
        {
            Expect(TokenType.LBrace);

            OpenScope( _topScope, ScopeType.Block);

            var list = ParseStatmentList();

            CloseScope();

            Expect(TokenType.RBrace);

            return new BlockStmt(list);
        }

        Stmt ParseStatement()
        {
            switch (_token.Type)
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
            Expect(TokenType.If);

            var condition = ParseRHS();

            var body = ParseBlockStmt();

            BlockStmt elseBody;

            if (_token.Type == TokenType.Else)
            {
                Next();
                elseBody = ParseBlockStmt();
            }
            else
            {
                elseBody = new BlockStmt();
            }

            return new IfStmt(condition, body, elseBody);
        }

        Stmt ParseForInit( )
        {
            var ident = ParseIdent();

            Declare( ident, _topScope, ident.Name );

            Expect( TokenType.Assign );

            var expr = ParseRHS();

            return new AssignStmt( ident, expr );
        }

        ForStmt ParseForStmt()
        {
            Expect(TokenType.For);

            OpenScope(_topScope, ScopeType.For);

            var init = ParseForInit();
           
            Expect(TokenType.SemiColon);


            var conStmt = ParseSimpleStmt();

            Expect(TokenType.SemiColon);

            var post = ParseSimpleStmt();

            var body = ParseBlockStmt();

            CloseScope();

            var condtion = conStmt as ExprStmt;

            return new ForStmt( init, condtion.X[0], post, body );
            
        }

        WhileStmt ParseWhileStmt()
        {
            Expect(TokenType.While);

            var condition = ParseRHS();

            var body = ParseBlockStmt();

            return new WhileStmt(condition, body);
        }
    }
}
