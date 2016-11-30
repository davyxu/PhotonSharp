

using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    internal partial class Parser
    {
        int GetTokenPrecedence()
        {
            switch (CurrTokenType)
            {
                case TokenType.Equal:
                case TokenType.NotEqual:
                case TokenType.LessEqual:
                case TokenType.LessThan:
                case TokenType.GreatEqual:
                case TokenType.GreatThan:
                    return 3;
                case TokenType.Add:
                case TokenType.Sub:
                    return 4;
                case TokenType.Mul:
                case TokenType.Div:
                    return 5;
            }

            return 0;
        }

        Ident ParseIdent( )
        {
            //var name = "_";
            

            if ( CurrTokenType == TokenType.Identifier )
            {
                var token = _token;
                Next();

                return new Ident(token);
            }
   
            Expect(TokenType.Identifier);

           return null;
        }

        List<Ident> ParseIdentList( )
        {
            List<Ident> list = new List<Ident>();
            list.Add(ParseIdent());

            while (CurrTokenType == TokenType.Comma)
            {
                Next();
                list.Add(ParseIdent());
            }

            return list;
        }

        Expr ParseOperand(bool lhs)
        {
            switch (CurrTokenType)
            {
                case TokenType.Identifier:
                    {
                        var x = ParseIdent();

                        Resolve(x);

                        return x;
                    }
                case TokenType.Number:
                case TokenType.QuotedString:
                    {
                        var x = new BasicLit(_token.Value, CurrTokenType, CurrTokenPos);
                        Next();
                        return x;
                    }
                case TokenType.LBracket:
                    {

                    }
                    break;
                case TokenType.Func: // a = func( ) {}
                    {
                        var funcPos = CurrTokenPos;
                        Next();

                        var scope = OpenScope(ScopeType.Closure, funcPos );
                        var paramlist = ParseParameters(scope, false);

                        var body = ParseBody(scope);

                        var funclit = new FuncLit(body, new FuncType( funcPos, paramlist, scope) );

                        return funclit;
                    }                    
            }

            return new BadExpr();
        }

        CallExpr ParseCallExpr( Expr func )
        {
            var lpos = CurrTokenPos;
            Expect(TokenType.LBracket);

            if (CurrTokenType != TokenType.RBracket)
            {
                var args = ParseRHSList();

                var rpos = CurrTokenPos;
                Expect(TokenType.RBracket);


                return new CallExpr(func, args, _topScope, lpos, rpos );
            }

            var rpos2 = CurrTokenPos;
            Expect(TokenType.RBracket);

            // 空参数
            return new CallExpr(func, new List<Expr>(), _topScope, lpos, rpos2);
        }

        void ResolveSelectorElement( Expr x, Ident sel, TokenPos dotpos )
        {
            var xident = x as Ident;
            if (xident == null)
                return;

            if ( xident.Symbol == null )
            {
                throw new CompileException(string.Format("{0} not defined", xident.Name), dotpos);
            }

            switch (xident.Symbol.Usage)
            {
                // 包.函数名
                case SymbolUsage.Package:
                    {
                        var pkg = Exe.GetPackageByName(xident.Name);
                        if (pkg == null)
                        {
                            throw new CompileException("package not found: " + xident.Name, dotpos);
                        }

                        // 包必须有一个顶级作用域
                        if (pkg.TopScope == null)
                        {
                            throw new CompileException("package should have a scope: " + xident.Name, dotpos);
                        }


                        Resolve(sel, pkg.TopScope);
                    }
                    break;
                // 实例.函数名
                case SymbolUsage.Variable:
                case SymbolUsage.Parameter:
                    {
                        // Class Scope
                        Resolve(sel, _topScope);
                    }
                    break;
                case SymbolUsage.SelfParameter:
                    {

                    }
                    break;
                default:
                    throw new CompileException("unknown usage", dotpos);
            }

        }
       

        Expr ParsePrimaryExpr(bool lhs)
        {
            var x = ParseOperand(lhs);

            bool parsing = true;

            while (parsing)
            {
                switch( CurrTokenType )
                {
                        // a.b
                    case TokenType.Dot:
                        {
                            var dotpos = CurrTokenPos;
                            Next();

                            Resolve(x);



                            switch (CurrTokenType)
                            {
                                case TokenType.Identifier:
                                    {
                                        var sel = ParseIdent();

                                        ResolveSelectorElement(x, sel, dotpos);
                        
                                        x = new SelectorExpr(x, sel, dotpos);
                                    }
                                    break;
                                default:                                    
                                   throw new CompileException("expect selector", CurrTokenPos);                         
                            }
                        
                        }
                        break;
                        // a[index]
                    case TokenType.LSqualBracket:
                        {
                            var lpos = CurrTokenPos;

                            Resolve(x);

                            Next();

                            var index = ParseRHSList();

                            var rpos = CurrTokenPos;
                            Expect(TokenType.RSqualBracket);

                            x = new IndexExpr(x, index[0], lpos, rpos);
                        }
                        break;
                    case TokenType.LBracket:
                        {

                            Resolve(x);

                            x = ParseCallExpr(x);

                        }
                        break;
                    default:
                        parsing = false;
                        break;
                }
            }


            return x;
        }
        Expr ParseUnaryExpr(bool lhs)
        {
            var op = CurrTokenType;

            switch (op)
            {
                case TokenType.Add:
                case TokenType.Sub:
                case TokenType.New:
                    {
                        
                        var oppos = CurrTokenPos;

                        Next();
                        var x = ParsePrimaryExpr( false );

                        if (op == TokenType.New )
                        {
                            var call = x as CallExpr;
                            if (call == null)
                            {
                                throw new CompileException("invalid new expression", oppos);
                            }

                            return new NewExpr(call, oppos);
                        }

                        return new UnaryExpr(x, op, oppos);
                    }
            }

            return ParsePrimaryExpr(lhs);
        }

        Expr ParseBinaryExpr(bool lhs, int prec1)
        {
            Expr x = ParseUnaryExpr( lhs );

            for (var prec = GetTokenPrecedence(); prec > prec1; prec--)
            {
                while (true)
                {
                    var opprec = GetTokenPrecedence();
                    if (opprec != prec)
                        break;

                    var op = CurrTokenType;
                    var oppos = CurrTokenPos;

                    Next();

                    Resolve(x);

                    var y = ParseBinaryExpr(false, prec + 1);
                    
                    
                    Resolve(y);

                    x = new BinaryExpr(x, y, op, oppos);
                }
            }


            return x;
        }

        Expr ParseExpr( bool lhs )
        {
            return ParseBinaryExpr(lhs, 1);
        }

        List<Expr> ParseExprList(bool lhs)
        {
            List<Expr> list = new List<Expr>();
            list.Add(ParseExpr(lhs));

            while( CurrTokenType == TokenType.Comma )
            {
                Next();
                list.Add(ParseExpr(lhs));
            }

            return list;
        }

        List<Expr> ParseLHSList()
        {
            var list = ParseExprList(true);

            foreach( var x in list )
            {
                Resolve(x);
            }

            return list;
        }

        List<Expr> ParseRHSList()
        {
            var list = ParseExprList(false);

            foreach (var x in list)
            {
                Resolve(x);
            }

            return list;
        }

        Expr ParseRHS()
        {
            return ParseExpr(false);
        }

       

        Stmt ParseSimpleStmt()
        {
            var x = ParseLHSList();

            var assignPos = CurrTokenPos;
            if (CurrTokenType == TokenType.Assign)
            {
                Next();

                var y = ParseRHSList();

                return new AssignStmt(x, y, assignPos);
            }

            // 函数调用
            return new ExprStmt( x );
        }
    }
}
