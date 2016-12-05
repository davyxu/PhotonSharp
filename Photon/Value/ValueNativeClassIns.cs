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
            object obj = _type.GetMember(nameKey);

            var prop = obj as PropertyInfo;

            if (prop != null)
            {
                var lanV = ValueNativeClassType.PhoValue2NativeValue(prop.PropertyType, v);

                prop.SetValue(_nativeIns, lanV);
                return;
            }

            throw new RuntimeException("member not exists: "+ _type.Key2Name(nameKey));
        }


        internal override Value GetMember(int nameKey)
        {
            object obj = _type.GetMember(nameKey);
            var func = obj as ValueNativeFunc;
            if (func != null)
                return func;

            var prop = obj as PropertyInfo;

            if ( prop != null )
            {                
                var v = prop.GetValue(_nativeIns);

                return ValueNativeClassType.NativeValue2PhoValue(prop.PropertyType, v);
            }

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
