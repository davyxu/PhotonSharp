using Photon.Model;
using System.Collections.Generic;

namespace Photon.AST
{
    public class FuncLit : Expr
    {
        public FuncType TypeInfo;

        public BlockStmt Body;

        public FuncLit(BlockStmt body, FuncType ft )
        {
            TypeInfo = ft;
            Body = body;

            BuildRelation();
        }


        public override string ToString()
        {
            return string.Format("FuncLit {0}", TypeInfo.ToString());
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

            var c = new ValueFunc(funcIndex, TypeInfo.FuncPos);
            var ci = exe.Constants.Add(c);

            cm.Add(new Command(Opcode.Closure, ci)).Comment = c.ToString();            

            Body.Compile(exe, newset, false);

            var upvalues = new List<Ident>();
            FindUsedUpvalue(Body, upvalues );

            for (int upIndex = 0; upIndex < upvalues.Count; upIndex++ )
            {
                var id = upvalues[upIndex];

                cm.Add(new Command(Opcode.LinkU, upIndex, id.ScopeInfo.RegIndex)).Comment = id.ToString();
            }


            newset.InputValueCount = TypeInfo.Params.Count;

            newset.OutputValueCount = FuncType.CalcReturnValueCount( Body.Child() );
            
        }

        void FindUsedUpvalue( Node n, List<Ident> upvalues )
        {
            var usedVar = n as Ident;
            if (usedVar != null && usedVar.UpValue)
            {
                upvalues.Add(usedVar);
            }

            foreach( var c in n.Child() )
            {
                FindUsedUpvalue(c, upvalues);
            }
        }


    }
}
