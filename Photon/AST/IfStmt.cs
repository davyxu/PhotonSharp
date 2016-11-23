
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    internal class IfStmt : Stmt
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

        internal override void Compile(CompileParameter param)
        {
            Condition.Compile(param.SetLHS(false));

            var jnzCmd = param.CS.Add(new Command(Opcode.JZ, 0))
                .SetCodePos(IfPos);

            Body.Compile(param.SetLHS(false));

            var jmpCmd = param.CS.Add(new Command(Opcode.JMP, 0))
                .SetCodePos(IfPos);

            // false body跳入
            jnzCmd.DataA = param.CS.CurrCmdID;

            if (ElseBody.Stmts.Count > 0)
            {
                ElseBody.Compile(param.SetLHS(false));                
            }

            // true body执行完毕跳出
            jmpCmd.DataA = param.CS.CurrCmdID;
        }

    }
}
