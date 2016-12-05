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



    class Cat
    {
        public string outAsRetValue(int a, string c, out int b)
        {
            b = 89;

            return "xx";
        }


        public string foo(int a)
        {
            return "cat";
        }

        // 手动额外绑定
        [NativeEntry(NativeEntryType.ClassMethod, "manualBinding")]
        public static int VMFoo(VMachine vm)
        {
            var instance = vm.DataStack.GetNativeInstance<Cat>(0);

            var a = vm.DataStack.GetInteger32(1);

            vm.DataStack.PushString("wa");

            return 1;
        }
    }


    partial class Program
    {
        static void TestDelegate()
        {
            //WrapperCodeGenerator.GenerateClass(typeof(Cat), "UnitTest", "../UnitTest/DelegateTestWrapper.cs");

            var testbox = new TestBox();

            testbox.Exe.RegisterNativeClass(typeof(DelegateTest), "DelegateTest");
            testbox.Exe.RegisterNativeClass(typeof(CatWrapper), "DelegateTest");

            testbox.RunFile("Delegate.pho")
                .TestGlobalRegEqualNumber(0, 3)
                .TestGlobalRegEqualString(2, "cat");       
        }
    }
}
