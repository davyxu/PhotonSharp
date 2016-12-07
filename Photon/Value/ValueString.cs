
namespace Photon
{
    class ValueString : Value
    {
        string _data;

        public ValueString(string v)
        {
            _data = v;
        }

        public string RawValue
        {
            get { return _data; }
        }

        internal override object Raw
        {
            get { return _data; }
        }

        public override bool Equals(object other)
        {
            var otherT = other as ValueString;
            if (otherT == null)
                return false;

            return otherT._data == _data;
        }

        public override string DebugString()
        {
            return string.Format("'{0}' (string)", _data);
        }

        public override ValueKind Kind
        {
            get { return ValueKind.String; }
        }
    }



 
}
