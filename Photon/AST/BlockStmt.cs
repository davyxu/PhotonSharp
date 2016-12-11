using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    internal class BlockStmt : Stmt
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

        internal override void Compile(CompileParameter param)
        {
            int index = 0;
            bool preHasNonImportStmt = false;

            foreach (var b in Stmts)
            {

                if ( b is ImportStmt )
                {
                    if ( preHasNonImportStmt )
                    {
                        throw new CompileException("'import' should at beginning of file", LBracePos);
                    }
                }
                else
                {
                    preHasNonImportStmt = true;
                }


                b.Compile(param.SetLHS(false));
                index++;
            }
        }
    }
}
