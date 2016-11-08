using Photon.VM;
using Photon.Model;
using System.IO;
namespace UnitTest
{
    partial class Program
    {

        static void TestCase()
        {
            new TestBox().RunFile("Closure.pho");

            new TestBox().RunFile("Array.pho").TestGlobalRegEqualNumber(2, 1);


            {
                var tb = new TestBox().CompileFile("Delegate.pho");
                tb.Script.RegisterDelegate("add", (vm) =>
                {
                    var a = VMachine.CastNumber(vm.Stack.Get(-1));
                    var b = VMachine.CastNumber(vm.Stack.Get(-2));
                    
                    vm.Stack.Push(new ValueNumber(a + b));

                    return 1;
                });

                tb.Run().TestStackClear().TestGlobalRegEqualNumber(1, 3 );
            }

            new TestBox().RunFile("Test.pho");

            new TestBox().RunFile("Scope.pho");

            new TestBox().RunFile("DataStackBalance.pho");
            new TestBox().RunFile("ForLoop.pho").TestGlobalRegEqualNumber(0, 8).TestLocalRegEqualNumber(0, 3);
            new TestBox().RunFile("If.pho").TestGlobalRegEqualNumber(0, 1).TestGlobalRegEqualNumber(1, 5);
            new TestBox().RunFile("MultiCall.pho").TestGlobalRegEqualNumber(2, 5);
            new TestBox().RunFile("SwapVar.pho").TestGlobalRegEqualNumber(0, 2).TestGlobalRegEqualNumber(1, 1);
            new TestBox().RunFile("WhileLoop.pho").TestGlobalRegEqualNumber(0, 3);
        }
    }
}
