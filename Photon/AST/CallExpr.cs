using System.Collections.Generic;
using SharpLexer;

namespace Photon
{

    internal class CallExpr : Expr
    {
        public Expr Func;
        public List<Expr> Args;
        public Scope S;

        public TokenPos LParen;
        public TokenPos RParen;

        public CallExpr(Expr f, List<Expr> args, Scope s, TokenPos lparen, TokenPos rparen)
        {
            Func = f;
            Args = args;
            S = s;
            LParen = lparen;
            RParen = rparen;

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            yield return Func;

            foreach( var e in Args)
            {
                yield return e;
            }
        }

        public override string ToString()
        {
            return "CallExpr";
        }

        int NeedBalanceDataStack
        {
            get {
                // 单独的一句时, 需要平衡数据栈
                if (Parent is ExprStmt)
                    return 1;

                return 0;
            }
        }

        internal override void Compile(CompileParameter param)
        {
            // 先放参数
            foreach (var arg in Args)
            {
                arg.Compile(param.SetLHS(false));                
            }

                // 本包及动态闭包调用
            Func.Compile(param.SetLHS(false));

            param.CS.Add(new Command(Opcode.CALLF, Args.Count, NeedBalanceDataStack)).SetCodePos(LParen);
        }
    }
}
