
namespace Photon
{
    class ValueClassType : Value
    {
        internal int ID { get; set; }

        protected ObjectName _name;
        public ObjectName Name
        {
            get { return _name; }
        }        

        internal ValueClassType( ObjectName name)
        {            
            _name = name;
        }

        internal override bool Equal(Value v)
        {
            var other = v as ValueClassType;

            return _name.Equals(other._name);
        }

        public override string TypeName
        {
            get { return Name.ToString(); }
        }

        internal virtual ValueObject CreateInstance( )
        {
            return null;
        }
    }

}
