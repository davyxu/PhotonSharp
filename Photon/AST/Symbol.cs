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


    internal class Symbol : IMarkSerializable
    {        
        public string Name;
     
        public TokenPos DefinePos;

        public Node Decl;
        
        public int RegIndex;

        public Scope RegBelong;
        
        public SymbolUsage Usage;


        public void Serialize(BinarySerializer ser)
        {
            ser.Serialize(ref Name);
            ser.Serialize(ref DefinePos);
            ser.Serialize(ref RegIndex);
            ser.Serialize(ref Usage);
        }


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
