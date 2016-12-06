using System;

namespace Photon
{
    class ValueObject : Value
    {
        internal virtual void SetValue( int nameKey, Value v )
        {

        }

        internal virtual Value GetValue(int nameKey)
        {
            return Value.Nil;
        }

        internal virtual void SetKeyValue(Value k, Value v)
        {

        }

        internal virtual Value GetKeyValue(Value k)
        {
            return Value.Nil;
        }

        internal virtual object Raw
        {
            get { return null; }
        }


    }

}
