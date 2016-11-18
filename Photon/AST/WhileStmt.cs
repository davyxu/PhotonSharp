
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

        internal override void Compile(Package exe, CommandSet cm, bool lhs)
        {
            var loopStart = cm.CurrCmdID;

            Condition.Compile(exe, cm, false);

            var jzCmd = cm.Add(new Command(Opcode.JZ, 0))
                .SetCodePos(WhilePos);

            Body.Compile(exe, cm, false);            

            cm.Add(new Command(Opcode.JMP, loopStart))
                .SetCodePos(WhilePos);

            // false body跳入
            jzCmd.DataA = cm.CurrCmdID;
        }

    }
}
