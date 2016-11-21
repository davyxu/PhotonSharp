
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{

    public class ImportStmt : Stmt
    {
        public List<BasicLit> Sources = new List<BasicLit>();
        public TokenPos ImportPos;

        public ImportStmt(List<BasicLit> list, TokenPos pos)
        {
            Sources = list;
            ImportPos = pos;

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            foreach (var s in Sources)
            {
                yield return s;
            }
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
