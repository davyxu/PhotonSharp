using System;

namespace Photon
{
    public partial class DataStack
    {        
        public void PushInteger32(Int32 v)
        {
            Push(new ValueNumber((float)v));
        }

        public void PushBool(bool v)
        {
            Push(new ValueBool(v));
        }

        public void PushFloat32(float v)
        {
            Push(new ValueNumber(v));
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
