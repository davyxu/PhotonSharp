using Photon.Model;
using System.Collections.Generic;
using System.Text;

namespace Photon.AST
{
    public class FuncType
    {
        public List<Ident> Params;

        public Scope ScopeInfo;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            int index = 0;
            foreach (var i in Params)
            {
                if (index > 0)
                {
                    sb.Append(", ");
                }

                sb.Append(i.Name.ToString());


                index++;
            }

            return sb.ToString();
        }


        public static int CalcReturnValueCount(IEnumerable<Node> nodeEnum)
        {
            foreach (var c in nodeEnum)
            {
                var retStmt = c as ReturnStmt;
                if (retStmt == null)
                {
                    continue;
                }

                return retStmt.Results.Count;
            }

            return 0;
        }

    }
}
