using SharpLexer;

namespace Photon
{
    class LoopStmt : Stmt
    {
        public TokenPos Pos;

        public Scope ScopeInfo; // for 有独立的作用域, 在body block的外层

        public BlockStmt Body;

        internal int LoopBeginCmdID = -1;

        internal int LoopEndCmdID = -1;

        internal static LoopStmt FindLoop(Node start)
        {
            Node n = start;
            while (n != null)
            {
                if (n is LoopStmt)
                    return n as LoopStmt;

                n = n.Parent;
            }

            return null;
        }
    }
}
