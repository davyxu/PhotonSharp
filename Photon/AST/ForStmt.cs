
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    internal class ForStmt : Stmt
    {
        public Stmt Init;

        public Expr Condition;

        public Stmt Post;

        public LoopTypes TypeInfo;

        public ForStmt(Stmt init, Expr con, Stmt post, LoopTypes types)
        {
            Condition = con;            
            Init = init;
            Post = post;
            TypeInfo = types;

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

            yield return TypeInfo.Body;
        }        

        internal override void Compile(CompileParameter param)
        {
            if (Init != null)
            {
                Init.Compile(param.SetLHS(false));
            }

            var loopStart = param.CS.CurrCmdID;

            Command jzCmd = null;

            if (Condition != null)
            {
                Condition.Compile(param.SetLHS(false));

                jzCmd = param.CS.Add(new Command(Opcode.JZ, -1))
                .SetCodePos(TypeInfo.Pos)
                .SetComment("for condition");
            }



            TypeInfo.Body.Compile(param.SetLHS(false));

            if (Post != null)
            {
                Post.Compile(param.SetLHS(false));
            }


            param.CS.Add(new Command(Opcode.JMP, loopStart))
                .SetCodePos(TypeInfo.Pos)
                .SetComment("for loop");

            // false body跳入
            if (jzCmd != null)
            {
                jzCmd.DataA = param.CS.CurrCmdID;
            }

            TypeInfo.EndCmdID = param.CS.CurrCmdID;
        }

    }
}
