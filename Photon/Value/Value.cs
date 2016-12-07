
namespace Photon
{
    public enum ValueKind
    {
        Nil = 0,
        Float32,
        Float64,
        Integer32,
        Integer64,
        Bool,
        String,
        Func,        
        ClassType,
        ClassInstance,
        NativeClassType,
        NativeClassInstance,
    }

    internal interface IContainer
    {
        void SetKeyValue(Value k, Value v);

        Value GetKeyValue(Value k);        
    }

    class Value
    {
        public override bool Equals(object obj)
        {
 	         return false;
        }        

        internal static Value Nil = new ValueNil();

        public override string ToString()
        {
            return DebugString();
        }

        public override int GetHashCode()
        {
            return Raw.GetHashCode();
        }

        public virtual string DebugString( )
        {
            throw new RuntimeException("NotImplementToString");            
        }

        public virtual ValueKind Kind
        {
            get { return ValueKind.Nil; }
        }

        public virtual string TypeName
        {
            get { return string.Empty; }
        }

        internal virtual object Raw
        {
            get { throw new System.NotImplementedException(); }
        }

        internal virtual Value BinaryOperate(Opcode code, Value other)
        {
            throw new RuntimeException("Expect numeral value");
        }

        internal virtual Value UnaryOperate(Opcode code)
        {
            throw new RuntimeException("Expect numeral value");
        }
     
    }



}
