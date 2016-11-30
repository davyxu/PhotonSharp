
using System.Collections.Generic;
using System.Text;

namespace Photon
{
    internal class FuncDeclare : Stmt
    {
        public Ident Name;

        public FuncType TypeInfo;
      
        public BlockStmt Body;

        public Ident ClassName;

        Procedure _proc;

        public FuncDeclare(Ident name, BlockStmt body, FuncType ft )
        {
            Name = name;
            Body = body;

            TypeInfo = ft;

            BuildRelation();
        }


        public override string ToString()
        {
            return string.Format("FuncDeclare {0} {1}", Name.Name, TypeInfo.ToString());
        }

        public override IEnumerable<Node> Child()
        {
            return Body.Child();
        }

        internal override void Resolve(CompileParameter param)
        {
            ResolveClassName(param, 2);
        }

        bool ResolveClassName(CompileParameter param, int pass)
        {
            if (ClassName != null)
            {
                var c = param.Exe.GetClassTypeByName( new ObjectName( param.Pkg.Name, ClassName.Name) );
                if (c == null )
                {
                    if (pass == 1)
                    {
                        param.NextPassToResolve(this);
                    }
                    else
                    {
                        throw new CompileException("unknown class name: " + ClassName.Name, TypeInfo.FuncPos);
                    }
                }
                else
                {
                    int nameKey = param.Pkg.Constants.Add(new ValueString(Name.Name));
                    c.AddMethod(nameKey, _proc);

                    return true;
                }
            }

            return false;
        }


        internal override void Compile(CompileParameter param)
        {
            var newset = new CommandSet(new ObjectName(param.Pkg.Name, Name.Name), TypeInfo.FuncPos, TypeInfo.ScopeInfo.CalcUsedReg(), false);

            _proc = param.Exe.AddProcedure(newset);

            ResolveClassName(param,  1);

            var funcParam = param.SetLHS(false).SetComdSet(newset);
            Body.Compile(funcParam);

            TypeInfo.GenDefaultRet(Body.Child(), funcParam);

            newset.InputValueCount = TypeInfo.Params.Count;

            newset.OutputValueCount = FuncType.CalcReturnValueCount(Body.Child());


            
        }

     

    }
}
