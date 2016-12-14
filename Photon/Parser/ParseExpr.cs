

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

        BasicLit ParseBasicLit()
        {
            switch (CurrTokenType)
            {
                case TokenType.Number:
                case TokenType.QuotedString:
                    {
                        var x = new BasicLit(_token.Value, CurrTokenType, CurrTokenPos);
                        Next();
                        return x;
                    }
            }

            throw new CompileException("Expect Literal value", CurrTokenPos);
        }

        Expr ParseOperand(bool lhs)
        {
            var defPos = CurrTokenPos;

            switch (CurrTokenType)
            {
                case TokenType.Identifier: // a = b
                    {
                        var x = ParseIdent();

                        ScopeMgr.Resolve(x);

                        return x;
                    }
                case TokenType.Base: // a= base.foo()
                    {
                        Next();

                        return new BaseLit(defPos);
                    }                    
                case TokenType.Number:
                case TokenType.QuotedString:
                    {
                        var x = new BasicLit(_token.Value, CurrTokenType, CurrTokenPos);
                        Next();
                        return x;
                    }
                case TokenType.LParen: // () 括号表达式
                    {
                        Next();

                        var x = ParseExpr(false);

                        var rparenPos = CurrTokenPos;
                        Expect(TokenType.RParen);

                        return new ParenExpr(x, defPos, rparenPos);
                    }
                case TokenType.LBracket: // a = [] 数组初始化
                    {
                        Next();

                        List<BasicLit> values = new List<BasicLit>();

                        while (CurrTokenType != TokenType.RBracket &&
                                CurrTokenType != TokenType.EOF)
                        {
                            values.Add(ParseBasicLit());


                            if (CurrTokenType == TokenType.RBracket ||
                                CurrTokenType == TokenType.EOF)
                            {
                                break;
                            }

                            Expect(TokenType.Comma);
                        }

                       
                        var rPos = CurrTokenPos;
                        Expect(TokenType.RBracket);

                        return new ArrayExpr(values, defPos, rPos);
                    }
                case TokenType.LBrace: // a = {} Map初始化
                    {
                        Next();

                        Dictionary<BasicLit, Expr> mapValues = new Dictionary<BasicLit, Expr>();

                        while (CurrTokenType != TokenType.RBrace &&
                                CurrTokenType != TokenType.EOF)
                        {
                            var key = ParseBasicLit();

                            Expect(TokenType.Colon);

                            var value = ParseRHS();

                            mapValues.Add(key, value);

                            if (CurrTokenType == TokenType.RBrace ||
                                CurrTokenType == TokenType.EOF)
                            {
                                break;
                            }

                            Expect(TokenType.Comma);
                        }


                        var rPos = CurrTokenPos;
                        Expect(TokenType.RBrace);

                        return new MapExpr(mapValues, defPos, rPos);
                    }      
                case TokenType.Func: // a = func( ) {}
                    {
                        
                        Next();

                        var scope = ScopeMgr.OpenScope(ScopeType.Closure, defPos);
                        var paramlist = ParseParameters(scope, false);

                        var body = ParseBody(scope);

                        var funclit = new FuncLit(body, new FuncType( defPos, paramlist, scope) );

                        return funclit;
                    }                    
            }

            throw new CompileException("Unknown operand", CurrTokenPos);
        }

        CallExpr ParseCallExpr( Expr func )
        {
            var lpos = CurrTokenPos;
            Expect(TokenType.LParen);

            if (CurrTokenType != TokenType.RParen)
            {
                var args = ParseRHSList();

                var rpos = CurrTokenPos;
                Expect(TokenType.RParen);


                return new CallExpr(func, args, ScopeMgr.TopScope, lpos, rpos );
            }

            var rpos2 = CurrTokenPos;
            Expect(TokenType.RParen);

            // 空参数
            return new CallExpr(func, new List<Expr>(), ScopeMgr.TopScope, lpos, rpos2);
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


                        ScopeMgr.Resolve(sel, pkg.TopScope);
                    }
                    break;
                // 实例.函数名
                case SymbolUsage.Variable:
                case SymbolUsage.Parameter:
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

            int callTimes = 0;

            while (parsing)
            {
                switch( CurrTokenType )
                {
                        // a.b
                    case TokenType.Dot:
                        {
                            var dotpos = CurrTokenPos;
                            Next();

                            ScopeMgr.Resolve(x);



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
                    case TokenType.LBracket:
                        {
                            var lpos = CurrTokenPos;

                            ScopeMgr.Resolve(x);

                            Next();

                            var index = ParseRHSList();

                            var rpos = CurrTokenPos;
                            Expect(TokenType.RBracket);

                            x = new IndexExpr(x, index[0], lpos, rpos);
                        }
                        break;
                    case TokenType.LParen: 
                        {
                            callTimes++;

                            // 函数调用不能连续， foo()()是不可以的
                            if ( callTimes >1)
                            {
                                throw new CompileException("invalid call statement", CurrTokenPos);
                            }

                            ScopeMgr.Resolve(x);

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
            var oppos = CurrTokenPos;

            switch (op)
            {
                case TokenType.Add:
                case TokenType.Sub:
                    {   
                        Next();
                        var x = ParsePrimaryExpr( false );

                        return new UnaryExpr(x, op, oppos);
                    }
                case TokenType.Int32:
                case TokenType.Int64:
                case TokenType.Float32:
                case TokenType.Float64:
                case TokenType.Len:
                    {
                        Next();
                        Expect(TokenType.LParen);
                        var x = ParseExpr(false);
                        Expect(TokenType.RParen);
                        return new UnaryExpr(x, op, oppos);
                    }
                case TokenType.New:
                    {
                        return ParseNewExpr();
                    }
            }

            return ParsePrimaryExpr(lhs);
        }

        Expr ParseNewExpr()
        {
            var oppos = CurrTokenPos;

            Expect(TokenType.New);
            Expect(TokenType.LParen);

            Ident pkgName = null;
            Ident className;

            Ident nameA = ParseIdent();
            Ident nameB;

            if (CurrTokenType == TokenType.Dot)
            {
                Next();
                nameB = ParseIdent();
                className = nameB;
                pkgName = nameA;
            }
            else
            {
                className = nameA;
            }

            Expect(TokenType.RParen);


            return new NewExpr(className, pkgName, oppos);
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

                    ScopeMgr.Resolve(x);

                    var y = ParseBinaryExpr(false, prec + 1);


                    ScopeMgr.Resolve(y);

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
                ScopeMgr.Resolve(x);
            }

            return list;
        }

        List<Expr> ParseRHSList()
        {
            var list = ParseExprList(false);

            foreach (var x in list)
            {
                ScopeMgr.Resolve(x);
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
            return new ExprStmt( x, CurrTokenPos );
        }
    }
}
