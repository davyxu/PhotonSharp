using Photon;

namespace UnitTest
{
    partial class Program
    {
        static bool GenAllFile;

        static void TestCase()
        {
            if (GenAllFile)
            {
                Compiler.InitBuildin(); 
            }

            TestArray();

            TestNativeClass();

            TestDelegate();

            TestDataStackBalance();


            new TestBox().RunFile("ClassInherit.pho")
                .CheckGlobalRegEqualString(1, "cat");

            new TestBox().RunFile("Class.pho")
                .CheckGlobalRegEqualNumber(1, 5);

            new TestBox().RunFile("Math.pho")
                .CheckGlobalRegEqualNumber(0, -1);

            new TestBox().RunFile("ComplexClosure.pho")
                .CheckGlobalRegEqualNumber(2, 15 );

            new TestBox().RunFile("Package.pho")
                .CheckGlobalRegEqualNumber(0, 3);

            new TestBox().RunFile("Closure.pho")
                .CheckGlobalRegEqualNumber(1, 12);

            new TestBox().RunFile("Scope.pho")
                .CheckGlobalRegEqualNumber(0, 1)
                .CheckGlobalRegEqualNumber(1, 1);
            
            new TestBox().RunFile("ForLoop.pho")
                .CheckGlobalRegEqualNumber(0, 8);

            new TestBox().RunFile("If.pho")
                .CheckGlobalRegEqualNumber(0, 1)
                .CheckGlobalRegEqualNumber(1, 5);

            new TestBox().RunFile("MultiCall.pho")
                .CheckGlobalRegEqualNumber(0, 15);

            new TestBox().RunFile("SwapVar.pho")
                .CheckGlobalRegEqualNumber(0, 2)
                .CheckGlobalRegEqualNumber(1, 1);

            new TestBox().RunFile("WhileLoop.pho")
                .CheckGlobalRegEqualNumber(0, 3);

        }
    }
}
