using System.Reflection;

namespace Photon
{
    class ValueNativeClassIns : ValueObject
    {
        ValueNativeClassType _type;        

        object _data;

        internal override object Raw
        {
            get { return _data; }
        }

        internal ValueNativeClassIns(ValueNativeClassType t, object nativeIns )
        {
            _type = t;
            _data = nativeIns;
        }


        internal override void OperateSetMemberValue(int nameKey, Value v)
        {            
            object obj = _type.GetMember(nameKey);

            var fastProp = obj as NativeProperty;
            if (fastProp != null)
            {
                object retValue = v;
                fastProp(_data, ref retValue, false);
                return;
            }

            throw new RuntimeException("member not exists: " + _type.Key2Name(nameKey));
        }

        internal override Value OperateGetMemberValue(int nameKey)
        {
            object obj = _type.GetMember(nameKey);
            var func = obj as ValueNativeFunc;
            if (func != null)
                return func;

            var fastProp = obj as NativeProperty;
            if (fastProp != null)
            {
                object retValue = null;
                fastProp(_data, ref retValue, true);
                return retValue as Value;
            }

            throw new RuntimeException("member not exists: " + _type.Key2Name(nameKey));     
        }

        public override bool Equals(object other)
        {
            var otherT = other as ValueNativeClassIns;
            if (otherT == null)
                return false;

            return otherT._data == _data;
        }

        public override int GetHashCode()
        {
            return _data.GetHashCode();
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
            get { return ValueKind.NativeClassInstance; }
        }




    }

}
