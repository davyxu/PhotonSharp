
using System;
namespace Photon
{
    public class DataAccessor
    {
        internal virtual Value Get(int index)
        {
            throw new NotImplementedException();
        }

        internal virtual void Set(int index, Value v)
        {
            throw new NotImplementedException();
        }

        public ValueKind GetValueKind( int index )
        {
            return Get(index).Kind;
        }

        public string GetString(int index)
        {
            return Get(index).CastString();
        }

        public void SetString(int index, string v)
        {
            Set(index, new ValueString(v));
        }


        public float GetFloat32( int index )
        {
            return Get(index).CastNumber();
        }

        public void SetFloat32(int index, float v)
        {
            Set(index, new ValueNumber(v));
        }

        public bool IsNil(int index)
        {
            return Get(index).Kind != ValueKind.Nil;
        }

        public void SetNil(int index)
        {
            Set(index, new ValueNil());
        }

        public string DebugString( int index )
        {
            return Get(index).DebugString();
        }
    }
}
