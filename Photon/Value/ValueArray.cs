using System.Collections.Generic;

namespace Photon
{
    class ValueArray : ValueObject
    {
        List<Value> _value = new List<Value>();

        public override string ToString()
        {
            return "(array)";
        }

        internal override Value Select(Value obj)
        {
            var method = obj as ValueString;

            switch (method.String)
            {
                case "append":
                    {
                        return new ValueDelegate( Append );
                    }                    
            }

            return Value.Nil;
        }

        int Append( VMachine vm )
        {
            var v = vm.DataStack.Pop();

            _value.Add(v);

            return 0;
        }

        internal override Value Get(Value obj)
        {
            var key = obj as ValueNumber;

            if (key == null)
                return Value.Nil;

            return _value[(int)key.Number];
        }

        internal override void Set(Value obj, Value value)
        {
            var key = obj as ValueNumber;

            if (key == null)
                return;

            _value[(int)key.Number] = value;
        }
    }

}
