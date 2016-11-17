using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    public class BlockStmt : Stmt
    {
        public List<Stmt> Stmts = new List<Stmt>();
        public TokenPos LBracePos;
        public TokenPos RBracePos;

        public BlockStmt(TokenPos lpos, TokenPos rpos)
        {

        }

        public BlockStmt(List<Stmt> list, TokenPos lpos, TokenPos rpos)
        {
            Stmts = list;
            LBracePos = lpos;
            RBracePos = rpos;

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            foreach (var s in Stmts)
            {
                yield return s;
            }
        }

        public override string ToString()
        {
            return "BlockStmt";
        }

        internal override void Compile(Package exe, CommandSet cm, bool lhs)
        {

            foreach (var b in Stmts)
            {
                b.Compile(exe, cm, false);
            }
        }
    }
}
