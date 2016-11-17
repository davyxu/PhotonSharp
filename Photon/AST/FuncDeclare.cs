
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


        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            var newset = new CommandSet(Name.Name, TypeInfo.FuncPos, TypeInfo.ScopeInfo.CalcUsedReg(), false);

            var funcIndex = exe.AddCmdSet(newset);

            var c = new ValueFunc(funcIndex, TypeInfo.FuncPos);
            var ci = exe.Constants.Add(c);

            cm.Add(new Command(Opcode.LoadC, ci))
                .SetComment(c.ToString())
                .SetCodePos(TypeInfo.FuncPos);

            cm.Add(new Command(Opcode.SetR, Name.ScopeInfo.RegIndex))
                .SetComment(Name.Name)
                .SetCodePos(TypeInfo.FuncPos);

            Body.Compile(exe, newset, false);

            newset.InputValueCount = TypeInfo.Params.Count;

            newset.OutputValueCount = FuncType.CalcReturnValueCount(Body.Child());
            
        }

     

    }
}
