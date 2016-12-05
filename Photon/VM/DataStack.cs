using System;
using System.Diagnostics;

namespace Photon
{
    public class DataStack : DataAccessor
    {
        Value[] _values;
        int _count = 0;

        internal DataStack(int max)
        {
            _values = new Value[max];

            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = Value.Nil;
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

        public void PushFloat32( float v )
        {
            Push(new ValueNumber(v));
        }

        public void PushInteger32(Int32 v)
        {
            Push(new ValueNumber((float)v));
        }

        public void PushString(string v)
        {
            Push(new ValueString(v));
        }

        public void PushNil( )
        {
            Push(new ValueNil());
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
