using MarkSerializer;
using System.Collections.Generic;

namespace Photon
{
    class ValuePhoClassType : ValueClassType
    {
        Dictionary<int, Value> _member = new Dictionary<int, Value>();

        internal ValuePhoClassType Parent { get; set; }
        internal int ParentID = 0;

        internal override int GetInheritLevel( )
        {
            ValuePhoClassType classType = Parent;

            int total = 0;

            while (classType != null)
            {
                total++;

                classType = classType.Parent;
            }

            return total;            
        }


        public ValuePhoClassType()
        {

        }

        internal ValuePhoClassType(Package pkg, ObjectName name)
            : base( name )
        {
            _pkg = pkg;
        }

        public override void Serialize(BinarySerializer ser)
        {
            base.Serialize(ser);

            ser.Serialize(ref _member);
            ser.Serialize(ref ParentID);
                      

        }

        internal override void OnSerializeDone(Executable exe)
        {
            Parent = exe.FindClassByPersistantID(ParentID);         
        }

        internal void AddMethod( int nameKey, ValueFunc proc )
        {
            _member.Add(nameKey, proc );
        }

        internal void AddMemeber( int nameKey, string name )
        {
            _member.Add(nameKey, new ValueNil());
        }

        internal bool GetBaseMember(int nameKey, out Value v)
        {
            v = Value.Nil;

            if (Parent == null)
                return true;

            if (Parent._member.TryGetValue(nameKey, out v))
            {
                return true;
            }

            return false;
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
