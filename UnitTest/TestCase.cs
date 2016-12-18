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

            //Executable a = Compiler.CompileFile("Constant.pho");

            //using (FileStream f = new FileStream("ser.bin", FileMode.Create))
            //{
            //    Executable.Serialize(a, f);                
            //    f.Close();
            //}


            //using (FileStream f = new FileStream("ser.bin", FileMode.Open))
            //{
            //    var newa = Executable.Deserialize(f);
            //    f.Close();

            //    var vm = new VMachine();
            //    vm.ShowDebugInfo = true;

            //    vm.Execute(newa);
            //}


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
