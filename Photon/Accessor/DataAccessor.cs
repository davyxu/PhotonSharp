﻿using System;

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

        public virtual string DebugString(int index)
        {
            return Get(index).DebugString();
        }

        public ValueKind GetValueKind( int index )
        {
            return Get(index).Kind;
        }


        public Int32 GetInteger32(int index)
        {
            return Convertor.ValueToInteger32(Get(index));
        }

        public Int64 GetInteger64(int index)
        {
            return Convertor.ValueToInteger64(Get(index));
        }

        public bool GetBool(int index)
        {
            return Convertor.ValueToBool(Get(index));
        }

        public float GetFloat32(int index)
        {
            return Convertor.ValueToFloat32(Get(index));
        }

        public double GetFloat64(int index)
        {
            return Convertor.ValueToFloat64(Get(index));
        }

        public string GetString(int index)
        {
            return Convertor.ValueToString(Get(index));
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


        internal void SetInteger32(int index, Int32 v)
        {
            Set(index, new ValueInteger32(v));
        }

        internal void SetInteger64(int index, Int64 v)
        {
            Set(index, new ValueInteger64(v));
        }

        internal void SetBool(int index, bool v)
        {
            Set(index, new ValueBool(v));
        }

        internal void SetFloat32(int index, float v)
        {
            Set(index, new ValueFloat32(v));
        }
        internal void SetFloat64(int index, double v)
        {
            Set(index, new ValueFloat64(v));
        }

        public void SetString(int index, string v)
        {
            Set(index, new ValueString(v));
        }

        internal void SetNil(int index)
        {
            Set(index, new ValueNil());
        }
    }
}
