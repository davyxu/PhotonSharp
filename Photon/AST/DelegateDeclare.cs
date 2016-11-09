using Photon.Model;
using System.Collections.Generic;
using System.Text;

namespace Photon.AST
{
    public class DelegateDeclare : Stmt
    {
        public Ident Name;

        public List<Ident> Params;

        public Scope ScopeInfo;

        public DelegateDeclare(Ident name, List<Ident> param, Scope s)
        {
            Name = name;
            Params = param;
            ScopeInfo = s;

            BuildRelation();
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

            return string.Format("DelegateDeclare {0}: {1}", Name, sb.ToString());
        }

        public override IEnumerable<Node> Child()
        {
            yield break;
        }


        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            var c = new ValueDelegate( );
            var ci = exe.Constants.Add(c);

            exe.DelegateMap.Add(Name.Name, c);

            cm.Add(new Command(Opcode.LoadC, ci)).Comment = c.ToString();

            cm.Add(new Command(Opcode.SetR, Name.ScopeInfo.RegIndex )).Comment = Name.Name;
        }
    }
}
