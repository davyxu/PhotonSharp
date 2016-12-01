using System.Collections.Generic;
using SharpLexer;

namespace Photon
{

    internal class NewExpr : Expr
    {
        public Ident ClassName;

        public Ident PackageName;

        public TokenPos NewPos;

        CallExpr _call;


        public NewExpr(CallExpr callexpr, TokenPos newpos )
        {
            _call = callexpr;

            var localFuncName = callexpr.Func as Ident;
            if (localFuncName != null )
            {
                ClassName = localFuncName;
            }

            var pkgFuncName = callexpr.Func as SelectorExpr;
            if ( pkgFuncName != null )
            {
                var pkgName = pkgFuncName.X as Ident;
                if (pkgName == null)
                {
                    throw new CompileException("invalid class package name", NewPos);
                }

                PackageName = pkgName;
                ClassName = pkgFuncName.Selector;
            }


            NewPos = newpos;
            
            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            yield return ClassName;

            if (PackageName != null)
            {
                yield return PackageName;
            }
        }

        public override string ToString()
        {
            return "NewExpr";
        }

        int NeedBalanceDataStack
        {
            get
            {
                // 单独的一句时, 需要平衡数据栈
                if (Parent is ExprStmt)
                    return 1;

                return 0;
            }
        }


        internal override void Compile(CompileParameter param)
        {


            ObjectName on = ObjectName.Empty;
            on.EntryName = ClassName.Name;
            if ( PackageName != null )
            {
                on.PackageName = PackageName.Name;   
            }
            else
            {
                on.PackageName = param.Pkg.Name;
            }

            var c = param.Exe.GetClassTypeByName(on);
            if ( c == null )
            {
                throw new CompileException("class entry not found", NewPos);
            }
            
            // 类名
            param.CS.Add(new Command(Opcode.LOADC, c.ID))
                .SetComment(on.ToString())
                .SetCodePos(NewPos);

            param.CS.Add(new Command(Opcode.NEW, _call.Args.Count, NeedBalanceDataStack))
                .SetComment(on.ToString())
                .SetCodePos(NewPos);



            // 先放参数
            foreach (var arg in _call.Args)
            {
                arg.Compile(param.SetLHS(false));
            }

            if ( c.Ctor != null )
            {
                param.CS.Add(new Command(Opcode.LOADF, c.Ctor.ID)).SetComment(c.Ctor.Name.EntryName).SetCodePos(NewPos);
                param.CS.Add(new Command(Opcode.CALL, _call.Args.Count + 1, NeedBalanceDataStack)).SetCodePos(NewPos);
            }
        }
    }
}
