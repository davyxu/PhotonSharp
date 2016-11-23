using System;

namespace Photon
{    
    class ValueDelegate : Value
    {
        public Func<VMachine, int> Entry;        

        public ValueDelegate( )
        {

        }

        public ValueDelegate( Func<VMachine, int> callback )
        {
            Entry = callback;
        }

        internal override bool Equal(Value other)
        {
            var otherT = other as ValueDelegate;
            if (otherT == null)
                return false;

            return otherT.Entry == Entry;
        }


        public override string ToString()
        {
            return "(delegate)";
        }
    }

 
}
