
namespace Photon
{
    public enum ValueKind
    {
        Nil = 0,
        Float32,
        Integer32,
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

    partial class Value
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
     
    }




}
