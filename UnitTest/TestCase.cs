using Photon;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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

            //Executable a = Compiler.CompileFile("Constant.pho");

            //using (FileStream f = new FileStream("ser.bin", FileMode.Create))
            //{                
            //    a.Serialize(new Photon.BinarySerializer(f, false));
            //    f.Close();
            //}

            //using (FileStream f = new FileStream("ser.bin", FileMode.Open))
            //{
            //    Executable newa = new Executable();
            //    newa.Serialize(new Photon.BinarySerializer(f, true));
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
