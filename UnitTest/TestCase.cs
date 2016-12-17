using Photon;
using System.IO;

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

            Executable a = Compiler.CompileFile("Constant.pho");

            using (FileStream f = new FileStream("ser.bin", FileMode.Create))
            {
                var bs = new BinarySerializer( f);
                bs.SerializeValue(typeof(Executable),a);
                f.Close();
            }

            using (FileStream f = new FileStream("ser.bin", FileMode.Open))
            {
                var bs = new BinaryDeserializer(f);
                var newa = bs.DeserializeValue<Executable>();
                f.Close();

                var vm = new VMachine();
                vm.ShowDebugInfo = true;

                vm.Execute(newa);
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
