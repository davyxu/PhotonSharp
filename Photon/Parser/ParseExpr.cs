using Photon.AST;
using Photon.Scanner;

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

        

        Expr ParseOperand()
        {
            switch (_token.Type)
            {
                case TokenType.Identifier:
                    {
                        var ident = ParseIdent();

                        // TODO Resolve

                        return ident;
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
                case TokenType.Func:
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
            var index = ParseRHS( );

            return new IndexExpr(x, index);
        }

        Expr ParsePrimaryExpr()
        {
            var x = ParseOperand();

            bool parsing = true;

            while (parsing)
            {
                switch( _token.Type )
                {
                        // a.b
                    case TokenType.Dot:
                        {
                            Next();

                            // TODO Resolve x

                            switch (_token.Type)
                            {
                                case TokenType.Identifier:
                                    {
                                        x = ParseSelector(x);
                                    }
                                    break;
                                default:
                                    ErrorExpect("expect selector");
                                    break;
                            }
                        
                        }
                        break;
                        // a[index]
                    case TokenType.LSqualBracket:
                        {
                            // TODO Resolve x

                            Next();

                            x = ParseIndex(x);

                            Expect(TokenType.RSqualBracket);
                        }
                        break;
                    default:
                        parsing = false;
                        break;
                }
            }


            return x;
        }
        Expr ParseUnaryExpr()
        {
            switch (_token.Type)
            {
                case TokenType.Add:
                case TokenType.Sub:
                    {
                        var op = _token.Type;
                        Next();
                        var x = ParsePrimaryExpr();

                        return new UnaryExpr(x, op);
                    }
            }

            return ParsePrimaryExpr();
        }

        Expr ParseBinaryExpr(int prec1)
        {
            Expr x = ParseUnaryExpr();

            for (var prec = GetTokenPrecedence(); prec > prec1; prec--)
            {
                while (true)
                {
                    var opprec = GetTokenPrecedence();
                    if (opprec != prec)
                        break;

                    var op = _token.Type;

                    Next();

                    var y = ParseBinaryExpr(prec + 1);

                    x = new BinaryExpr(x, y, op);
                }
            }


            return x;
        }

        Expr ParseExpr()
        {
            return ParseBinaryExpr(1);
        }

        Expr ParseLHS()
        {
            return ParseExpr();
        }

        Expr ParseRHS()
        {
            return ParseExpr();
        }

        Stmt ParseSimpleStmt()
        {
            var x = ParseLHS();


            if (_token.Type == TokenType.Assign)
            {
                Next();

                var y = ParseRHS();

                return new AssignStmt(x, y);
            }

            return new BadStmt();
        }
    }
}
