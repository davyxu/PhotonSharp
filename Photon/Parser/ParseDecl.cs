

using System.Collections.Generic;

namespace Photon
{
    internal partial class Parser
    {
        Stmt ParseFuncDecl()
        {
            var funcPos = CurrTokenPos;

            Expect(TokenType.Func);

            var scope = ScopeMgr.OpenScope(ScopeType.Function, funcPos);

            Ident funcName;
            Ident className = null ;

            var NameA = ParseIdent();

            Scope funcAtScope;

            if (CurrTokenType == TokenType.Dot)
            {
                Next();

                funcName = ParseIdent();

                className = NameA;
                
                funcAtScope = ScopeMgr.GetClassScope(className.Name);

                // 主定义文件还未出现， 所以暂时创建空的scope
                if ( funcAtScope == null )
                {
                    funcAtScope = ScopeMgr.OpenClassScope(className.Name, funcPos);
                    ScopeMgr.CloseScope();
                }
            }
            else
            {
                funcAtScope = ScopeMgr.PackageScope;
                funcName = NameA;
            }


            var paramlist = ParseParameters(scope, className != null );
            
            if ( CurrTokenType == TokenType.LBrace )
            {
                

                var decl = new FuncDeclare(funcName, new FuncType(funcPos, paramlist, scope));
                decl.ClassName = className;

                funcName.Symbol = ScopeManager.Declare(decl, funcAtScope, funcName.Name, funcName.DefinePos, SymbolUsage.Func);


                decl.Body = ParseBody(scope);

                decl.BuildRelation();


                

                return decl;
            }
            else
            {
                // 声明已经结束
                ScopeMgr.CloseScope();

                // 函数前置声明
                var decl = new DelegateDeclare(funcName, new FuncType(funcPos, paramlist, scope) );

                funcName.Symbol = ScopeManager.Declare(decl, ScopeMgr.PackageScope, funcName.Name, funcName.DefinePos, SymbolUsage.Delegate);

                return decl;
            }
            
        }

        List<Ident> ParseParameters( Scope s, bool isMethod )
        {
            Expect(TokenType.LParen);

            List<Ident> p = new List<Ident>();


            if (CurrTokenType != TokenType.RParen )
            {
                while (true)
                {
                    var param = ParseIdent();
                    p.Add(param);


                    SymbolUsage usage;
                    if (isMethod && p.Count == 1 )
                    {
                        usage = SymbolUsage.SelfParameter;
                    }
                    else
                    {
                        usage = SymbolUsage.Parameter;
                    }


                    ScopeManager.Declare(param, s, param.Name, param.DefinePos, usage);

                    if (CurrTokenType != TokenType.Comma)
                    {
                        break;
                    }

                    Next();
                }
            }
            
            Expect(TokenType.RParen);

            return p;
        }


        Stmt ParseConstDecl()
        {
            var defpos = CurrTokenPos;
            Expect(TokenType.Const);

            var ident = ParseIdent();

            ScopeManager.Declare(ident, ScopeMgr.TopScope, ident.Name, ident.DefinePos, SymbolUsage.Constant);

            List<Expr> values = new List<Expr>();

            var assignPos = CurrTokenPos;

            Expect(TokenType.Assign);

            var value = ParseExpr(false);

            return new ConstDeclareStmt(ident, value, defpos);
        }


        Stmt ParseVarDecl()
        {
            var defpos = CurrTokenPos;
            Expect(TokenType.Var);

            var idents = ParseIdentList();

            foreach( var i in idents )
            {
                ScopeManager.Declare(i, ScopeMgr.TopScope, i.Name, i.DefinePos, SymbolUsage.Variable);
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

                return new AssignStmt(lhs, values, assignPos, TokenType.Assign);
            }

            return new VarDeclareStmt(idents, defpos);
        }


        Stmt ParseClassDecl( )
        {
            List<Ident> memberVar = new List<Ident>();

            var defpos = CurrTokenPos;
            Expect(TokenType.Class);

            var className = ParseIdent();

            var scope = ScopeMgr.OpenClassScope(className.Name, defpos);

            
            Ident parentName = null;
            var colonPos = CurrTokenPos;
            if ( CurrTokenType == TokenType.Colon )
            {

                Next();

                parentName = ParseIdent();
            }

            Expect(TokenType.LBrace);


            var decl = new ClassDeclare(className, parentName, scope, memberVar, defpos);

            if (parentName != null )
            {
                decl.ColonPos = colonPos;
            }

            className.Symbol = ScopeManager.Declare(decl, ScopeMgr.PackageScope, className.Name, className.DefinePos, SymbolUsage.Class);

            while (CurrTokenType != TokenType.RBrace)
            {
                var param = ParseIdent();
                memberVar.Add(param);

                ScopeManager.Declare(param, scope, param.Name, param.DefinePos, SymbolUsage.Member);                
            }

            ScopeMgr.CloseScope();

            Next();

            return decl;
        }


    }
}
