
using System.Collections.Generic;
using System.Text;

namespace Photon
{
    // 函数前置声明
    public class DelegateDeclare : Stmt
    {
        public Ident Name;

        public FuncType TypeInfo;

        public DelegateDeclare(Ident name, FuncType ft )
        {
            Name = name;
            TypeInfo = ft;

            BuildRelation();
        }

        public override string ToString()
        {
            return string.Format("DelegateDeclare {0} {1}", Name.Name, TypeInfo.ToString());
        }

        public override IEnumerable<Node> Child()
        {
            yield break;
        }


        internal override void Compile(Package pkg, CommandSet cm, bool lhs)
        {
            var c = new ValueDelegate( );
            var ci = pkg.Constants.Add(c);

            pkg.Exe.AddDelegate(Name.Name, c);

            //cm.Add(new Command(Opcode.LOADC, ci))
            //    .SetComment(c.ToString())
            //    .SetCodePos(TypeInfo.FuncPos);

            //cm.Add(new Command(Opcode.SETR, Name.ScopeInfo.RegIndex))
            //    .SetComment(Name.Name)
            //    .SetCodePos(TypeInfo.FuncPos);
        }
    }
}
