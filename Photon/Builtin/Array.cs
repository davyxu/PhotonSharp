using System.Collections.Generic;
using System.Reflection;

namespace Photon
{
    class Array : IContainer
    {
        List<Value> _data = new List<Value>();

        public void Add(Value v)
        {
            _data.Add(v);
        }

        public void Remove(Value v)
        {
            _data.Remove(v);
        }

        public void RemoveAt(int index)
        {
            _data.RemoveAt(index);
        }

        public bool TryGet(int index, out Value v )
        {
            if (index > 0 || index >= _data.Count)
            {
                v = Value.Nil;

                return false;
            }

            v = _data[index];

            return true;
        }

        public bool TrySet(int index, Value v )
        {
            if (index > 0 || index >= _data.Count)
            {                
                return false;
            }            

            _data[index] = v;

            return true;
        }

        public Value Get(int index)
        {
            if (index > 0 || index >= _data.Count)
            {
                throw new RuntimeException("Array out of bound");
            }

            return _data[index];
        }

        public void Set(int index, Value v)
        {
            if (index > 0 || index >= _data.Count)
            {
                throw new RuntimeException("Array out of bound");
            }            

            _data[index] = v;
        }

        public bool Contains(Value v)
        {
            return _data.Contains(v);
        }

        public int IndexOf(Value v)
        {
            return _data.IndexOf(v);
        }

        public void Clear()
        {
            _data.Clear();
        }

        [NoGenWrapper]
        public void SetKeyValue(Value k, Value v)
        {
            int key = Convertor.ValueToInteger32(k);
            Set(key, v);
        }

        [NoGenWrapper]
        public Value GetKeyValue(Value k)
        {
            int key = Convertor.ValueToInteger32(k);
            return Get(key);
        }

        [NoGenWrapper]
        public int GetCount( )
        {
            return _data.Count;
        }

        internal static void Register( Executable exe )
        {
            exe.RegisterNativeClass(Assembly.GetExecutingAssembly(), "Photon.ArrayWrapper", "Builtin");
        }

        internal static void GenerateWrapper()
        {
            WrapperCodeGenerator.GenerateClass(typeof(Photon.Array), "Photon", "../Photon/Builtin/ArrayWrapper.cs");
        }
    }
}
