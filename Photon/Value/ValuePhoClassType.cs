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


        internal override bool Equal(Value v)
        {
            var other = v as ValuePhoClassType;

            return _name.Equals(other._name);
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
