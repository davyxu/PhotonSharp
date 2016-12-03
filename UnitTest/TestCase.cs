using Photon;
using System;
using System.Reflection;

namespace PhotonCompiler
{
    public class DelegateTest
    {
        // TODO 在特性上标注进入参数, 在编译期检查
        [NativeEntry( NativeEntryType.StaticFunc, "add")]
        public static int AddValue( VMachine vm )
        {                 
            
            var a = vm.DataStack.GetFloat32(-1);
            var b = vm.DataStack.GetFloat32(-2);

            vm.DataStack.PushFloat32(a + b);            

            return 1;
        }
    }

    public class DataStackBalanceTest
    {        
        public static int native_less()
        {
            return 9;
        }

        public static void native_more(out int out1, out int out2)
        {
            out1 = 2;
            out2 = 4;
        }
    }

    class Cat
    {
        public string xx(int a, string c, out int b)
        {
            b = 89;

            return "xx";
        }


        public string foo( int a )
        {
            return "cat";
        }

        // 手动额外绑定
        [NativeEntry(NativeEntryType.ClassMethod, "foo2")]
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

        static void TestCase()
        {

            //WrapperCodeGenerator.GenerateClass(typeof(Cat), "PhotonCompiler", "../UnitTest/CatWrapper.cs");
            //WrapperCodeGenerator.GenerateClass(typeof(DataStackBalanceTest), "PhotonCompiler", "../UnitTest/DataStackBalanceTestWrapper.cs");

            {
                var testbox = new TestBox();
                testbox.Exe.RegisterNativeClass(Assembly.GetEntryAssembly(), "PhotonCompiler.DataStackBalanceTestWrapper", "DataStackBalanceTest");

                testbox.RunFile("DataStackBalance.pho").TestGlobalRegEqualNumber(0, 2).TestGlobalRegEqualNumber(2, 4);
            }



            new TestBox().RunFile("ClassInherit.pho").TestGlobalRegEqualString(1, "cat");

            new TestBox().RunFile("Class.pho").TestGlobalRegEqualNumber(1, 5);
            new TestBox().RunFile("Math.pho").TestGlobalRegEqualNumber(0, -1);



            new TestBox().RunFile("ComplexClosure.pho").TestGlobalRegEqualNumber(1, 15 );
            new TestBox().RunFile("Package.pho").TestGlobalRegEqualNumber(0, 3);
            new TestBox().RunFile("Closure.pho").TestGlobalRegEqualNumber(1, 12);
            new TestBox().RunFile("Scope.pho").TestGlobalRegEqualNumber(0, 1).TestGlobalRegEqualNumber(1, 1);
            
            new TestBox().RunFile("ForLoop.pho").TestGlobalRegEqualNumber(0, 8);
            new TestBox().RunFile("If.pho").TestGlobalRegEqualNumber(0, 1).TestGlobalRegEqualNumber(1, 5);
            new TestBox().RunFile("MultiCall.pho").TestGlobalRegEqualNumber(0, 15);
            new TestBox().RunFile("SwapVar.pho").TestGlobalRegEqualNumber(0, 2).TestGlobalRegEqualNumber(1, 1);
            new TestBox().RunFile("WhileLoop.pho").TestGlobalRegEqualNumber(0, 3);


            




            {
                var testbox = new TestBox();

                testbox.Exe.RegisterNativeClass(typeof(DelegateTest), "DelegateTest");
                testbox.Exe.RegisterNativeClass(typeof(CatWrapper), "DelegateTest");

                testbox.RunFile("Delegate.pho").TestGlobalRegEqualNumber(0, 3).TestGlobalRegEqualString(2, "cat");
            }
            
        }
    }
}
