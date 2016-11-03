
using Photon.OpCode;
namespace Photon.AST
{
    public class Ident : Expr
    {
        public string Name;

        public Symbol ScopeInfo;

        public Ident(string n)
        {
            Name = n;
        }

        public override string ToString()
        {
            if (ScopeInfo != null)
            {
                return string.Format("{0} R({1})", Name, ScopeInfo.RegIndex);
            }

            return Name;

        }

        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {            

            if (lhs)
            {
                cm.Add(new Command(Opcode.SetR, ScopeInfo.RegIndex )).Comment = Name;
            }
            else
            {
                cm.Add(new Command(Opcode.LoadR, ScopeInfo.RegIndex )).Comment = Name;
            }
        }
    }
}
