using System.Collections.Generic;
using SharpLexer;

namespace Photon
{

    internal class NewExpr : Expr
    {
        public Ident ClassName;

        public Ident PackageName;

        public TokenPos NewPos;

        Command _cmd;

        public NewExpr(Ident className, Ident pkgName, TokenPos newpos )
        {
            NewPos = newpos;
            ClassName = className;
            PackageName = pkgName;
            
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

        void ResolveClassName(CompileParameter param, int pass)
        {
            ObjectName on = ObjectName.Empty;
            on.EntryName = ClassName.Name;
            if (PackageName != null)
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
                on.PackageName = "Builtin";
                c = param.Exe.GetClassTypeByName(on);
            }

            

            if ( pass == 1 )
            {
                param.NextPassToResolve(this);
                return;
            }
            else if (c == null)
            {
                throw new CompileException("class entry not found: " + ClassName.Name, NewPos);
            }

            _cmd.DataA = c.ID;

            _cmd.SetComment(on.ToString());
        }

        internal override void Resolve(CompileParameter param)
        {
            ResolveClassName(param, 2);
        }

        internal override void Compile(CompileParameter param)
        {
            ResolveClassName(param, 1);

            _cmd = param.CS.Add(new Command(Opcode.NEW, -1))                
                .SetCodePos(NewPos);
        }
    }
}
