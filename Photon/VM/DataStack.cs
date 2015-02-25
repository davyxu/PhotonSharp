using Photon.OpCode;
using System.Diagnostics;

namespace Photon.VM
{
    public class DataStack
    {
        DataValue[] _values;
        int _count = 0;

        public DataStack( int max )
        {
            _values = new DataValue[max];
        }

        public int Count
        {
            get { return _count; }
        }

        public void Clear()
        {
            for (int i = 0; i < _values.Length;i++ )
            {
                _values[i] = null;
            }

            _count = 0;
        }

        public void Push( DataValue v )
        {
            _values[_count] = v;
            _count++;
        }

        public DataValue Pop( )
        {
            var v = _values[_count - 1];

            // 调试功能, 让栈看起来清爽
            _values[_count - 1] = null;

            _count--;

            return v;
        }

        public void PopMulti( int count )
        {
            // 调试功能, 让栈看起来清爽
            for( int i = 1;i<= count;i++)
            {
                _values[_count - i] = null;
            }

            _count -= count;
        }

        public DataValue Peek( int index = -1 )
        {
            if ( index >=0 )
            {
                return _values[index];
            }

            
            return _values[_count + index];
        }

        public void DebugPrint( )
        {
            Debug.WriteLine("stack:");

            for( int i = 0;i < _count;i++)
            {
                Debug.WriteLine("{0}: {1}", i, _values[i].GetDesc());
            }
        }
    }
}
