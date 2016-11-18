

using Photon;
namespace UnitTest
{
    partial class Program
    {

        static void TestCase()
        {
            //new TestBox().RunFile("Package.pho");

           // new TestBox().RunFile("Test.pho");

            //new TestBox().RunFile("MultiCall.pho").TestLocalRegEqualNumber(0, 15);

            new TestBox().RunFile("Closure.pho").TestLocalRegEqualNumber(1, 12 );

           // new TestBox().RunFile("Array.pho").TestLocalRegEqualNumber(2, 1);


            //{
            //    var tb = new TestBox().CompileFile("Delegate.pho");
            //    tb.Exe.RegisterDelegate("add", (vm) =>
            //    {
            //        var a = vm.DataStack.Get(-1).CastNumber();
            //        var b = vm.DataStack.Get(-2).CastNumber();

            //        vm.DataStack.Push(new ValueNumber(a + b));

            //        return 1;
            //    });

            //    tb.Run().TestStackClear().TestLocalRegEqualNumber(1, 3 );
            //}

            

            new TestBox().RunFile("Scope.pho");

            new TestBox().RunFile("DataStackBalance.pho");
            new TestBox().RunFile("ForLoop.pho").TestLocalRegEqualNumber(0, 8);
            new TestBox().RunFile("If.pho").TestLocalRegEqualNumber(0, 1).TestLocalRegEqualNumber(1, 5);
            new TestBox().RunFile("MultiCall.pho").TestLocalRegEqualNumber(0, 15);
            new TestBox().RunFile("SwapVar.pho").TestLocalRegEqualNumber(0, 2).TestLocalRegEqualNumber(1, 1);
            new TestBox().RunFile("WhileLoop.pho").TestLocalRegEqualNumber(0, 3);
        }
    }
}
