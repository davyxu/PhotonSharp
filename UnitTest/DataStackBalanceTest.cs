using Photon;
using System.Reflection;

namespace UnitTest
{
    public class DataStackBalanceTest
    {
        public static int native_less()
        {
            return 9;
        }

        public static void native_more(out int out1, out int out2, out int out3)
        {
            out1 = 2;
            out2 = 4;
            out3 = 6;
        }
    }


    partial class Program
    {

        static void TestDataStackBalance()
        {
            if (GenAllFile)
            {
                WrapperCodeGenerator.GenerateClass(typeof(DataStackBalanceTest), "UnitTest", "../UnitTest/DataStackBalanceTestWrapper.cs");
            }

            var testbox = new TestBox();
            testbox.Exe.RegisterNativeClass(Assembly.GetEntryAssembly(),
                "UnitTest.DataStackBalanceTestWrapper",
                "DataStackBalanceTest");

            testbox.RunFile("DataStackBalance.pho")
                .CheckGlobalVarMatchValue("a", 2)
                .CheckGlobalVarMatchValue("b", 4)
                .CheckGlobalVarMatchKind("c", ValueKind.Nil)
                .CheckGlobalVarMatchValue("e", 1)
                .CheckGlobalVarMatchValue("m", 9)
                .CheckGlobalVarMatchKind("n", ValueKind.Nil)
                .CheckGlobalVarMatchValue("p", 2)
                .CheckGlobalVarMatchValue("q", 4);                
        }
    }
}
