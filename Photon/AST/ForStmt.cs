using Photon.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.AST
{
    public class ForStmt : Stmt
    {
        public Expr Condition;

        public BlockStmt Body;


        public Stmt Init;
        public Stmt Post;

        public ForStmt(Stmt init, Expr con, Stmt post, BlockStmt body)
        {
            Condition = con;
            Body = body;
            Init = init;
            Post = post;
        }


        public override string ToString()
        {
            return "ForStmt";
        }

        public override IEnumerable<Node> Child()
        {
            if (Init != null)
            {
                yield return Init;
            }

            yield return Condition;

            if (Post != null)
            {
                yield return Post;
            }

            yield return Body;
        }

        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            Init.Compile(exe, cm, false);            

            var loopStart = cm.CurrGenIndex;

            Condition.Compile(exe, cm, false);           

            var jnzCmd = cm.Add(new Command(Opcode.Jnz, 0));

            Body.Compile(exe, cm, false);

            Post.Compile(exe, cm, false);

            cm.Add(new Command(Opcode.Jmp, loopStart));

            // false body跳入
            jnzCmd.DataA = cm.CurrGenIndex;
        }

    }
}
