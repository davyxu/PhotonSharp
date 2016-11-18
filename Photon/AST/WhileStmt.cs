
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    public class WhileStmt : Stmt
    {
        public Expr Condition;

        public BlockStmt Body;

        public TokenPos WhilePos;        

        public WhileStmt(Expr con, BlockStmt body, TokenPos whilepos)
        {
            Condition = con;
            Body = body;
            WhilePos = whilepos;

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
            var loopStart = param.CS.CurrCmdID;
            
            Condition.Compile(param.SetLHS(false));

            var jzCmd = param.CS.Add(new Command(Opcode.JZ, 0))
                .SetCodePos(WhilePos);

            param.LHS = false;
            Body.Compile(param.SetLHS(false));

            param.CS.Add(new Command(Opcode.JMP, loopStart))
                .SetCodePos(WhilePos);

            // false body跳入
            jzCmd.DataA = param.CS.CurrCmdID;
        }

    }
}
