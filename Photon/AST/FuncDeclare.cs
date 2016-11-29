﻿
using System.Collections.Generic;
using System.Text;

namespace Photon
{
    internal class FuncDeclare : Stmt
    {
        public Ident Name;

        public FuncType TypeInfo;
      
        public BlockStmt Body;

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


        internal override void Compile(CompileParameter param)
        {
            var newset = new CommandSet(new ProcedureName(param.Pkg.Name, Name.Name), TypeInfo.FuncPos, TypeInfo.ScopeInfo.CalcUsedReg(), false);

            param.Pkg.Exe.AddProcedure(newset);            

            Body.Compile(param.SetLHS(false).SetComdSet(newset));

            newset.InputValueCount = TypeInfo.Params.Count;

            newset.OutputValueCount = FuncType.CalcReturnValueCount(Body.Child());
            
        }

     

    }
}
