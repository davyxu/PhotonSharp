using Photon.AST;
using Photon.Scanner;
using System.Collections.Generic;

namespace Photon.Parser
{
    public partial class Parser
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

        public Chunk ParseChunk()
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
    }
}
