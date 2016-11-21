using SharpLexer;

namespace Photon
{
    public enum SymbolUsage
    {
        None,
        Delegate,
        Func,
        Variable,
        Parameter,
    }


    public class Symbol
    {
        public string Name;
        public TokenPos DefinePos;
        public Node Decl;
        public int RegIndex;

        public Scope RegBelong;

        public SymbolUsage Usage;

        public bool IsGlobal
        {
            get
            {
                if (RegBelong == null)
                    return false;

                return RegBelong.Type == ScopeType.Package;
            }
        }

        public override string ToString()
        {
            if ( RegIndex == -1 )
                return string.Format("'{0}' ({1}) {2}", Name, Usage, DefinePos);

            string RegType = IsGlobal ? "G":"R";

            return string.Format("'{0}' ({1}) {2} {3}{4}", Name, Usage, DefinePos, RegType, RegIndex);
        }
    }
}
