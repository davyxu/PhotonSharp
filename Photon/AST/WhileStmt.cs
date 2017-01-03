
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    internal class WhileStmt : LoopStmt
    {
        public Expr Condition;   

        public WhileStmt(Expr con, TokenPos defpos, Scope s, BlockStmt body)
        {
            Condition = con;
            Pos = defpos;
            ScopeInfo = s;
            Body = body;

            BuildRelation();
        }

        public override string ToString()
        {
            return "WhileStmt";
        }

        public override IEnumerable<Node> Child()
        {
            yield return Condition;

            yield return Body;
        }

        internal override void Compile(CompileParameter param)
        {
            LoopBeginCmdID = param.CS.CurrCmdID;
            
            Condition.Compile(param.SetLHS(false));

            var jzCmd = param.CS.Add(new Command(Opcode.JZ, 0))
                .SetCodePos(Pos)
                .SetComment("while condition");

            param.LHS = false;
            Body.Compile(param.SetLHS(false));

            param.CS.Add(new Command(Opcode.JMP, LoopBeginCmdID))
                .SetCodePos(Pos)
                .SetComment("while loop");                

            // false body跳入
            LoopEndCmdID = param.CS.CurrCmdID;
            jzCmd.DataA = LoopEndCmdID;
        }

    }
}
