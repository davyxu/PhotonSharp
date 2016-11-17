
namespace Photon
{
    public class Value
    {
        public virtual bool Equal( Value other )
        {
            return false;
        }

        public static Value Nil = new ValueNil();

        public override string ToString()
        {
            return "(empty)";
        }

        public float CastNumber( )
        {
            var v = this as ValueNumber;
            if (v == null)
            {
                throw new RuntimeExcetion("expect number");                
            }

            return v.Number;
        }
        public string CastString()
        {
            var v = this as ValueString;
            if (v == null)
            {
                throw new RuntimeExcetion("expect string");
            }

            return v.String;
        }


        public ValueObject CastObject()
        {
            var v = this as ValueObject;
            if (v == null)
            {
                throw new RuntimeExcetion("expect object");
            }

            return v;
        }

        public ValueFunc CastFunc()
        {
            var v = this as ValueFunc;
            if (v == null)
            {
                throw new RuntimeExcetion("expect function");
            }

            return v;
        }
    }




}
