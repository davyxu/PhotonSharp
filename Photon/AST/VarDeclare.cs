using Photon.OpCode;
using System.Collections.Generic;
using System.Text;

namespace Photon.AST
{

    public class VarDeclare : Stmt
    {
        public List<Ident> Names;
        public List<Expr> Values;

        public VarDeclare(List<Ident> names, List<Expr> values )
        {
            Names = names;
            Values = values;
        }

        public override IEnumerable<Node> Child()
        {
            foreach (var e in Names)
            {
                yield return e;
            }


            foreach (var e in Values)
            {
                yield return e;
            }
        }

        public override string ToString()
        {
            return "VarDeclare";
        }

        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            if (Values.Count > 0)
            {
                foreach (var e in Values)
                {
                    e.Compile(exe, cm, false);                    
                }

                foreach (var e in Names)
                {
                    e.Compile(exe, cm, true);                    
                }

            }
        }
    }
}
