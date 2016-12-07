
namespace Photon
{

    class ValueNativeFunc : ValueFunc
    {
        NativeFunction _entry;

        internal ValueNativeFunc(ObjectName name)
            : base( name )
        {
            
        }

        internal ValueNativeFunc(ObjectName name, NativeFunction entry)
            : base(name)
        {
            _entry = entry;
        }

        public override string DebugString()
        {
            return string.Format("{0}", Name);
        }

        internal override bool Invoke(VMachine vm, int argCount, int receiverCount, ValueClosure closure)
        {
            // 外部调用不进行栈调整
            var preTop = vm.DataStack.Count - argCount;

            if (_entry != null)
            {
                // 暂时不使用返回值， 可以通过前后top差判断出来
                _entry(vm);
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
