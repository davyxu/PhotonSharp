using Photon.Model;
using System.Diagnostics;

namespace Photon.VM
{
    public class DataStack
    {
        Value[] _values;
        int _count = 0;

        public DataStack( int max )
        {
            _values = new Value[max];

            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = Value.Nil;
            }
        }

        public int Count
        {
            get { return _count; }
            set
            {
                // 设置到希望的高度

                for (int i = value; i < _count; i++)
                {
                    _values[i] = Value.Nil;
                }

                _count = value;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _values.Length;i++ )
            {
                _values[i] = Value.Nil;
            }

            _count = 0;
        }

        public void Push( Value v )
        {

            _values[_count] = v;
            _count++;
        }

        public Value Pop( )
        {
            var v = _values[_count - 1];

            // 调试功能, 让栈看起来清爽
            _values[_count - 1] = Value.Nil;

            _count--;

            return v;
        }

        public void PopMulti( int count )
        {
            // 调试功能, 让栈看起来清爽
            for( int i = 1;i<= count;i++)
            {
                _values[_count - i] = Value.Nil;
            }

            _count -= count;
        }

        // preTop + 废数据 + 有效数据,  将废弃部分去掉, 有效部分前移覆盖
        public void Cut( int preTop, int validCount )
        {
            int validBegin = Count - validCount;

            for (int i = 0; i < validCount; i++)
            {
                _values[preTop + i] = _values[validBegin + i];
            }

            _count = preTop + validCount;
        }

        public Value Get( int index = -1 )
        {
            if ( index >=0 )
            {
                return _values[index];
            }

            var final = _count + index;

            if (final >= _values.Length || final < 0)
                return Value.Nil;


            return _values[final];
        }

        public void DebugPrint( )
        {            
            for( int i = 0;i < _count;i++)
            {
 
                var v = _values[i];

                Debug.WriteLine("[stack] {0}: {1}", i, v.ToString());
            }
        }
    }
}
