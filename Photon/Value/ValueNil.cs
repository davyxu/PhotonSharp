
namespace Photon
{
    class ValueNil : Value
    {
        public override bool Equals(object other)
        {
            return (other as ValueNil) != null;
        }

        public override string DebugString()
        {
            return "(nil)";
        }

        public override string ToString()
        {
            return "nil";
        }

        public override ValueKind Kind
        {
            get { return ValueKind.Nil; }
        }
    }
}
