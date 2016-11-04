
using System.IO;
namespace UnitTest
{
    partial class Program
    {
        static void TestCase()
        {
            new TestBox().RunFile("Test.pho");

            new TestBox().RunFile("Scope.pho");

            new TestBox().RunFile("DataStackBalance.pho");
            new TestBox().RunFile("ForLoop.pho").TestGlobalRegEqual(0, 8).TestLocalRegEqual(0, 3);
            new TestBox().RunFile("If.pho").TestGlobalRegEqual(0, 1).TestGlobalRegEqual(1, 5);
            new TestBox().RunFile("MultiCall.pho").TestGlobalRegEqual(2, 5);
            new TestBox().RunFile("SwapVar.pho").TestGlobalRegEqual(0, 2).TestGlobalRegEqual(1, 1);
            new TestBox().RunFile("WhileLoop.pho").TestGlobalRegEqual(0, 3);
        }
    }
}
