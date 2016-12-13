
namespace Photon
{

    class ValueNativeFunc : ValueFunc
    {
        NativeFunction _data;

        internal ValueNativeFunc(ObjectName name)
            : base( name )
        {
            
        }

        internal ValueNativeFunc(ObjectName name, NativeFunction entry)
            : base(name)
        {
            _data = entry;
        }

        public override bool Equals(object other)
        {
            var otherT = other as ValueNativeFunc;
            if (otherT == null)
                return false;

            return otherT._data == _data;
        }

        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }

        internal override object Raw
        {
            get { return _data; }
        }

        public override string DebugString()
        {
            return string.Format("{0} id: {1}", Name, ID);
        }

        internal override bool Invoke(VMachine vm, int argCount, int receiverCount, ValueClosure closure)
        {
            // 外部调用不进行栈调整
            var preTop = vm.DataStack.Count - argCount;

            if (_data != null)
            {
                // 暂时不使用返回值， 可以通过前后top差判断出来
                _data(vm);
            }

            // 手工push值是按从左到右顺序， 和栈顺序反的， 需要倒置
            vm.DataStack.Reverse(preTop);

            // 去掉输入参数
            vm.DataStack.PopMulti(argCount);

            // 调整返回参
            vm.DataStack.Adjust(preTop, receiverCount );

            return true;
        }
    }

}
