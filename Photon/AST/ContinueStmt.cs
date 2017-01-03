using SharpLexer;

namespace Photon
{
    internal class ContinueStmt : Stmt
    {                
        public TokenPos Pos;

        public ContinueStmt(TokenPos pos)
        {
            Pos = pos;
        }

        public override string ToString()
        {
            return string.Format("ContinueStmt {0}", Pos);
        }

        LoopStmt nearestForStmt;

        Command cmd;

        internal override void Resolve(CompileParameter param)
        {
            cmd.DataA = nearestForStmt.LoopBeginCmdID;
        }

        internal override void Compile(CompileParameter param)
        {
            nearestForStmt = LoopStmt.FindLoop(this);
            if ( nearestForStmt == null )
            {
                throw new CompileException("'continue' should in for statement", Pos);
            }

            param.NextPassToResolve(this);

            cmd = param.CS.Add(new Command(Opcode.JMP, -1))
               .SetCodePos(Pos).SetComment("continue");
        }

      
    }
}
