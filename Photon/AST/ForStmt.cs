
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    public class ForStmt : Stmt
    {
        public Stmt Init;

        public Expr Condition;

        public Stmt Post;

        public BlockStmt Body;

        public TokenPos ForPos;

        public ForStmt(Stmt init, Expr con, Stmt post, BlockStmt body, TokenPos forpos)
        {
            Condition = con;
            Body = body;
            Init = init;
            Post = post;

            BuildRelation();
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

        internal override void Compile(Package exe, CommandSet cm, bool lhs)
        {
            Init.Compile(exe, cm, false);            

            var loopStart = cm.CurrCmdID;

            Condition.Compile(exe, cm, false);           

            var jzCmd = cm.Add(new Command(Opcode.JZ, 0))
                .SetCodePos(ForPos);

            Body.Compile(exe, cm, false);

            Post.Compile(exe, cm, false);

            cm.Add(new Command(Opcode.JMP, loopStart))
                .SetCodePos(ForPos);

            // false body跳入
            jzCmd.DataA = cm.CurrCmdID;
        }

    }
}
