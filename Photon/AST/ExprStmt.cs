
using System.Collections.Generic;

namespace Photon
{
    // 单独的一句表达式(例如: 纯函数调用)
    public class ExprStmt : Stmt
    {
        public List<Expr> X;
        public ExprStmt(List<Expr> x)
        {
            X = x;

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

        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {

            foreach (var b in X)
            {
                b.Compile(exe, cm, false);
            }
        }
    }
}
