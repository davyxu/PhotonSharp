
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    public class IfStmt : Stmt
    {
        public Expr Condition;

        public BlockStmt Body;

        public BlockStmt ElseBody;

        public TokenPos IfPos;

        public IfStmt(Expr con, BlockStmt body, BlockStmt elsebody, TokenPos ifpos)
        {
            Condition = con;
            Body = body;
            ElseBody = elsebody;
            IfPos = ifpos;

            BuildRelation();
        }


        public override string ToString()
        {
            return "IfStmt";
        }

        public override IEnumerable<Node> Child()
        {
            yield return Condition;
            
            yield return Body;

            yield return ElseBody;
        }

        internal override void Compile(Package exe, CommandSet cm, bool lhs)
        {
            Condition.Compile(exe, cm, false);            

            var jnzCmd = cm.Add(new Command(Opcode.Jz, 0))
                .SetCodePos(IfPos);

            Body.Compile(exe, cm, false);            

            var jmpCmd = cm.Add(new Command(Opcode.Jmp, 0))
                .SetCodePos(IfPos);

            // false body跳入
            jnzCmd.DataA = cm.CurrGenIndex;

            if (ElseBody.Stmts.Count > 0)
            {
                ElseBody.Compile(exe, cm, false);                
            }

            // true body执行完毕跳出
            jmpCmd.DataA = cm.CurrGenIndex;
        }

    }
}
