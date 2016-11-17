using System;

namespace Photon
{
    public class ValueObject : Value
    {                
        public override bool Equal(Value other)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "(object)";
        }



        public virtual Value Get( Value obj )
        {
            return Value.Nil;
        }

        public virtual Value Select(Value obj)
        {
            return Value.Nil;
        }

         public virtual void Set(Value obj, Value value )
         {

         }


    }

}
