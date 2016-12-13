
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    class ImportStmt : Stmt
    {
        public BasicLit Source;
        public TokenPos ImportPos;

        public ImportStmt(BasicLit src, TokenPos pos)
        {
            Source = src;
            ImportPos = pos;

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            yield return Source;
        }

        public override string ToString()
        {
            return "ImportStmt";
        }

        internal override void Compile(CompileParameter param)
        {

        }
    }
}
