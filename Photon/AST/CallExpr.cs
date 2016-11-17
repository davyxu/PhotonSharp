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


        internal override void Resolve()
        {

        }

        Command _callCmdToResolve;
        string _funcName;        

        void AnalyseFuncEntry(Package pkg, CommandSet cm)
        {            
            // 名字访问, 可能是本地函数调用, 或动态变量访问
            var funcNameToken = Func as Ident;
            if ( funcNameToken != null )
            {
                var funcNameSymbol = S.FindSymbol(funcNameToken.Name);
                if ( funcNameSymbol == null )
                {
                    throw new ParseException("func name not found:" + funcNameToken.Name, LParen);
                }

                switch (funcNameSymbol.Usage )
                {
                    case SymbolUsage.Delegate:
                    case SymbolUsage.Func:
                        {
                            _funcName = funcNameSymbol.Name;
                            _callCmdToResolve=cm.Add(new Command(Opcode.CallD, NeedBalanceDataStack)).SetCodePos(LParen);
                        }
                        break;
                }
            }

            var sel = Func as SelectorExpr;
            if ( sel != null)
            {
                var packageName = sel.X as Ident;

                // 可能是a.b.c()的多个selector调用, 现在暂时不处理这种复杂情况
                if ( packageName == null )
                {
                    throw new ParseException("invalid function entry", LParen);
                }

                //var funcName = sel.Selector;
            }
            
        }

        static Node GetOneChild(Node n)
        {
            foreach (var c in n.Child())
            {
                return c;
            }

            return null;
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

        internal override void Compile(Package pkg, CommandSet cm, bool lhs)
        {
            // 先放参数
            foreach (var arg in Args)
            {
                arg.Compile(pkg, cm, false);                
            }

            AnalyseFuncEntry(pkg, cm);

            // 再放函数
            Func.Compile(pkg, cm, false);


            cm.Add(new Command(Opcode.Call, Args.Count, NeedBalanceDataStack)).SetCodePos(LParen);
        }
    }
}
