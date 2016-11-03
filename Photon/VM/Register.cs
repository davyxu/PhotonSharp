using Photon.OpCode;
using System;
using System.Diagnostics;

namespace Photon.VM
{
    public class Register
    {
        DataValue[] _data;
        int _usedSlot = -1;
        string _usage;

        public Register( string usage, int maxReg )
        {
            _usage = usage;
            _data = new DataValue[maxReg];
        }

        public void Set( int index, DataValue v )
        {
            _data[index] = v;
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
                var v = _data[i];

                string str = "null";
                if (v != null)
                {
                    str = v.GetDesc();
                }

                Debug.WriteLine("[{0}] {1}: {2}", _usage, i, str);
            }
        }
    }
}
