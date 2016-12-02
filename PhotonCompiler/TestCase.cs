using Photon;
using System;
namespace PhotonCompiler
{
    public class MyMath
    {
        // TODO 在特性上标注进入参数, 在编译期检查
        [NativeEntry]
        public static int AddValue( VMachine vm )
        {                        
            var a = vm.DataStack.GetFloat32(-1);
            var b = vm.DataStack.GetFloat32(-2);

            vm.DataStack.PushFloat32(a + b);            

            return 1;
        }
    }

    class Animal
    {
        [NativeEntry]
        public int foo(VMachine vm)
        {
            var a = vm.DataStack.GetFloat32(-1);

            vm.DataStack.PushString("animal");

            return 1;
        }
    }

    class Cat : Animal
    {
        //public override string foo()
        //{
        //    return "cat";
        //}

        [NativeEntry]
        public int foo(VMachine vm)
        {
            var a = vm.DataStack.GetFloat32(-1);            

            vm.DataStack.PushFloat32(a);

            vm.DataStack.PushString("cat");

            return 2;
        }
    }


    partial class Program
    {

        static void TestCase()
        {

            {
                var testbox = new TestBox();

                testbox.Exe.RegisterNativeClass(typeof(MyMath), "DelegateTest");
                testbox.Exe.RegisterNativeClass(typeof(Cat), "DelegateTest");

                testbox.RunFile("Delegate.pho").TestGlobalRegEqualNumber(0, 3).TestGlobalRegEqualNumber(2, 2016).TestGlobalRegEqualString(3, "cat");
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
