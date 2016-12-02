using Photon;
using System;
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

    class Cat
    {
        public string realFoo( int a )
        {
            return "cat";
        }

        [NativeEntry(NativeEntryType.ClassMethod, "foo")]
        public static int VMFoo(VMachine vm )
        {
            var instance = vm.DataStack.GetNativeInstance<Cat>(0);
            var a = vm.DataStack.GetFloat32(1);

            var str = instance.realFoo((int)a);

            vm.DataStack.PushString(str);

            return 1;
        }
    }


    partial class Program
    {

        static void TestCase()
        {

            {
                var testbox = new TestBox();

                testbox.Exe.RegisterNativeClass(typeof(DelegateTest), "DelegateTest");
                testbox.Exe.RegisterNativeClass(typeof(Cat), "DelegateTest");

                testbox.RunFile("Delegate.pho").TestGlobalRegEqualNumber(0, 3).TestGlobalRegEqualString(2, "cat");
            }
            

            new TestBox().RunFile("ClassInherit.pho").TestGlobalRegEqualString(1, "cat");

            new TestBox().RunFile("Class.pho").TestGlobalRegEqualNumber(1, 5);
            new TestBox().RunFile("Math.pho").TestGlobalRegEqualNumber(0, -1);



            new TestBox().RunFile("ComplexClosure.pho").TestGlobalRegEqualNumber(1, 15 );
            new TestBox().RunFile("Package.pho").TestGlobalRegEqualNumber(0, 3);
            new TestBox().RunFile("Closure.pho").TestGlobalRegEqualNumber(1, 12);
            new TestBox().RunFile("Scope.pho").TestGlobalRegEqualNumber(0, 1).TestGlobalRegEqualNumber(1, 1);
            new TestBox().RunFile("DataStackBalance.pho");
            new TestBox().RunFile("ForLoop.pho").TestGlobalRegEqualNumber(0, 8);
            new TestBox().RunFile("If.pho").TestGlobalRegEqualNumber(0, 1).TestGlobalRegEqualNumber(1, 5);
            new TestBox().RunFile("MultiCall.pho").TestGlobalRegEqualNumber(0, 15);
            new TestBox().RunFile("SwapVar.pho").TestGlobalRegEqualNumber(0, 2).TestGlobalRegEqualNumber(1, 1);
            new TestBox().RunFile("WhileLoop.pho").TestGlobalRegEqualNumber(0, 3);
        }
    }
}
