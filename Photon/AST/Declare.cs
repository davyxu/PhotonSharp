using System.Collections.Generic;
using Photon.Scanner;
using System.Text;

namespace Photon.AST
{
    public class FuncDeclare : Stmt
    {
        public string Name;

        public List<Ident> Params;

        public BlockStmt Body;

        public FuncDeclare( string name, List<Ident> param, BlockStmt body )
        {
            Name = name;
            Params = param;
            Body = body;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            int index = 0;
            foreach( var i in Params )
            {
                if ( index > 0 )
                {
                    sb.Append(", ");
                }

                sb.Append(i.Name.ToString());
                

                index++;
            }

            return string.Format("FuncDeclare {0}: {1}",Name, sb.ToString());
        }

        public override IEnumerable<Node> Child()
        {
            return Body.Child();
        }

    }

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
    }
}
