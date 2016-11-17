using SharpLexer;

namespace Photon
{
    public class Symbol
    {
        public string Name;
        public TokenPos DefinePos;
        public Node Decl;
        public int RegIndex;

        public Scope Belong;

        public bool IsGlobal
        {
            get
            {
                return Belong.Type == ScopeType.Global;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} R{1}  {2}", Name, RegIndex, DefinePos);
        }
    }
}
