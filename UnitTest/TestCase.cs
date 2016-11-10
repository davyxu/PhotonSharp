using Photon.VM;
using Photon.Model;
using System.IO;
namespace UnitTest
{
    partial class Program
    {

        static void TestCase()
        {        

            new TestBox().RunFile("Closure.pho").TestLocalRegEqualNumber(2, 12 );

            new TestBox().RunFile("Array.pho").TestLocalRegEqualNumber(2, 1);


            {
                var tb = new TestBox().CompileFile("Delegate.pho");
                tb.Script.RegisterDelegate("add", (vm) =>
                {
                    var a = VMachine.CastNumber(vm.Stack.Get(-1));
                    var b = VMachine.CastNumber(vm.Stack.Get(-2));
                    
                    vm.Stack.Push(new ValueNumber(a + b));

                    return 1;
                });

                tb.Run().TestStackClear().TestLocalRegEqualNumber(1, 3 );
            }

            new TestBox().RunFile("Test.pho");

            new TestBox().RunFile("Scope.pho");

            new TestBox().RunFile("DataStackBalance.pho");
            new TestBox().RunFile("ForLoop.pho").TestLocalRegEqualNumber(0, 8);
            new TestBox().RunFile("If.pho").TestLocalRegEqualNumber(0, 1).TestLocalRegEqualNumber(1, 5);
            new TestBox().RunFile("MultiCall.pho").TestLocalRegEqualNumber(2, 15);
            new TestBox().RunFile("SwapVar.pho").TestLocalRegEqualNumber(0, 2).TestLocalRegEqualNumber(1, 1);
            new TestBox().RunFile("WhileLoop.pho").TestLocalRegEqualNumber(0, 3);
        }
    }
}
