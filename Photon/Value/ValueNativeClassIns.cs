using System.Collections.Generic;
using System.Reflection;

namespace Photon
{
    class ValueNativeClassIns : ValueObject
    {
        ValueNativeClassType _type;        

        object _nativeIns;

        internal override object Instance
        {
            get { return _nativeIns; }
        }

        internal ValueNativeClassIns(ValueNativeClassType t, object nativeIns )
        {
            _type = t;
            _nativeIns = nativeIns;
        }

        internal override void SetMember( int nameKey, Value v )
        {

        }


        internal override Value GetMember(int nameKey)
        {
            return _type.GetMethod(nameKey);
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
