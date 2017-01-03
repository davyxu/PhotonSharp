using SharpLexer;

namespace Photon
{
    class LoopTypes
    {
        public TokenPos Pos;

        public Scope ScopeInfo;

        public BlockStmt Body;

        internal int BeginCmdID = -1;

        internal int EndCmdID = -1;

        internal LoopTypes(TokenPos pos, Scope s, BlockStmt body)
        {
            Pos = pos;
            ScopeInfo = s;
            Body = body;
        }
    }
}
