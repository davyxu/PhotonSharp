
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

        ValueFunc _proc;

        internal ValuePhoFunc bodyCS;

        public FuncDeclare(Ident name, FuncType ft )
        {
            Name = name;

            TypeInfo = ft;
        }


        public override string ToString()
        {
            return string.Format("FuncDeclare '{0}' {1}", Name.Name, TypeInfo.ToString());
        }

        public override IEnumerable<Node> Child()
        {
            return Body.Child();
        }

        internal override void Resolve(CompileParameter param)
        {
            var c = param.Exe.GetClassTypeByName(new ObjectName(param.Pkg.Name, ClassName.Name));
            if (c == null)
            {
                throw new CompileException("unknown class name: " + ClassName.Name, TypeInfo.FuncPos);
            }
            else
            {
                int nameKey = param.Constants.AddString(Name.Name);
                var cc = c as ValuePhoClassType;
                cc.AddMethod(nameKey, _proc);
            } 
        }


        internal override void Compile(CompileParameter param)
        {
            ObjectName on = new ObjectName(param.Pkg.Name, Name.Name);

            if (ClassName != null)
            {
                // 成员函数必须有至少1个参数(self)
                if (TypeInfo.Params.Count < 1)
                {
                    throw new CompileException("Expect 'self' in method", TypeInfo.FuncPos);
                }

                on.ClassName = ClassName.Name;
            }

            var newset = new ValuePhoFunc(on, TypeInfo.FuncPos, TypeInfo.ScopeInfo.CalcUsedReg(), TypeInfo.ScopeInfo);
            bodyCS = newset;

            _proc = param.Exe.AddFunc(newset);

            if ( ClassName != null )
            {
                param.NextPassToResolve(this);
            }            

            var funcParam = param.SetLHS(false).SetCmdSet(newset);
            Body.Compile(funcParam);

            TypeInfo.GenDefaultRet(Body.Child(), funcParam);

            newset.InputValueCount = TypeInfo.Params.Count;

            newset.OutputValueCount = FuncType.CalcReturnValueCount(Body.Child());


            
        }

     

    }
}
