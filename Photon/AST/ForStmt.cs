
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    internal class ForStmt : LoopStmt
    {
        public Stmt Init;

        public Expr Condition;

        public Stmt Post;

        public ForStmt(Stmt init, Expr con, Stmt post, TokenPos defpos, Scope s, BlockStmt body)
        {
            Condition = con;            
            Init = init;
            Post = post;
            Pos = defpos;
            ScopeInfo = s;
            Body = body;

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

            if (Condition != null)
            {
                yield return Condition;
            }

            if (Post != null)
            {
                yield return Post;
            }

            yield return Body;
        }        

        internal override void Compile(CompileParameter param)
        {
            if (Init != null)
            {
                Init.Compile(param.SetLHS(false));
            }

            LoopBeginCmdID = param.CS.CurrCmdID;

            Command jzCmd = null;

            if (Condition != null)
            {
                Condition.Compile(param.SetLHS(false));

                jzCmd = param.CS.Add(new Command(Opcode.JZ, -1))
                .SetCodePos(Pos)
                .SetComment("for condition");
            }

            Body.Compile(param.SetLHS(false));

            if (Post != null)
            {
                Post.Compile(param.SetLHS(false));
            }


            param.CS.Add(new Command(Opcode.JMP, LoopBeginCmdID))
                .SetCodePos(Pos)
                .SetComment("for loop");

            // false body跳入
            if (jzCmd != null)
            {
                jzCmd.DataA = param.CS.CurrCmdID;
            }

            LoopEndCmdID = param.CS.CurrCmdID;
        }

    }
}
