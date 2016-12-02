using System.Collections.Generic;

namespace Photon
{
    class ValuePhoClassIns : ValueObject
    {
        ValuePhoClassType _type;

        Dictionary<int, Value> _memberVar = new Dictionary<int, Value>();

        internal ValuePhoClassIns(ValuePhoClassType t)
        {
            _type = t;
        }

        internal override void SetMember(int nameKey, Value v)
        {
            Value tt;
            if (!_type.GetVirtualMember( nameKey, out tt ))
            {
                throw new RuntimeException("member not exists");
            }

            if ( tt.Kind == ValueKind.Func )
            {
                throw new RuntimeException("member function is immutable");
            }


            Value existV;
            if (_memberVar.TryGetValue(nameKey, out existV))
            {
                existV = v;
            }
            else
            {
                _memberVar.Add(nameKey, v);
            }
        }

        internal override Value GetMember(int nameKey)
        {
            // 从类型虚表中取
            Value tt;
            if (!_type.GetVirtualMember(nameKey, out tt))
            {
                throw new RuntimeException("member not exists");
            }

            // 函数优先返回
            if (tt.Kind == ValueKind.Func)
            {
                return tt;
            }


            Value existV;
            if (_memberVar.TryGetValue(nameKey, out existV))
            {
                return existV;
            }

            return Value.Nil;
        }

        public override string DebugString()
        {
            return string.Format("{0}(class ins)", TypeName);
        }

        public override string TypeName
        {
            get { return _type.Name.ToString(); }
        }


        public override ValueKind Kind
        {
            get { return ValueKind.ClassInstance; }
        }


    }

}
