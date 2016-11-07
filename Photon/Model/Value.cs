
namespace Photon.Model
{
    public class Value
    {
        public virtual bool Equal( Value other )
        {
            return false;
        }

        public static Value Empty = new Value();

        public override string ToString()
        {
            return "(empty)";
        }
    }

}
