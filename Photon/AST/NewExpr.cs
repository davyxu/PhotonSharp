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


        internal override void Compile(CompileParameter param)
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

            _cmd = param.CS.Add(new Command(Opcode.NEW))                
                .SetCodePos(NewPos);

            _cmd.EntryName = on;

            _cmd.SetComment(on.ToString());
        }
    }
}
