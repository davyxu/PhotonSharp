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

            TestBasic();

            TestDataStackBalance();

            TestFuncPackage();

            TestDelegateExecute();

            TestFlow();

            TestClass();

            TestNativeClass();

            TestContainer();
        }
    }
}
