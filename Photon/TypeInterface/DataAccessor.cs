using System;

namespace Photon
{
    public partial class DataAccessor
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

        public string DebugString( int index )
        {
            return Get(index).DebugString();
        }

        public Int32 GetInteger32(int index)
        {
            return (Int32)Convertor.ValueToFloat32(Get(index));
        }


        internal void SetInteger32(int index, Int32 v)
        {
            Set(index, new ValueNumber((float)v));
        }

        public bool GetBool(int index)
        {
            return Convertor.ValueToBool(Get(index));
        }
        internal void SetBool(int index, bool v)
        {
            Set(index, new ValueBool(v));
        }

        public float GetFloat32(int index)
        {
            return Convertor.ValueToFloat32(Get(index));
        }

        internal void SetFloat32(int index, float v)
        {
            Set(index, new ValueNumber(v));
        }

        public string GetString(int index)
        {
            return Convertor.ValueToString(Get(index));
        }

        public void SetString(int index, string v)
        {
            Set(index, new ValueString(v));
        }

        internal Value GetValue(int index)
        {
            return Get(index);
        }

        public T GetNativeInstance<T>(int index) where T : class
        {
            return Convertor.CastObject(Get(index)).Raw as T;
        }

        public bool IsNil(int index)
        {
            return Get(index).Kind == ValueKind.Nil;
        }

        internal void SetNil(int index)
        {
            Set(index, new ValueNil());
        }
    }
}
