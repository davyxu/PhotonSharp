using System;
using System.Collections.Generic;

namespace Photon
{
    class ValueClassIns : ValueObject
    {
        ClassType _type;

        Dictionary<int, Value> _memberVar = new Dictionary<int, Value>();

        internal ValueClassIns(ClassType t)
        {
            _type = t;
        }

        internal void SetMember( int nameKey, Value v )
        {
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

        internal Value GetMember(int nameKey )
        {
            Value existV;
            if (_memberVar.TryGetValue(nameKey, out existV))
            {
                return existV;
            }

            return Value.Nil;
        }

        public override string DebugString()
        {
            return string.Format("{0}(class ins)", _type.Name);
        }

        public override ValueType GetValueType()
        {
            return ValueType.ClassInstance;
        }


    }

}
