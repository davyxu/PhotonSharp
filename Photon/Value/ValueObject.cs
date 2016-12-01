using System;

namespace Photon
{
    public class ValueObject : Value
    {
        internal override bool Equal(Value other)
        {
            throw new NotImplementedException();
        }

        public override ValueKind Kind
        {
            get { return ValueKind.Object; }
        }

        internal virtual void SetMember( int nameKey, Value v )
        {

        }

        internal virtual Value GetMember(int nameKey)
        {
            return Value.Nil;
        }


    }

}
