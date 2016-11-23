
namespace Photon
{
    class ValueNumber : Value
    {
        float _number = 0;

        public ValueNumber( float number )
        {
            _number = number;
        }

        public float Number
        {
            get { return _number; }
        }

        internal override bool Equal(Value other)
        {
            var otherT = other as ValueNumber;
            if (otherT == null)
                return false;

            return otherT._number == _number;
        }
        public override string DebugString()
        {
            return _number.ToString() + "(number)";
        }

        public override string ToString()
        {
            return _number.ToString();
        }

        public override ValueType GetValueType()
        {
            return ValueType.Number;
        }
    }



 
}
