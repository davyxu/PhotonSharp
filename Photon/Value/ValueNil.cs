
namespace Photon
{
    public class ValueNil : Value
    {
        public override bool Equal(Value other)
        {
            var otherT = other as ValueNil;
            if (otherT == null)
                return false;

            return true;
        }

        public override string ToString()
        {
            return "(nil)";
        }
    }
}
