using Photon;
using System;
using System.Reflection;

namespace UnitTest
{



    partial class Program
    {
        static void TestCase()
        {

            TestDelegate();

            TestDataStackBalance();


            new TestBox().RunFile("ClassInherit.pho")
                .TestGlobalRegEqualString(1, "cat");

            new TestBox().RunFile("Class.pho")
                .TestGlobalRegEqualNumber(1, 5);

            new TestBox().RunFile("Math.pho")
                .TestGlobalRegEqualNumber(0, -1);

            new TestBox().RunFile("ComplexClosure.pho")
                .TestGlobalRegEqualNumber(2, 15 );

            new TestBox().RunFile("Package.pho")
                .TestGlobalRegEqualNumber(0, 3);

            new TestBox().RunFile("Closure.pho")
                .TestGlobalRegEqualNumber(1, 12);

            new TestBox().RunFile("Scope.pho")
                .TestGlobalRegEqualNumber(0, 1)
                .TestGlobalRegEqualNumber(1, 1);
            
            new TestBox().RunFile("ForLoop.pho")
                .TestGlobalRegEqualNumber(0, 8);

            new TestBox().RunFile("If.pho")
                .TestGlobalRegEqualNumber(0, 1)
                .TestGlobalRegEqualNumber(1, 5);

            new TestBox().RunFile("MultiCall.pho")
                .TestGlobalRegEqualNumber(0, 15);

            new TestBox().RunFile("SwapVar.pho")
                .TestGlobalRegEqualNumber(0, 2)
                .TestGlobalRegEqualNumber(1, 1);

            new TestBox().RunFile("WhileLoop.pho")
                .TestGlobalRegEqualNumber(0, 3);

        }
    }
}
