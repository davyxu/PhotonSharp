
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{

    internal class ClassDeclare : Stmt
    {
        public Ident Name;

        public TokenPos ClassPos;

        public Scope ScopeInfo;

        public List<Ident> Member;

        public ClassDeclare(Ident name, Scope s, List<Ident> member, TokenPos classpos)
        {
            Name = name;
            ClassPos = classpos;
            Member = member;
            ScopeInfo = s;

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            foreach (var s in Member)
            {
                yield return s;
            }
        }

        public override string ToString()
        {
            return "ClassStmt";
        }

        internal override void Compile(CompileParameter param)
        {
            var c = new ClassType(param.Exe, new ObjectName(param.Pkg.Name, Name.Name ));

            foreach( var m in Member )
            {
                var ki = param.Pkg.Constants.Add( new ValueString(m.Name));

                c.AddMemeber(ki, m.Name);
            }

            param.Exe.AddClassType(c);
        }
    }
}
