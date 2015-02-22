using Photon.AST;
using Photon.Scanner;
using System.Collections.Generic;

namespace Photon.Parser
{
    public partial class Parser
    {
        int GetTokenPrecedence()
        {
            switch (_token.Type)
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
            var name = "_";

            if ( _token.Type == TokenType.Identifier )
            {
                name = _token.Value;
                Next();
            }
            else
            {
                Expect(TokenType.Identifier);
            }

            return new Ident(name);
        }

        List<Ident> ParseIdentList( )
        {
            List<Ident> list = new List<Ident>();
            list.Add(ParseIdent());

            while (_token.Type == TokenType.Comma)
            {
                Next();
                list.Add(ParseIdent());
            }

            return list;
        }

        Expr ParseOperand(bool lhs)
        {
            switch (_token.Type)
            {
                case TokenType.Identifier:
                    {
                        var x = ParseIdent();

                        if (lhs)
                        {
                            Resolve(x);
                        }

                        return x;
                    }
                case TokenType.Number:
                case TokenType.QuotedString:
                    {
                        var x = new BasicLit(_token.Value, _token.Type);
                        Next();
                        return x;
                    }
                case TokenType.LBracket:
                    {

                    }
                    break;
                case TokenType.Func: // a = func( ) {}
                    {

                    }
                    break;
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

            var args = ParseRHSList();

            Expect(TokenType.RBracket);

            return new CallExpr(func, args);
        }

        Expr ParsePrimaryExpr(bool lhs)
        {
            var x = ParseOperand(lhs);

            bool parsing = true;

            while (parsing)
            {
                switch( _token.Type )
                {
                        // a.b
                    case TokenType.Dot:
                        {
                            Next();

                            if ( lhs )
                            {
                                Resolve(x);
                            }

                            switch (_token.Type)
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
                            if (lhs)
                            {
                                Resolve(x);
                            }

                            Next();

                            x = ParseIndex(x);

                            Expect(TokenType.RSqualBracket);
                        }
                        break;
                    case TokenType.LBracket:
                        {
                            if (lhs)
                            {
                                Resolve(x);
                            }


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
            switch (_token.Type)
            {
                case TokenType.Add:
                case TokenType.Sub:
                    {
                        var op = _token.Type;
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

                    var op = _token.Type;

                    Next();

                    if ( lhs )
                    {
                        Resolve(x);
                        lhs = false;
                    }

                    var y = ParseBinaryExpr(false, prec + 1);

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

            while( _token.Type == TokenType.Comma )
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
            return ParseExprList(false);
        }

       

        Stmt ParseSimpleStmt()
        {
            var x = ParseLHSList();


            if (_token.Type == TokenType.Assign)
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
