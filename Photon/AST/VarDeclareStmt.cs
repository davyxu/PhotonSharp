
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    // var XXX  纯变量定义
    internal class VarDeclareStmt : Stmt
    {
        public List<Ident> Names;

        public TokenPos VarPos;

        public VarDeclareStmt(List<Ident> names, TokenPos varpos )
        {
            Names = names;
            VarPos = varpos;

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            foreach (var e in Names)
            {
                yield return e;
            }
        }

        public override string ToString()
        {
            return "VarDeclareStmt";
        }

        internal override void Compile(CompileParameter param)
        {
         
        }
    }
}
