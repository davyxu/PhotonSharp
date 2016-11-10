using Photon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.AST
{
    public class IfStmt : Stmt
    {
        public Expr Condition;

        public BlockStmt Body;

        public BlockStmt ElseBody;

        public IfStmt(Expr con, BlockStmt body, BlockStmt elsebody)
        {
            Condition = con;
            Body = body;
            ElseBody = elsebody;

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

        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            Condition.Compile(exe, cm, false);            

            var jnzCmd = cm.Add(new Command(Opcode.Jz, 0));

            Body.Compile(exe, cm, false);            

            var jmpCmd = cm.Add(new Command(Opcode.Jmp, 0));

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
