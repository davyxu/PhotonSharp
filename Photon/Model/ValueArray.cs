using System.Collections.Generic;

namespace Photon.Model
{
    public class ValueArray : ValueObject
    {
        List<Value> _value = new List<Value>();

        public override string ToString()
        {
            return "(array)";
        }

        public override Value Get(Value obj)
        {
            var key = obj as ValueNumber;

            if (key == null)
                return Value.Empty;

            return _value[(int)key.Number];
        }

        public override void Set(Value obj, Value value )
        {
            var key = obj as ValueNumber;

            if (key == null)
                return;

            _value[(int)key.Number] = value;
        }
    }

}
