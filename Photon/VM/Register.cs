using Photon.OpCode;
using System.Diagnostics;

namespace Photon.VM
{
    public class Register
    {
        DataValue[] _data;
        int _usedSlot;
        string _usage;

        public Register( string usage, int maxReg )
        {
            _usage = usage;
            _data = new DataValue[maxReg];
        }

        public void Set( int index, DataValue data )
        {
            _data[index] = data;
            _usedSlot = index;
        }

        public DataValue Get( int index )
        {
            return _data[index];
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
                if (_data[i] == null)
                    continue;

                Debug.WriteLine("[{0}] {1}: {2}", _usage, i, _data[i].GetDesc());
            }
        }
    }
}
