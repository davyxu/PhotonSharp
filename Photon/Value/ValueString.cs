
namespace Photon
{
    public class ValueString : Value
    {
        string _str;

        public ValueString(string v)
        {
            _str = v;
        }

        public string String
        {
            get { return _str; }
        }

        public override bool Equal(Value other)
        {
            var otherT = other as ValueString;
            if (otherT == null)
                return false;

            return otherT._str == _str;
        }

        public override string ToString()
        {
            return string.Format("'{0}' (string)", _str.ToString());
        }
    }



 
}
