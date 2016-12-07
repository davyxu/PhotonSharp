
namespace Photon
{
    class ValueFloat32 : Value
    {
        float _data = 0;

        public ValueFloat32( float data )
        {
            _data = data;
        }

        public float RawValue
        {
            get { return _data; }
        }

        internal override object Raw
        {
            get { return _data; }
        }

        public override bool Equals(object other)
        {
            var otherT = other as ValueFloat32;
            if (otherT == null)
                return false;

            return otherT._data == _data;
        }
        public override string DebugString()
        {
            return _data.ToString() + " (float32)";
        }

        public override ValueKind Kind
        {
            get { return ValueKind.Float32; }
        }
    }



 
}
