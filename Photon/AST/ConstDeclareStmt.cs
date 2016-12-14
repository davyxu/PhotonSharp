
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    // var XXX  纯变量定义
    internal class ConstDeclareStmt : Stmt
    {
        public Ident Name;

        public Expr Value;

        public TokenPos ConstPos;

        public ConstDeclareStmt(Ident name, Expr value, TokenPos defpos)
        {
            Name = name;
            ConstPos = defpos;
            Value = value;

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            yield return Name;

            yield return Value;
        }

        public override string ToString()
        {
            return "ConstDeclareStmt";
        }

        internal override void Compile(CompileParameter param)
        {
            var newset = new ValuePhoFunc(new ObjectName("fakepkg", "constcalc"), ConstPos, 0, null);
            

            var funcParam = param.SetCmdSet(newset);            

            // 用空的常量表
            funcParam.Constants = new ConstantSet();

            // 生成表达式计算指令
            Value.Compile(funcParam);

            try
            {
                var eval = VMachine.MiniExec(newset, funcParam.Constants);

                var ci = param.Constants.Add(eval);
                Name.Symbol.RegIndex = ci;
            }
            catch(RuntimeException)
            {
                throw new CompileException("Expect constant value to caculate value", ConstPos);
            }

           

        }
    }
}
