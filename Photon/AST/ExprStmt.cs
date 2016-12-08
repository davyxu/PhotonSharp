using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    // 单独的一句表达式(例如: 纯函数调用)
    internal class ExprStmt : Stmt
    {
        public List<Expr> X;
        public TokenPos DefPos;

        public ExprStmt(List<Expr> x, TokenPos defpos )
        {
            X = x;
            DefPos = defpos;

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            foreach (var e in X)
            {
                yield return e;
            }
        }

        public override string ToString()
        {
            return "ExprStmt";
        }

        internal override void Compile(CompileParameter param)
        {

            foreach (var b in X)
            {
                // ExprStmt下不出现CallExpr可能是-foo();  1+foo() 这种奇葩写法
                // 返回值无法被回收, 所以直接报错
                if (!(b is CallExpr))
                {
                    throw new CompileException("invalid expression statement", DefPos);
                }

                b.Compile(param.SetLHS(false));
            }
        }
    }
}
