
namespace Photon
{
    class ValueString : Value
    {
        string _str;

        public ValueString(string v)
        {
            _str = v;
        }

        public string Raw
        {
            get { return _str; }
        }

        public override bool Equals(object other)
        {
            var otherT = other as ValueString;
            if (otherT == null)
                return false;

            return otherT._str == _str;
        }

        public override string DebugString()
        {
            return string.Format("'{0}' (string)", _str);
        }

        public override ValueKind Kind
        {
            get { return ValueKind.String; }
        }
    }



 
}
