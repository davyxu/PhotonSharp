using System;

namespace Photon
{
    class ValueClassType : ValueObject
    {
        public ClassType CoreClass;

        internal ValueClassType(ClassType c)
        {
            CoreClass = c;
        }

        internal override bool Equal(Value v)
        {
            var other = v as ValueClassType;

            return other.CoreClass == this.CoreClass;
        }

        public override string DebugString()
        {
            return string.Format("{0}(class type)", TypeName);
        }

        public override string TypeName
        {
            get { return CoreClass.Name.ToString(); }
        }

        public override ValueKind Kind
        {
            get { return ValueKind.ClassType; }
        }


    }

}
