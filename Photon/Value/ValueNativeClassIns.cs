using System.Collections.Generic;
using System.Reflection;

namespace Photon
{
    public class ValueNativeClassIns : ValueObject
    {
        ValueNativeClassType _type;

        Dictionary<int, ValueNativeFunc> _memberVar = new Dictionary<int, ValueNativeFunc>();

        object _nativeIns;

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
            ValueNativeFunc func;
            if (_memberVar.TryGetValue( nameKey, out func ))
            {
                return func;
            }

            MethodInfo methodInfo;
            var nativeDel = _type.CreateMethodDelegate(nameKey, _nativeIns, out methodInfo);
            if (nativeDel == null)
            {
                throw new RuntimeException("method not found: " + methodInfo.Name);
            }

            func = new ValueNativeFunc(new ObjectName(_type.Pkg.Name, methodInfo.Name), nativeDel);

            _memberVar.Add(nameKey, func );

            return func;
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
