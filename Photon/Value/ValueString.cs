
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

        internal override Value OperateUnary(Opcode code)
        {
            switch (code)
            {
                case Opcode.LEN:
                    return new ValueInteger32(_data.Length);
                default:
                    throw new RuntimeException("Unknown binary operator:" + code.ToString());
            }
            
        }

        public override bool Equals(object other)
        {
            var otherT = other as ValueString;
            if (otherT == null)
                return false;

            return otherT._data == _data;
        }

        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }

        public override string DebugString()
        {
            return string.Format("'{0}' (string)", _data);
        }

        public override ValueKind Kind
        {
            get { return ValueKind.String; }
        }

        internal override Value OperateBinary(Opcode code, Value other)
        {            
            var a = RawValue;

            var b = Convertor.ValueToString(other);            


            switch (code)
            {
                case Opcode.ADD:
                    return new ValueString(a + b);
                default:
                    throw new RuntimeException("Unknown binary operator:" + code.ToString());
            }
        }
    }



 
}
