
using System.Diagnostics;

namespace Photon
{
    public class Register : DataAccessor
    {
        Value[] _values;
        int _usedSlot = 0;
        string _usage;

        int _slotIDGen = 0;

        int GenSlotID( )
        {
            return ++_slotIDGen;
        }

        internal Register( string usage, int maxReg )
        {
            _usage = usage;
            _values = new Value[maxReg];
            Clear();
        }

        public int Count
        {
            get { return _usedSlot; }
        }

        internal void SetUsedCount( int count )
        {            
            _usedSlot = count;
        }

        internal override void Set( int index, Value v )
        {            
            _values[index] = v;
        }

        internal override Value Get(int index)
        {
            return _values[index];
        }

        internal void Clear()
        {
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = Value.Nil;
            }

            _usedSlot = 0;
        }

        public override string ToString()
        {
            return string.Format("{0} used:{1}", _usage, _usedSlot);
        }

        public void DebugPrint()
        {            
            for (int i = 0; i < _usedSlot; i++)
            {
                var v = _values[i];

                Debug.WriteLine("{0}{1}: {2}", _usage, i, v.ToString());
            }
        }
    }
}
