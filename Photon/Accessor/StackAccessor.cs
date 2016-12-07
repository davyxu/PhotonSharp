using System;

namespace Photon
{
    public partial class DataStack
    {        
        public void PushInteger32(Int32 v)
        {
            Push(new ValueInteger32(v));
        }

        public void PushInteger64(Int64 v)
        {
            Push(new ValueInteger64(v));
        }

        public void PushBool(bool v)
        {
            Push(new ValueBool(v));
        }

        public void PushFloat32(float v)
        {
            Push(new ValueFloat32(v));
        }

        public void PushFloat64(float v)
        {
            Push(new ValueFloat64(v));
        }

        public void PushString(string v)
        {
            Push(new ValueString(v));
        }

        internal void PushValue(Value v)
        {
            Push(v);
        }

        public void PushNil()
        {
            Push(new ValueNil());
        }
    }
}
