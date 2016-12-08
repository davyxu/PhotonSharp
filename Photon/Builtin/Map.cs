using System.Collections.Generic;
using System.Reflection;

namespace Photon
{
    class Map : IContainer
    {
        Dictionary<Value, Value> _data = new Dictionary<Value, Value>();

        [NoGenWrapper]
        public void SetKeyValue(Value k, Value v)
        {
            if ( v.Equals(Value.Nil) )
            {
                _data.Remove(k);
            }
            else if (!_data.ContainsKey(k))
            {
                _data[k] = v;
            }
        }

        [NoGenWrapper]
        public Value GetKeyValue(Value k)
        {
            Value ret;
            if (_data.TryGetValue(k, out ret))
            {
                return ret;
            }

            return Value.Nil;
        }

        [NoGenWrapper]
        public int GetCount()
        {
            return _data.Count;
        }


        public bool ContainsKey(Value k)
        {
            return _data.ContainsKey(k);
        }

        public bool ContainsValue(Value k)
        {
            return _data.ContainsKey(k);
        }

        public void Clear()
        {
            _data.Clear();            
        }

        internal static void Register( Executable exe )
        {
            exe.RegisterNativeClass(Assembly.GetExecutingAssembly(), "Photon.MapWrapper", "Builtin");
        }

        internal static void GenerateWrapper()
        {
            WrapperCodeGenerator.GenerateClass(typeof(Photon.Map), "Photon", "../Photon/Builtin/MapWrapper.cs");
        }
    }
}
