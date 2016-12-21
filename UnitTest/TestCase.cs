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
            //    Executable.Serialize(f, ref a, false);                
            //    f.Close();

            //    var vm = new VMachine();
            //    vm.ShowDebugInfo = true;

            //    vm.Execute(a);
            //}


            //using (FileStream f = new FileStream("ser.bin", FileMode.Open))
            //{
            //    Executable newa = new Executable();
            //    Executable.Serialize(f, ref newa, true);
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
