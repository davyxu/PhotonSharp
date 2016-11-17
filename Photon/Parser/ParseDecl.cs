

using System.Collections.Generic;

namespace Photon
{
    public partial class CodeParser
    {
        Stmt ParseFuncDecl()
        {
            var funcPos = CurrTokenPos;

            Expect(TokenType.Func);

            var scope = OpenScope(ScopeType.Function, funcPos );

            var ident = ParseIdent();

            var paramlist = ParseParameters(scope);

            if ( CurrTokenType == TokenType.LBrace )
            {
                var body = ParseBody(scope);

                var decl = new FuncDeclare(ident, body, new FuncType(funcPos, paramlist, scope) );

                ident.ScopeInfo = Declare(decl, _global, ident.Name, ident.DefinePos);

                return decl;
            }
            else
            {
                // 声明已经结束
                CloseScope();

                var decl = new DelegateDeclare(ident, new FuncType(funcPos, paramlist, scope) );

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
            var lpos = CurrTokenPos;
            Expect(TokenType.LBrace);

            _topScope = s;

            var list = ParseStatmentList();

            CloseScope();

            var rpos = CurrTokenPos;
            Expect(TokenType.RBrace);

            return new BlockStmt(list, lpos, rpos);
        }

        Stmt ParseVarDecl()
        {
            var defpos = CurrTokenPos;
            Expect(TokenType.Var);

            var idents = ParseIdentList();

            foreach( var i in idents )
            {
                Declare(i, _topScope, i.Name, i.DefinePos);
            }

            List<Expr> values = new List<Expr>();

            var assignPos = CurrTokenPos;

            if (CurrTokenType == TokenType.Assign)
            {
                Next();

                values = ParseRHSList();

                var lhs = new List<Expr>();
                foreach( var id in idents )
                {
                    lhs.Add( id );
                }

                return new AssignStmt(lhs, values, assignPos);
            }

            return new VarDeclareStmt(idents, defpos);
        }


    }
}
