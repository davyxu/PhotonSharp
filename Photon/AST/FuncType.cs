
using SharpLexer;
using System.Collections.Generic;
using System.Text;

namespace Photon
{
    public class FuncType
    {
        public TokenPos FuncPos;

        public List<Ident> Params;

        public Scope ScopeInfo;

        public FuncType( TokenPos funcPos, List<Ident> param, Scope s )
        {
            FuncPos = funcPos;
            Params = param;
            ScopeInfo = s;
        }

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
