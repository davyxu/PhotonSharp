using Photon.AST;
using Photon.Model;
using SharpLexer;
using System.Collections.Generic;

namespace Photon.Parser
{
    public partial class ScriptParser
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
                        var x = new BasicLit(_token.Value, CurrTokenType);
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
                        var paramlist = ParseParameters(scope);

                        var body = ParseBody(scope);

                        var funclit = new FuncLit(body, new FuncType( funcPos, paramlist, scope) );

                        return funclit;
                    }                    
            }

            return new BadExpr();
        }

        Expr ParseSelector( Expr x )
        {
            var ident = ParseIdent();

            return new SelectorExpr(x, ident);
        }

        Expr ParseIndex( Expr x )
        {
            var index = ParseRHSList( );

            return new IndexExpr(x, index[0]);
        }

        CallExpr ParseCallExpr( Expr func )
        {
            Expect(TokenType.LBracket);

            if (CurrTokenType != TokenType.RBracket)
            {
                var args = ParseRHSList();

                Expect(TokenType.RBracket);


                return new CallExpr(func, args, _topScope);
            }

            Expect(TokenType.RBracket);

            // 空参数
            return new CallExpr(func, new List<Expr>(), _topScope);
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
                            Next();

                            Resolve(x);

                            switch (CurrTokenType)
                            {
                                case TokenType.Identifier:
                                    {
                                        x = ParseSelector(x);
                                    }
                                    break;
                                default:
                                    Error("expect selector");
                                    break;
                            }
                        
                        }
                        break;
                        // a[index]
                    case TokenType.LSqualBracket:
                        {
                            Resolve(x);

                            Next();

                            x = ParseIndex(x);

                            Expect(TokenType.RSqualBracket);
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
            switch (CurrTokenType)
            {
                case TokenType.Add:
                case TokenType.Sub:
                    {
                        var op = CurrTokenType;
                        Next();
                        var x = ParsePrimaryExpr( false );

                        return new UnaryExpr(x, op);
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

                    Next();

                    Resolve(x);

                    var y = ParseBinaryExpr(false, prec + 1);
                    
                    
                    Resolve(y);

                    x = new BinaryExpr(x, y, op);
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


            if (CurrTokenType == TokenType.Assign)
            {
                Next();

                var y = ParseRHSList();

                return new AssignStmt(x, y);
            }

            // 函数调用
            return new ExprStmt( x );
        }
    }
}
