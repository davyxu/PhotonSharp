using Photon;
using System.IO;

namespace UnitTest
{
    partial class Program
    {
        static bool GenAllFile = false;

        static void TestCase()
        {
            Directory.SetCurrentDirectory("../../../TestCase");

            if (GenAllFile)
            {
                Compiler.GenerateBuiltinFiles(); 
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
