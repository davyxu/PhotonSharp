using Photon.Model;
using System.Diagnostics;

namespace Photon.VM
{
    public class Register
    {
        Value[] _data;
        int _usedSlot = -1;
        string _usage;

        public Register( string usage, int maxReg )
        {
            _usage = usage;
            _data = new Value[maxReg];
        }

        public void Set( int index, Value v )
        {
            _data[index] = v;
            _usedSlot = index;
        }

        public Value Get( int index )
        {
            return _data[index];
        }

        public string ValueToString( int index )
        {
            var v = Get(index);
            if ( v == null )
            {
                return "null";
            }

            return v.ToString();
        }

        public void ClearTo( int index )
        {
            for (int i = index; i <= _usedSlot;i++ )
            {
                _data[i] = null;
            }

            _usedSlot = index;
        }

        public void Clear()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = null;
            }

            _usedSlot = 0;
        }

        public void DebugPrint()
        {            
            for (int i = 0; i <= _usedSlot; i++)
            {
                var v = _data[i];

                string str = "null";
                if (v != null)
                {
                    str = v.ToString();
                }

                Debug.WriteLine("[{0}] {1}: {2}", _usage, i, str);
            }
        }
    }
}
