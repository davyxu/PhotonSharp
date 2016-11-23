
using System.Diagnostics;

namespace Photon
{
    public class Register : DataAccessor
    {
        Slot[] _values;
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
            _values = new Slot[maxReg];
        }

        public int Count
        {
            get { return _usedSlot; }
        }

        internal void SetUsedCount( int count )
        {            
            // 扩展
            if ( count > _usedSlot )
            {
                for (int i = _usedSlot; i < count; i++)
                {
                    // 重新分配槽, 避免覆盖原来的值
                    _values[i] = new Slot(GenSlotID());
                }
            }
            // 缩减
            else if ( count < _usedSlot )
            {
                for( int i = count; i < _usedSlot; i++ )
                {
                    // 重新分配槽, 避免覆盖原来的值
                    _values[i] = new Slot(GenSlotID());
                }

            }

            _usedSlot = count;
        }

        internal override void Set( int index, Value v )
        {            
            _values[index].SetData(v);
        }

        internal override Value Get(int index)
        {
            return _values[index].Data;
        }

        internal Slot GetSlot( int index )
        {
            return _values[index];
        }

        internal void Clear()
        {
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i].SetData( Value.Nil );
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
                var v = _values[i].Data;

                Debug.WriteLine("{0}{1}: {2}", _usage, i, v.ToString());
            }
        }
    }
}
