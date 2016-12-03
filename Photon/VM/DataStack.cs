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

        // preTop + 废数据 + 有效数据,  将废弃部分去掉, 有效部分前移覆盖
        internal void AdjustNative( int preTop, int validCount )
        {
            int validBegin = Count - validCount;

            for (int i = 0; i < validCount; i++)
            {
                // 手工填充习惯按声明顺序进行返回, 但是栈里要倒过来
                _values[preTop + i] = _values[ validBegin + validCount - i - 1 ];
            }

            _count = preTop + validCount;
        }

        internal void AdjustPho( int expect )
        {
            // 收的多, 返回的变量少
            if ( expect <= _count )
            {
                // 从期望的到最终数量间填充nil
                for (int i = expect; i < _count; i++)
                {
                    _values[i] = Value.Nil;
                }
            }
            else
            { // 收的少, 返回的变量多, 在当前返回量前填充nil
                
                int padding = expect - _count;

                for (int i = _count - 1; i < expect - 1; i++)
                {
                    // 将老值放到更高的位置
                    _values[i + padding] = _values[i];

                    // 前边(右边)留空
                    _values[i] = Value.Nil;
                }
            }


            _count = expect;
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
