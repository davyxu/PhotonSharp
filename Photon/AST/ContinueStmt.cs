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
            return string.Format("BreakStmt {0}", Pos);
        }

        static ForStmt FindForStmt(Node start)
        {
            Node n = start;
            while( n != null )
            {
                if (n is ForStmt)
                    return n as ForStmt;

                n = n.Parent;
            }

            return null;
        }

        ForStmt nearestForStmt;

        Command cmd;

        internal override void Resolve(CompileParameter param)
        {
            cmd.DataA = nearestForStmt.TypeInfo.BeginCmdID;
        }

        internal override void Compile(CompileParameter param)
        {
            nearestForStmt = FindForStmt(this);
            if ( nearestForStmt == null )
            {
                throw new CompileException("'break' should in for statement", Pos);
            }

            param.NextPassToResolve(this);

            cmd = param.CS.Add(new Command(Opcode.JMP, -1))
               .SetCodePos(Pos).SetComment("break");
        }

      
    }
}
