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
            if (!ResolveFuncEntry(_belongPackage, _funcName, _callCmdToResolve))
            {
                throw new ParseException("unsolved function entry", LParen);
            }            
        }
        static bool ResolveFuncEntry( Package pkg, string name, Command cmd )
        {            

            // 优先在本包找
            var cs = pkg.FindProcedureByName(name) as CommandSet;
            if (cs != null)
            {
                cmd.DataA = pkg.ID;
                cmd.DataB = cs.ID;

                return true;
            }

            Procedure outP;
            Package outPkg;

            if (pkg.Exe.FindCmdSetInPackage(name, out outP, out outPkg))
            {
                var outCS = outP as CommandSet;

                cmd.DataA = outPkg.ID;
                cmd.DataB = outCS.ID;

                return true;
            }

            return false;
        }

        Command _callCmdToResolve;
        string _funcName;
        Package _belongPackage;


        static Symbol FindFuncNameSymbol( Scope s, string name )
        {
            while( s != null)
            {
                var symbol = s.FindSymbol(name);
                if (symbol != null)
                {
                    return symbol;
                }

                s = s.Outter;
            }

            return null;
            
        }

        void AnalyseFuncEntry(Package pkg, CommandSet cm)
        {            
            // 名字访问, 可能是本地函数调用, 或动态变量访问
            var funcNameToken = Func as Ident;
            if ( funcNameToken != null )
            {
                var funcNameSymbol = FindFuncNameSymbol(S, funcNameToken.Name);
                if ( funcNameSymbol == null )
                {
                    throw new ParseException("func name not found:" + funcNameToken.Name, LParen);
                }

                switch (funcNameSymbol.Usage )
                {
                    case SymbolUsage.Delegate:
                    case SymbolUsage.Func:
                        {
                            // 需要等下次pass时,才能确认symbol存在
                            _belongPackage = pkg;
                            _funcName = funcNameSymbol.Name;
                            _callCmdToResolve=cm.Add(new Command(Opcode.LOADF,0,0 )).SetCodePos(LParen);

                            if (!ResolveFuncEntry(_belongPackage, _funcName, _callCmdToResolve))
                            {
                                // 不是本package, 或者函数在本package后面定义, 延迟到下次pass进行解析
                                pkg.Exe._secondPassNode.Add(this);
                            }

                            
                        }
                        break;
                    case SymbolUsage.Variable:
                    case SymbolUsage.Parameter:
                        {
                            // 动态调用, 放入变量
                            Func.Compile(pkg, cm, false);
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
            //Func.Compile(pkg, cm, false);


            cm.Add(new Command(Opcode.CALLF, Args.Count, NeedBalanceDataStack)).SetCodePos(LParen);
        }
    }
}
