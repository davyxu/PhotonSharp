
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

        internal override bool Equal(Value other)
        {
            var otherT = other as ValueBool;
            if (otherT == null)
                return false;

            return otherT._data == _data;
        }
        public override string DebugString()
        {
            return _data.ToString() + "(bool)";
        }

        public override string ToString()
        {
            return _data.ToString();
        }

        public override ValueKind Kind
        {
            get { return ValueKind.Bool; }
        }
    }



 
}
