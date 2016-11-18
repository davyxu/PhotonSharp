
using System.Collections.Generic;
using System.Text;

namespace Photon
{
    public class FuncDeclare : Stmt
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


        internal override void Compile(Package pkg, CommandSet cm, bool lhs)
        {
            var newset = new CommandSet(Name.Name, TypeInfo.FuncPos, TypeInfo.ScopeInfo.CalcUsedReg(), false);

            var cmdSetID = pkg.AddProcedure(newset);

            //var c = new ValueFunc(pkg.ID, cmdSetID, TypeInfo.FuncPos);
            //var ci = pkg.Constants.Add(c);

            //cm.Add(new Command(Opcode.LOADC, ci))
            //    .SetComment(c.ToString())
            //    .SetCodePos(TypeInfo.FuncPos);

            //cm.Add(new Command(Opcode.SETR, Name.ScopeInfo.RegIndex))
            //    .SetComment(Name.Name)
            //    .SetCodePos(TypeInfo.FuncPos);

            Body.Compile(pkg, newset, false);

            newset.InputValueCount = TypeInfo.Params.Count;

            newset.OutputValueCount = FuncType.CalcReturnValueCount(Body.Child());
            
        }

     

    }
}
