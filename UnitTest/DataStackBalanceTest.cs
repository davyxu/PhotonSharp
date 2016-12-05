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
            //WrapperCodeGenerator.GenerateClass(typeof(DataStackBalanceTest), "UnitTest", "../UnitTest/DataStackBalanceTestWrapper.cs");

            var testbox = new TestBox();
            testbox.Exe.RegisterNativeClass(Assembly.GetEntryAssembly(),
                "UnitTest.DataStackBalanceTestWrapper",
                "DataStackBalanceTest");

            testbox.RunFile("DataStackBalance.pho")
                .TestGlobalRegEqualNumber(0, 2)
                .TestGlobalRegEqualNumber(1, 4)
                .TestGlobalRegEqualNil(2)
                .TestGlobalRegEqualNumber(3, 3)
                .TestGlobalRegEqualNumber(4, 1)
                .TestGlobalRegEqualNumber(5, 9)
                .TestGlobalRegEqualNil(6)
                .TestGlobalRegEqualNumber(7, 2)
                .TestGlobalRegEqualNumber(8, 4);  
        }
    }
}
