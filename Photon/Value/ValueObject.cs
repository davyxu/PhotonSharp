using System;

namespace Photon
{
    class ValueObject : Value
    {
        internal override bool Equal(Value other)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "(object)";
        }



        internal virtual Value Get(Value obj)
        {
            return Value.Nil;
        }

        internal virtual Value Select(Value obj)
        {
            return Value.Nil;
        }

        internal virtual void Set(Value obj, Value value)
         {

         }

         public override ValueType GetValueType()
         {
             return ValueType.Object;
         }


    }

}
