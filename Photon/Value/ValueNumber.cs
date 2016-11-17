
namespace Photon
{
    public class ValueNumber : Value
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

        public override bool Equal(Value other)
        {
            var otherT = other as ValueNumber;
            if (otherT == null)
                return false;

            return otherT._number == _number;
        }

        public override string ToString()
        {
            return _number.ToString() + "(number)";
        }
    }



 
}
