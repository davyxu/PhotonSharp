using System;

namespace Photon.Model
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
            return Value.Empty;
        }

        public virtual Value Select(Value obj)
        {
            return Value.Empty;
        }

         public virtual void Set(Value obj, Value value )
         {

         }


    }

}
