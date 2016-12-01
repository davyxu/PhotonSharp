using System.Collections.Generic;

namespace Photon
{
    public class ValueNativeClassIns : ValueObject
    {
        ValueNativeClassType _type;

        Dictionary<int, Value> _memberVar = new Dictionary<int, Value>();        

        internal ValueNativeClassIns(ValueNativeClassType t)
        {
            _type = t;
        }

        internal override void SetMember( int nameKey, Value v )
        {

        }

        internal override Value GetMember(int nameKey)
        {
            return Value.Nil;
        }

        public override string DebugString()
        {
            return string.Format("{0}(native class ins)", TypeName);
        }

        public override string TypeName
        {
            get { return _type.ToString(); }
        }


        public override ValueKind Kind
        {
            get { return ValueKind.ClassInstance; }
        }


    }

}
