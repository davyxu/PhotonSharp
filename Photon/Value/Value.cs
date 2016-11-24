
namespace Photon
{
    public enum ValueType
    {
        Nil = 0,
        Number,
        String,
        Func,
        Object,
    }

    class Value
    {
        internal virtual bool Equal(Value other)
        {
            return false;
        }

        public static Value Nil = new ValueNil();

        public override string ToString()
        {
            return "(NotImplementToString)";
        }

        public virtual string DebugString( )
        {
            return "(NotImplementToString)";
        }

        public virtual ValueType GetValueType()
        {
            return ValueType.Nil;
        }

        public float CastNumber( )
        {
            var v = this as ValueNumber;
            if (v == null)
            {
                throw new RuntimeException("expect number");                
            }

            return v.Number;
        }
        public string CastString()
        {
            var v = this as ValueString;
            if (v == null)
            {
                throw new RuntimeException("expect string");
            }

            return v.String;
        }


        public ValueObject CastObject()
        {
            var v = this as ValueObject;
            if (v == null)
            {
                throw new RuntimeException("expect object");
            }

            return v;
        }

        public ValueFunc CastFunc()
        {
            var v = this as ValueFunc;
            if (v == null)
            {
                throw new RuntimeException("expect function");
            }

            return v;
        }

        public ValueClosure CastClosure()
        {
            var v = this as ValueClosure;
            if (v == null)
            {
                throw new RuntimeException("expect closure");
            }

            return v;
        }
    }




}
