using Photon.AST;
using Photon.Scanner;
using System.Collections.Generic;

namespace Photon.Parser
{
    public partial class Parser
    {
        FuncDeclare ParseFuncDecl( )
        {
            Expect(TokenType.Func);

            var ident = ParseIdent();

            var paramlist = ParseParameters();

            var body = ParseBody();

            return new FuncDeclare(ident.Name, paramlist, body);
        }

        List<Ident> ParseParameters()
        {
            Expect(TokenType.LBracket);

            List<Ident> p = new List<Ident>();


            if (_token.Type != TokenType.RBracket )
            {
                while (true)
                {
                    var param = ParseIdent();
                    p.Add(param);

                    if (_token.Type != TokenType.Comma)
                    {
                        break;
                    }

                    Next();
                }
            }
            
            Expect(TokenType.RBracket);

            return p;
        }

        
        BlockStmt ParseBody() 
        {
            Expect(TokenType.LBrace);

            var list = ParseStatmentList();

            Expect(TokenType.RBrace);

            return new BlockStmt(list);
        }
    }
}
