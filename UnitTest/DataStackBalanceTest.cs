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
                .CheckGlobalRegEqualNumber(0, 2)
                .CheckGlobalRegEqualNumber(1, 4)
                .CheckGlobalRegEqualNil(2)
                .CheckGlobalRegEqualNumber(3, 3)
                .CheckGlobalRegEqualNumber(4, 1)
                .CheckGlobalRegEqualNumber(5, 9)
                .CheckGlobalRegEqualNil(6)
                .CheckGlobalRegEqualNumber(7, 2)
                .CheckGlobalRegEqualNumber(8, 4);  
        }
    }
}
