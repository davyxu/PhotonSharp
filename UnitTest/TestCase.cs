using Photon;
using System.IO;
using MarkSerializer;

namespace UnitTest
{
    partial class Program
    {
        static bool GenAllFile = false;

        static void TestCase()
        {

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
