using Photon.AST;
using Photon.Model;
using System.Collections.Generic;

namespace Photon.Parser
{
    public partial class ScriptParser
    {
        Stmt ParseFuncDecl()
        {
            Expect(TokenType.Func);

            var scope = OpenScope(ScopeType.Function);

            var ident = ParseIdent();

            var paramlist = ParseParameters(scope);

            if ( CurrTokenType == TokenType.LBrace )
            {
                var body = ParseBody(scope);

                var decl = new FuncDeclare(ident, paramlist, body, scope);

                ident.ScopeInfo = Declare(decl, _global, ident.Name, ident.DefinePos);

                return decl;
            }
            else
            {
                // 声明已经结束
                CloseScope();

                var decl = new DelegateDeclare(ident, paramlist, scope);

                ident.ScopeInfo = Declare(decl, _global, ident.Name, ident.DefinePos);

                return decl;
            }
            
        }

        List<Ident> ParseParameters( Scope s )
        {
            Expect(TokenType.LBracket);

            List<Ident> p = new List<Ident>();


            if (CurrTokenType != TokenType.RBracket )
            {
                while (true)
                {
                    var param = ParseIdent();
                    p.Add(param);

                    Declare(param, s, param.Name, param.DefinePos);

                    if (CurrTokenType != TokenType.Comma)
                    {
                        break;
                    }

                    Next();
                }
            }
            
            Expect(TokenType.RBracket);

            return p;
        }


        BlockStmt ParseBody(Scope s) 
        {
            Expect(TokenType.LBrace);

            _topScope = s;

            var list = ParseStatmentList();

            CloseScope();

            Expect(TokenType.RBrace);

            return new BlockStmt(list);
        }

        Stmt ParseVarDecl()
        {
            Expect(TokenType.Var);

            var idents = ParseIdentList();

            foreach( var i in idents )
            {
                Declare(i, _topScope, i.Name, i.DefinePos);
            }

            List<Expr> values = new List<Expr>();

            if (CurrTokenType == TokenType.Assign)
            {
                Next();

                values = ParseRHSList();

                var lhs = new List<Expr>();
                foreach( var id in idents )
                {
                    lhs.Add( id );
                }

                return new AssignStmt(lhs, values);
            }

            return new VarDeclareStmt(idents );
        }


    }
}
