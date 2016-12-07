
namespace Photon
{
    class ValueBool : Value
    {
        bool _data;

        public ValueBool(bool data)
        {
            _data = data;
        }

        public bool Raw
        {
            get { return _data; }
        }

        public override bool Equals(object other)
        {
            var otherT = other as ValueBool;
            if (otherT == null)
                return false;

            return otherT._data == _data;
        }
        public override string DebugString()
        {
            return _data.ToString() + " (bool)";
        }

        public override ValueKind Kind
        {
            get { return ValueKind.Bool; }
        }
    }



 
}
