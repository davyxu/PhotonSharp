using Photon.Model;
using System;
using System.Diagnostics;

namespace Photon.VM
{
    public class Register
    {
        Value[] _data;
        int _usedSlot = 0;
        string _usage;

        public Register( string usage, int maxReg )
        {
            _usage = usage;
            _data = new Value[maxReg];
        }

        public int Count
        {
            get { return _usedSlot; }
        }

        public void SetUsedCount( int count )
        {

            for (int i = 0; i < _data.Length; i++)
            {
                if (i >= count)
                {
                    _data[i] = null;
                }

            }

            _usedSlot = count;
        }

        public void Set( int index, Value v )
        {
            _data[index] = v;            
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
            for (int i = 0; i < _usedSlot; i++)
            {
                var v = _data[i];

                string str = "null";
                if (v != null)
                {
                    str = v.ToString();
                }

                Debug.WriteLine("R{0}: {1}", i, str);
            }
        }
    }
}
