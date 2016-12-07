using Photon;

namespace UnitTest
{
    partial class Program
    {
        static bool GenAllFile = false;

        static void TestCase()
        {
            if (GenAllFile)
            {
                Compiler.GenerateBuildinFiles(); 
            }

            new TestBox().RunFile("Math.pho")
                .CheckGlobalVarMatchValue("a", -1);

            new TestBox().RunFile("SwapVar.pho")
                .CheckGlobalVarMatchValue("x", 2)
                .CheckGlobalVarMatchValue("y", 1);

            TestDataStackBalance();

            TestFuncPackage();

            TestDelegate();

            TestFlow();

            TestClass();

            TestNativeClass();

            TestContainer();
        }
    }
}
