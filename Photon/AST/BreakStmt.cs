using SharpLexer;

namespace Photon
{
    internal class BreakStmt : Stmt
    {                
        public TokenPos Pos;            

        public BreakStmt(TokenPos pos)
        {
            Pos = pos;
        }

        public override string ToString()
        {
            return string.Format("BreakStmt {0}", Pos);
        }

        LoopStmt nearestLoopStmt;

        Command cmd;

        internal override void Resolve(CompileParameter param)
        {
            cmd.DataA = nearestLoopStmt.LoopEndCmdID;
        }

        internal override void Compile(CompileParameter param)
        {
            nearestLoopStmt = LoopStmt.FindLoop(this);
            if ( nearestLoopStmt == null )
            {
                throw new CompileException("'break' should in loop statement", Pos);
            }

            param.NextPassToResolve(this);

            cmd = param.CS.Add(new Command(Opcode.JMP, -1))
               .SetCodePos(Pos).SetComment("break");
        }

      
    }
}
