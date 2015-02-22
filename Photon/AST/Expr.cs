using System.Collections.Generic;
using Photon.Scanner;

namespace Photon.AST
{
    public class Node
    {
        public virtual IEnumerable<Node> Child()
        {
            yield break;
        }
    }


    public class Ident : Expr
    {
        public string Name;

        public Ident( string n )
        {
            Name = n;
        }

        public override string ToString( )
        {
            return Name;
        }
    }


    public class Expr : Node
    {

    }

    public class BadExpr : Expr
    {

    }

    public class BasicLit : Expr
    {
        public TokenType Type;
        public string Value;
        public BasicLit(string v, TokenType t)
        {
            Value = v;
            Type = t;
        }

        public override string ToString()
        {
            return Value;
        }
    }


    public class UnaryExpr : Expr
    {
        public TokenType Op;
        public Expr X;
        public UnaryExpr(Expr x, TokenType t)
        {
            X = x;
            Op = t;
        }

        public override IEnumerable<Node> Child()
        {
            yield return X;
        }
    }

    // a.b   x=a  selector=b
    public class SelectorExpr : Expr
    {
        public Ident Selector;
        public Expr X;
        public SelectorExpr(Expr x, Ident i )
        {
            X = x;
            Selector = i;
        }

        public override IEnumerable<Node> Child()
        {
            yield return X;
        }

        public override string ToString()
        {
            return string.Format("SelectorExpr {0}", Selector.ToString());
        }
    }

    public class IndexExpr : Expr
    {
        public Expr X;
        public Expr Index;

        public IndexExpr(Expr x, Expr index)
        {
            X = x;
            Index = index;
        }

        public override IEnumerable<Node> Child()
        {
            yield return X;

            yield return Index;
        }

        public override string ToString()
        {
            return "IndexExpr";
        }
    }

    public class BinaryExpr : Expr
    {
        public Expr X;
        public TokenType Op;
        public Expr Y;

        public BinaryExpr( Expr x, Expr y, TokenType t )
        {
            X = x;
            Y = y;
            Op = t;
        }

        public override IEnumerable<Node> Child()
        {
            yield return X;

            yield return Y;
        }

        public override string ToString()
        {
            return string.Format("BinaryExpr {0}", Op.ToString());
        }
    }



}
