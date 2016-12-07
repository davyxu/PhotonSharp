using System;

namespace Photon
{
    class ValueInteger32 : Value
    {
        Int32 _data = 0;

        public ValueInteger32(Int32 data)
        {
            _data = data;
        }

        public Int32 RawValue
        {
            get { return _data; }
        }

        internal override object Raw
        {
            get { return _data; }
        }

        public override bool Equals(object other)
        {
            var otherT = other as ValueInteger32;
            if (otherT == null)
                return false;

            return otherT._data == _data;
        }
        public override string DebugString()
        {
            return _data.ToString() + " (int32)";
        }

        public override ValueKind Kind
        {
            get { return ValueKind.Integer32; }
        }
    }



 
}
