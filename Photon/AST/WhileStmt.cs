using Photon.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.AST
{
    public class WhileStmt : Stmt
    {
        public Expr Condition;

        public BlockStmt Body;

        public WhileStmt(Expr con, BlockStmt body)
        {
            Condition = con;
            Body = body;
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

            var jnzCmd = cm.Add(new Command(Opcode.Jnz, 0));

            Body.Compile(exe, cm, false);            

            cm.Add(new Command(Opcode.Jmp, loopStart));

            // false body跳入
            jnzCmd.DataA = cm.CurrGenIndex;
        }

    }
}
