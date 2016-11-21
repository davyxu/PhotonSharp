using System.Collections.Generic;
using SharpLexer;

namespace Photon
{

    public class CallExpr : Expr
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

            var sel = Func as SelectorExpr;
            if (sel != null)
            {
                var packageName = sel.X as Ident;

                // 可能是a.b.c()的多个selector调用, 现在暂时不处理这种复杂情况
                if (packageName == null)
                {
                    throw new ParseException("invalid function entry", LParen);
                }

                //var funcName = sel.Selector;
            }
            else
            {
                // 本包及动态闭包调用
                Func.Compile(param.SetLHS(false));
            }

            param.CS.Add(new Command(Opcode.CALLF, Args.Count, NeedBalanceDataStack)).SetCodePos(LParen);
        }
    }
}
