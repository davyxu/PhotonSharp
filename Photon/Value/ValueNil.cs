
namespace Photon
{
    class ValueNil : Value
    {
        internal override bool Equal(Value other)
        {
            var otherT = other as ValueNil;
            if (otherT == null)
                return false;

            return true;
        }

        public override string DebugString()
        {
            return "(nil)";
        }

        public override string ToString()
        {
            return "nil";
        }

        public override ValueType GetValueType()
        {
            return ValueType.Nil;
        }
    }
}
