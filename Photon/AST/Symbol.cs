using MarkSerializer;
using SharpLexer;

namespace Photon
{
    enum SymbolUsage
    {
        None,
        Delegate,
        Func,
        Variable,
        Constant,
        SelfParameter,
        Parameter,
        Member,
        Package,
        Class,
    }


    internal class Symbol
    {
        [MarkSerialize]
        public string Name;

        [MarkSerialize]
        public TokenPos DefinePos;

        public Node Decl;

        [MarkSerialize]
        public int RegIndex;

        public Scope RegBelong;

        [MarkSerialize]
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
