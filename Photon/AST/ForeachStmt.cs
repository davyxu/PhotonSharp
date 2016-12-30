
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    // for k, v in x
    internal class ForeachStmt : Stmt
    {        
        public Ident Key;

        public Ident Value;

        public Expr X;

        public BlockStmt Body;

        public TokenPos ForPos;

        public TokenPos InPos;

        public Scope ScopeInfo;

        public ForeachStmt(Ident key, Ident value, Expr x, BlockStmt body, TokenPos forpos, TokenPos inpos, Scope s )
        {
            Key = key;
            Value = value;
            X = x;
            Body = body;            
            ForPos = forpos;
            InPos = inpos;
            ScopeInfo = s;

            BuildRelation();
        }


        public override string ToString()
        {
            return "ForRangeStmt";
        }

        public override IEnumerable<Node> Child()
        {
            if (Key != null)
            {
                yield return Key;
            }
            
            if (Value != null)
            {
                yield return Value;
            }


            yield return X;

            yield return Body;
        }

        Ident DelcareIteratorVar( )
        {
            var iter = new Ident(new Token(ForPos, null, "@Iterator"));
            iter.BaseScope = ScopeInfo;
            iter.Symbol = new Symbol();
            iter.Symbol.Name = iter.Name;
            iter.Symbol.Decl = this;
            iter.Symbol.DefinePos = ForPos;
            iter.Symbol.Usage = SymbolUsage.Variable;

            return iter;
        }
        // 手动分配1个iterator变量
        // k, v, iter = ITER( x, iter )
        // 
        internal override void Compile(CompileParameter param)
        {
           
        }

    }
}
