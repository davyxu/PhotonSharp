
using System.Collections.Generic;
using System.Text;

namespace Photon
{
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


        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            var c = new ValueDelegate( );
            var ci = exe.Constants.Add(c);

            exe.AddDelegate(Name.Name, c);

            cm.Add(new Command(Opcode.LoadC, ci))
                .SetComment(c.ToString())
                .SetCodePos(TypeInfo.FuncPos);

            cm.Add(new Command(Opcode.SetR, Name.ScopeInfo.RegIndex))
                .SetComment(Name.Name)
                .SetCodePos(TypeInfo.FuncPos);
        }
    }
}
