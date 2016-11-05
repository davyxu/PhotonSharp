using System.Collections.Generic;
using Photon.Model;
using Photon.Model;

namespace Photon.AST
{

    public class CallExpr : Expr
    {
        public Expr Func;
        public List<Expr> Args;
        public Scope S;

        public CallExpr(Expr f, List<Expr> args, Scope s )
        {
            Func = f;
            Args = args;
            S = s;

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

        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            Func.Compile(exe, cm, false);

            foreach (var arg in Args)
            {
                arg.Compile(exe, cm, false);                
            }

            // 单独的一句时, 需要平衡数据栈
            int needBalanceDataStack = 0 ;
            if ( Parent is ExprStmt )
            {
                needBalanceDataStack = 1;
            }


            cm.Add(new Command(Opcode.Call, Args.Count, needBalanceDataStack));
        }
    }
}
