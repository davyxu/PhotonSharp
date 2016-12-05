using Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class DelegateTest
    {
        // TODO 在特性上标注进入参数, 在编译期检查
        [NativeEntry(NativeEntryType.StaticFunc, "add")]
        public static int AddValue(VMachine vm)
        {

            var a = vm.DataStack.GetInteger32(0);
            var b = vm.DataStack.GetInteger32(1);

            vm.DataStack.PushInteger32(a + b);

            return 1;
        }
    }


    partial class Program
    {
        static void TestDelegate()
        {            
            var testbox = new TestBox();

            testbox.Exe.RegisterNativeClass(typeof(DelegateTest), "DelegateTest");

            testbox.RunFile("Delegate.pho")
                .TestGlobalRegEqualNumber(0, 3);                
        }
    }
}
