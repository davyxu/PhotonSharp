
namespace Photon
{

    class ValueNativeFunc : ValueFunc
    {
        NativeDelegate _entry;

        internal ValueNativeFunc(ObjectName name)
            : base( name )
        {
            
        }

        internal ValueNativeFunc(ObjectName name, NativeDelegate entry)
            : base(name)
        {
            _entry = entry;
        }

        public override string ToString()
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

            vm.DataStack.Adjust(preTop, receiverCount, true );

            return true;
        }
    }

}
