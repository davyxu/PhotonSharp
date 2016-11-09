using Photon.Model;
using System.Collections.Generic;
using System.Text;

namespace Photon.AST
{
    public class FuncDeclare : Stmt
    {
        public Ident Name;

        public FuncType TypeInfo = new FuncType();
      
        public BlockStmt Body;

        public FuncDeclare(Ident name, List<Ident> param, BlockStmt body, Scope s)
        {
            Name = name;
            Body = body;

            TypeInfo.Params = param;
            TypeInfo.ScopeInfo = s;            

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
            var newset = new CommandSet(Name.Name, TypeInfo.ScopeInfo.SymbolCount, false );

            var funcIndex = exe.AddCmdSet(newset);

            var c = new ValueFunc(funcIndex);
            var ci = exe.Constants.Add(c);

            cm.Add(new Command(Opcode.LoadC, ci)).Comment = c.ToString();

            cm.Add(new Command(Opcode.SetR, Name.ScopeInfo.RegIndex )).Comment = Name.Name;

            Body.Compile(exe, newset, false);

            newset.InputValueCount = TypeInfo.Params.Count;

            newset.OutputValueCount = FuncType.CalcReturnValueCount(Body.Child());
            
        }

     

    }
}
