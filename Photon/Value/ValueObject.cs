using System;

namespace Photon
{
    class ValueObject : Value
    {
        internal override bool Equal(Value other)
        {
            throw new NotImplementedException();
        }

        public override ValueKind Kind
        {
            get { return ValueKind.Object; }
        }


    }

}
