
namespace Photon
{
    public enum ValueKind
    {
        Nil = 0,
        Number,
        Bool,
        String,
        Func,        
        ClassType,
        ClassInstance,
        NativeClassType,
        NativeClassInstance,
    }

    partial class Value
    {
        internal virtual bool Equal(Value other)
        {
            return false;
        }

        internal static Value Nil = new ValueNil();

        public override string ToString()
        {
            return DebugString();
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

     
    }




}
