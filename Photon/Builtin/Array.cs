using System.Collections.Generic;
using System.Reflection;

namespace Photon
{
    class Array
    {
        List<Value> _data = new List<Value>();

        public void Add(Value v)
        {
            _data.Add(v);
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

        public int Count
        {
            get { return _data.Count; }
        }

        internal static void Register( Executable exe )
        {
            exe.RegisterNativeClass(Assembly.GetExecutingAssembly(), "Photon.ArrayWrapper", "Builtin");
        }

        internal static void GenerateWrapper()
        {
            WrapperCodeGenerator.GenerateClass(typeof(Array), "Photon", "../Photon/Builtin/ArrayWrapper.cs");
        }
    }
}
