using System;

namespace Photon
{
    public class ValueNativeClassType : ValueClassType
    {
        internal int ID { get; set; }                
        
        Executable _exe;
        Type _t;

        internal Type Raw
        {
            get { return _t; }
        }

        internal ValueNativeClassType(Executable exe, Type t )
            : base( exe, ObjectName.Empty)
        {
            _exe = exe;
            _t = t;
        }

        internal override ValueObject CreateInstance()
        {
            return new ValueNativeClassIns(this);
        }

        internal override bool Equal(Value v)
        {
            var other = v as ValueNativeClassType;

            return _t.Equals(other._t);
        }

        public override string DebugString()
        {
            return string.Format("{0}(native class type)", TypeName);
        }

        public override string TypeName
        {
            get { return _t.Name; }
        }

        public override ValueKind Kind
        {
            get { return ValueKind.NativeClassType; }
        }


    }

}
