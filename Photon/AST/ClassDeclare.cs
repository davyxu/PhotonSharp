
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{

    internal class ClassDeclare : Stmt
    {
        public TokenPos ClassPos;

        public Ident Name;

        public TokenPos ColonPos;

        public Ident ParentName;

        public Scope ScopeInfo;

        public List<Ident> Member;

        ValuePhoClassType _class;

        public ClassDeclare(Ident name, Ident parentName, Scope s, List<Ident> member, TokenPos classpos)
        {
            Name = name;
            ParentName = parentName;
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

        bool ResolveParent(CompileParameter param, int pass )
        {            
            var parent = param.Exe.GetClassTypeByName( new ObjectName(param.Pkg.Name, ParentName.Name)) as ValuePhoClassType;
            if (parent != null)
            {
                _class.ParentID = parent.ID;
                _class.Parent = param.Exe.FindClassByPersistantID(parent.ID);      

                return true;
            }

            if ( pass > 1 )
            {
                throw new CompileException("unknown parent class: " + ParentName.Name, ColonPos);
            }

            return false;
        }

        internal override void Resolve(CompileParameter param)
        {
            ResolveParent(param, 2);
        }
        

        internal override void Compile(CompileParameter param)
        {
            _class = new ValuePhoClassType(param.Pkg, new ObjectName(param.Pkg.Name, Name.Name));
            
            _class.ID = param.Exe.GenPersistantID();

            foreach( var m in Member )
            {
                var ki = param.Constants.AddString(m.Name);

                _class.AddMemeber(ki, m.Name);
            }

            param.Exe.AddClassType(_class);


            if (ParentName != null && !ResolveParent(param, 1))
            {
                param.NextPassToResolve(this);
            }
        }
    }
}
