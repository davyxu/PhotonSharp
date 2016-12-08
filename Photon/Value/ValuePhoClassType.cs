using System.Collections.Generic;

namespace Photon
{
    class ValuePhoClassType : ValueClassType
    {
        Dictionary<int, Value> _member = new Dictionary<int, Value>();

        internal ValuePhoClassType Parent { get; set; }

        internal ValuePhoClassType( ObjectName name)
            : base( name )
        {

        }

        internal void AddMethod( int nameKey, ValueFunc proc )
        {
            _member.Add(nameKey, proc );
        }

        internal void AddMemeber( int nameKey, string name )
        {
            _member.Add(nameKey, new ValueNil());
        }

        internal bool GetVirtualMember( int nameKey, out Value v )
        {
            v = Value.Nil;

            ValuePhoClassType ct = this;

            while( ct != null )
            {                
                if (ct._member.TryGetValue(nameKey, out v))
                {
                    return true;
                }

                ct = ct.Parent;
            }

            return false;
        }


        public override bool Equals(object v)
        {
            var other = v as ValuePhoClassType;
            if (other == null)
                return false;

            return _name.Equals(other._name);
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }

        internal override ValueObject CreateInstance()
        {
            return new ValuePhoClassIns( this );
        }

        public override string DebugString()
        {
            return string.Format("{0}(class type)", TypeName);
        }

        public override string TypeName
        {
            get { return Name.ToString(); }
        }

        public override ValueKind Kind
        {
            get { return ValueKind.ClassType; }
        }


    }

}
