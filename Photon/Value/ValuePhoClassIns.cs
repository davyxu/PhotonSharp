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


        internal override void OperateSetMemberValue(int nameKey, Value v)
        {
            Value tt;
            if (!_type.GetVirtualMember(nameKey, out tt))
            {
                throw new RuntimeException("member not exists:" + _type.Key2Name(nameKey));
            }

            if (tt.Kind == ValueKind.Func)
            {
                throw new RuntimeException("member function is immutable:" + _type.Key2Name(nameKey));
            }


            _memberVar[nameKey] = v;            
        }

        internal override Value OperateGetMemberValue(int nameKey)
        {
            // 从类型虚表中取
            Value tt;
            if (!_type.GetVirtualMember(nameKey, out tt))
            {
                throw new RuntimeException("member not exists:" + _type.Key2Name(nameKey));
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

        internal Value GetBaseValue(int nameKey)
        {
            // 从类型虚表中取
            Value tt;
            if (!_type.GetBaseMember(nameKey, out tt))
            {
                throw new RuntimeException("member not exists:" + _type.Key2Name(nameKey));
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
