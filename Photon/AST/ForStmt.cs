
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    internal class ForStmt : Stmt
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
            ForPos = forpos;

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

        internal override void Compile(CompileParameter param)
        {
            Init.Compile(param.SetLHS(false));

            var loopStart = param.CS.CurrCmdID;

            Condition.Compile(param.SetLHS(false));

            var jzCmd = param.CS.Add(new Command(Opcode.JZ, 0))
                .SetCodePos(ForPos);

            Body.Compile(param.SetLHS(false));

            Post.Compile(param.SetLHS(false));

            param.CS.Add(new Command(Opcode.JMP, loopStart))
                .SetCodePos(ForPos);

            // false body跳入
            jzCmd.DataA = param.CS.CurrCmdID;
        }

    }
}
