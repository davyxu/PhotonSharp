
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

        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            var loopStart = cm.CurrGenIndex;

            Condition.Compile(exe, cm, false);

            var jzCmd = cm.Add(new Command(Opcode.Jz, 0))
                .SetCodePos(WhilePos);

            Body.Compile(exe, cm, false);            

            cm.Add(new Command(Opcode.Jmp, loopStart))
                .SetCodePos(WhilePos);

            // false body跳入
            jzCmd.DataA = cm.CurrGenIndex;
        }

    }
}
