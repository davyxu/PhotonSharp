using System;
using System.Diagnostics;

namespace Photon
{
    public partial class DataStack : DataAccessor
    {
        Value[] _values;
        int _count = 0;

        internal DataStack( )
        {
            ValidCount(4);
        }

        void ValidCount(int count)
        {
            if (_values == null || count > _values.Length)
            {
                count = count * 2;

                var newValues = new Value[count];

                if ( _values == null )
                {
                    for (int i = 0; i < newValues.Length; i++)
                    {
                        newValues[i] = Value.Nil;
                    }
                }
                else
                {
                    System.Array.Copy(_values, newValues, _values.Length);

                    for (int i = _values.Length; i < newValues.Length; i++)
                    {
                        newValues[i] = Value.Nil;
                    }
                                        
                }

                _values = newValues;
            }
        }

        internal void Clear()
        {
            for (int i = 0; i < _values.Length;i++ )
            {
                _values[i] = Value.Nil;
            }

            _count = 0;
        }


        internal void Push(Value v)
        {
            ValidCount(_count + 1);

            _values[_count] = v;
            _count++;

            
        }

        internal override Value Get(int index)
        {
            if (index >= 0)
            {
                return _values[index];
            }

            var final = _count + index;

            if (final >= _values.Length || final < 0)
                return Value.Nil;


            return _values[final];
        }

        internal Value Pop()
        {
            if ( _count < 1)
            {
                return Value.Nil;
            }

            var v = _values[_count - 1];

            // 调试功能, 让栈看起来清爽
            _values[_count - 1] = Value.Nil;

            _count--;

            return v;
        }

        internal void PopMulti(int count)
        {
            // 调试功能, 让栈看起来清爽
            for( int i = 1;i<= count;i++)
            {
                _values[_count - i] = Value.Nil;
            }

            _count -= count;
        }

        internal void Reverse(int preTop )
        {
            int retCount = _count - preTop;

            for (int i = 0; i < retCount / 2; i++)
            {
                int a = preTop + i;
                int b = preTop + retCount - i - 1;

                var t = _values[a];
                _values[a] = _values[b];
                _values[b] = t;
            }
        }

        internal void Adjust(int preTop, int recvCount )
        {
            int retCount = _count - preTop;

            int finalTop = preTop + recvCount;

            _count = preTop + recvCount;

            // var a, b, c 对应  栈  -1, -2, -3

            // 返回数据的多, 接收的变量少
            if (retCount > recvCount)
            {                
                int nilCount = retCount - recvCount;

                // 将数据上移（往栈底）
                for (int i = 0; i < recvCount; i++)
                {
                    _values[preTop + i] = _values[preTop + nilCount + i];
                }
                
                // 在栈顶补Nil
                for (int i = 0; i < nilCount;i++ )
                {
                    _values[preTop + recvCount + i ] = Value.Nil;
                }
            }
            else if (retCount < recvCount)
            {   
                int nilCount = recvCount - retCount;

                // 将数据下移（往Top）
                for (int i = recvCount - 1; i >= 0 ; i--)
                {
                    _values[preTop + nilCount + i ] = _values[preTop + i];
                }

                // 在栈底补Nil
                for (int i = 0; i < nilCount; i++)
                {
                    _values[preTop + i] = Value.Nil;
                }
            }
        }

        public int Count
        {
            get { return _count; }
        }

        public void DebugPrint( )
        {            
            for( int i = 0;i < _count;i++)
            {
 
                var v = _values[i];

                Logger.DebugLine("[stack] {0}: {1}", i, v.ToString());
            }
        }
    }
}
