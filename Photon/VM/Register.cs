
using System.Diagnostics;

namespace Photon
{
    public class Register
    {
        Slot[] _values;
        int _usedSlot = 0;
        string _usage;

        int _slotIDGen = 0;

        int GenSlotID( )
        {
            return ++_slotIDGen;
        }

        public Register( string usage, int maxReg )
        {
            _usage = usage;
            _values = new Slot[maxReg];
        }

        public int Count
        {
            get { return _usedSlot; }
        }

        public void SetUsedCount( int count )
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

        public void Set( int index, Value v )
        {            
            _values[index].SetData(v);
        }

        public Value Get( int index )
        {
            return _values[index].Data;
        }

        public Slot GetSlot( int index )
        {
            return _values[index];
        }

        public void Clear()
        {
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i].SetData( Value.Nil );
            }

            _usedSlot = 0;
        }

        public void DebugPrint()
        {            
            for (int i = 0; i < _usedSlot; i++)
            {
                var v = _values[i].Data;

                Debug.WriteLine("R{0}: {1}", i, v.ToString());
            }
        }
    }
}
