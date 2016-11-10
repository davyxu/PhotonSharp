using Photon.Model;
using System;
using System.Diagnostics;

namespace Photon.VM
{
    public class Register
    {
        Value[] _values;
        int _usedSlot = 0;
        string _usage;

        public Register( string usage, int maxReg )
        {
            _usage = usage;
            _values = new Value[maxReg];
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = Value.Nil;
            }
        }

        public int Count
        {
            get { return _usedSlot; }
        }

        public void SetUsedCount( int count )
        {

            for (int i = 0; i < _values.Length; i++)
            {
                if (i >= count)
                {
                    _values[i] = Value.Nil;
                }

            }

            _usedSlot = count;
        }

        public void Set( int index, Value v )
        {
            _values[index] = v;            
        }

        public Value Get( int index )
        {
            return _values[index];
        }

        public string ValueToString(int index = -1)
        {
            return Get(index).ToString();
        }


        public void Clear()
        {
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = Value.Nil;
            }

            _usedSlot = 0;
        }

        public void DebugPrint()
        {            
            for (int i = 0; i < _usedSlot; i++)
            {
                var v = _values[i];

                Debug.WriteLine("R{0}: {1}", i, v.ToString());
            }
        }
    }
}
