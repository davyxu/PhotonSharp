using Photon;
using System.Reflection;

namespace UnitTest
{
    class NativeClass
    {
        public string MyProp
        {
            get;
            set;
        }

        public string outAsRetValue(int a, string c, out int b)
        {
            b = 89;

            return "xx";
        }


        public string foo(int a)
        {
            return "cat";
        }

        public NativeClass()
        {
            MyProp = "HP";
        }

        // 手动额外绑定
        [NativeEntry(NativeEntryType.ClassMethod, "manualBinding")]
        public static int VMFoo(VMachine vm)
        {
            var instance = vm.DataStack.GetNativeInstance<NativeClass>(0);

            var a = vm.DataStack.GetInteger32(1);

            vm.DataStack.PushString("wa");

            return 1;
        }
    }


    partial class Program
    {
        static void TestNativeClass()
        {
            if (GenAllFile)
            {
                WrapperCodeGenerator.GenerateClass(typeof(NativeClass), "UnitTest", "../UnitTest/NativeClassWrapper.cs");
            }
            
            new TestBox().RegisterRunFile(delegate(Executable exe){
                exe.RegisterNativeClass(Assembly.GetExecutingAssembly(), "UnitTest.NativeClassWrapper", "NativeClassTest");
            },"NativeClass.pho")
                .CheckGlobalVarMatchKind("x", ValueKind.NativeClassInstance)
                .CheckGlobalVarMatchValue("b", "cat")
                .CheckGlobalVarMatchValue("c", 89)
                .CheckGlobalVarMatchValue("d", "xx")
                .CheckGlobalVarMatchValue("e", "wa")
                .CheckGlobalVarMatchValue("f", "HP");                
        }
    }
}
