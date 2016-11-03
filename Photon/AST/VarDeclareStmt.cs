using Photon.OpCode;
using System.Collections.Generic;
using System.Text;

namespace Photon.AST
{
    // var XXX  纯变量定义
    public class VarDeclareStmt : Stmt
    {
        public List<Ident> Names;        

        public VarDeclareStmt(List<Ident> names )
        {
            Names = names;            

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
    }
}
