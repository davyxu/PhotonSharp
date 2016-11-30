
namespace Photon
{

    class Delegate : Procedure
    {
        public DelegateEntry Entry;

        internal Delegate(ObjectName name)
            : base( name )
        {
            
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }

        internal override bool Invoke(VMachine vm, int argCount, bool balanceStack, ValueClosure closure)
        {
            // 外部调用不进行栈调整
            var stackBeforeCall = vm.DataStack.Count;

            int retValueCount = 0;

            if (Entry != null)
            {
                retValueCount = Entry(vm);
            }

            // 调用结束时需要平衡栈( 返回值没有被用到 )
            if (balanceStack)
            {
                // 调用前(包含参数+ delegate)
                vm.DataStack.Count = stackBeforeCall - argCount;
            }
            else
            {
                vm.DataStack.Cut(stackBeforeCall - argCount, retValueCount);
            }

            return true;
        }
    }

}
