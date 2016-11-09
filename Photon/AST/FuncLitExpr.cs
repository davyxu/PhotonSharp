using Photon.Model;
using System.Collections.Generic;
using System.Text;

namespace Photon.AST
{
    public class FuncLitExpr : Expr
    {
        public FuncType TypeInfo = new FuncType();

        public BlockStmt Body;

        public FuncLitExpr(List<Ident> param, BlockStmt body, Scope s)
        {
            TypeInfo.Params = param;
            TypeInfo.ScopeInfo = s;
            Body = body;

            BuildRelation();
        }


        public override string ToString()
        {            
            return string.Format("FuncLitExpr {0}", TypeInfo.ToString());
        }

        public override IEnumerable<Node> Child()
        {
            return Body.Child();
        }


        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            // TODO 还要加上 捕获的变量数量
            var newset = new CommandSet("closure", TypeInfo.ScopeInfo.SymbolCount, false );

            var funcIndex = exe.AddCmdSet(newset);

            var c = new ValueFunc(funcIndex);
            var ci = exe.Constants.Add(c);

            cm.Add(new Command(Opcode.LoadC, ci)).Comment = c.ToString();

            //cm.Add(new Command(Opcode.SetG, Name.ScopeInfo.RegIndex)).Comment = Name.Name;

            Body.Compile(exe, newset, false);

            newset.InputValueCount = TypeInfo.Params.Count;

            newset.OutputValueCount = FuncType.CalcReturnValueCount( Body.Child() );
            
        }


    }
}
