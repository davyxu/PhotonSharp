
using Photon.OpCode;
namespace Photon.AST
{
    public class Ident : Expr
    {
        public string Name;

        public ScopeMeta ScopeInfo;

        public Ident(string n)
        {
            Name = n;
        }

        public override string ToString()
        {
            if (ScopeInfo != null)
            {
                return string.Format("{0} @ {1}", Name, ScopeInfo.Slot);
            }

            return Name;

        }

        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            var scopeIndex = ScopeInfo.Parent.Index;

            if (lhs)
            {
                cm.Add(new Command(Opcode.SetR, ScopeInfo.Slot, scopeIndex)).Comment = Name;
            }
            else
            {
                cm.Add(new Command(Opcode.LoadR, ScopeInfo.Slot, scopeIndex)).Comment = Name;
            }
        }
    }
}
