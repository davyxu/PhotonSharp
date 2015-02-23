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
                    {
                        return ParseSimpleStmt();
                    }
                case TokenType.Func:
                    return ParseFuncDecl();
                case TokenType.Return:
                    return ParseReturnStmt();
                case TokenType.Var:
                    return ParseVarDecl();
            }

            return new BadStmt();
        }
    }
}
