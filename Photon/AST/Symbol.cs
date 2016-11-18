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

        public Scope Belong;

        public SymbolUsage Usage;

        public bool IsGlobal
        {
            get
            {
                return Belong.Type == ScopeType.Global;
            }
        }

        public override string ToString()
        {
            if ( RegIndex == -1 )
                return string.Format("'{0}' {1} {2}", Name, Usage, DefinePos);


            return string.Format("'{0}' {1} {2} R{3}", Name, Usage, DefinePos, RegIndex);
        }
    }
}
