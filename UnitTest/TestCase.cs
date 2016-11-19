

using Photon;
namespace UnitTest
{
    partial class Program
    {

        static void TestCase()
        {
            new TestBox().RunFile("ForLoop.pho").TestGlobalRegEqualNumber(0, 8);

            //new TestBox().RunFile("Package.pho");

            //new TestBox().RunFile("Test.pho");

            new TestBox().RunFile("MultiCall.pho").TestGlobalRegEqualNumber(0, 15);

           // new TestBox().RunFile("Closure.pho").TestGlobalRegEqualNumber(1, 12);

           // new TestBox().RunFile("Array.pho").TestGlobalRegEqualNumber(2, 1);


            //{
            //    var tb = new TestBox().CompileFile("Delegate.pho");
            //    tb.Exe.RegisterDelegate("add", (vm) =>
            //    {
            //        var a = vm.DataStack.Get(-1).CastNumber();
            //        var b = vm.DataStack.Get(-2).CastNumber();

            //        vm.DataStack.Push(new ValueNumber(a + b));

            //        return 1;
            //    });

            //    tb.Run().TestStackClear().TestGlobalRegEqualNumber(1, 3 );
            //}

            

            new TestBox().RunFile("Scope.pho");

            new TestBox().RunFile("DataStackBalance.pho");
            new TestBox().RunFile("ForLoop.pho").TestGlobalRegEqualNumber(0, 8);
            new TestBox().RunFile("If.pho").TestGlobalRegEqualNumber(0, 1).TestGlobalRegEqualNumber(1, 5);
            new TestBox().RunFile("MultiCall.pho").TestGlobalRegEqualNumber(0, 15);
            new TestBox().RunFile("SwapVar.pho").TestGlobalRegEqualNumber(0, 2).TestGlobalRegEqualNumber(1, 1);
            new TestBox().RunFile("WhileLoop.pho").TestGlobalRegEqualNumber(0, 3);
        }
    }
}
