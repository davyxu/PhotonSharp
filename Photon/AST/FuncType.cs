
using SharpLexer;
using System.Collections.Generic;
using System.Text;

namespace Photon
{
    internal class FuncType
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

        internal static int CalcReturnValueCount(IEnumerable<Node> nodeEnum)
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


        internal void GenDefaultRet(IEnumerable<Node> nodeEnum, CompileParameter param)
        {
            bool anyCode = false;
            foreach (var c in nodeEnum)
            {
                anyCode = true;

                var retStmt = c as ReturnStmt;
                if (retStmt != null)
                {                    
                    return;
                }                
            }
           
            var cmd = param.CS.Add(new Command(Opcode.RET));

            // 有任意代码, 且有指令, 用最后的代码位置生成RET的代码位置
            if (anyCode && param.CS.CurrCmdID > 1)
            {
                cmd.SetCodePos(param.CS.Commands[param.CS.CurrCmdID - 2].CodePos);
            }
            else
            {
                // 使用函数定义地方做exit位置
                cmd.SetCodePos(FuncPos);
            }
        }

    }
}
