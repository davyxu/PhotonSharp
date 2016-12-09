using Photon;

namespace UnitTest
{
    partial class Program
    {
        static void TestBasic()
        {
            new TestBox().RunFile("Math.pho")
                .CheckGlobalVarMatchValue("a", -1)
                .CheckGlobalVarMatchValue("b", (System.Int64)5)
                .CheckGlobalVarMatchValue("c", 3.0f)
                .CheckGlobalVarMatchValue("s", "hello world");

            new TestBox().RunFile("SwapVar.pho")
                .CheckGlobalVarMatchValue("x", 2)
                .CheckGlobalVarMatchValue("y", 1);

        }
    }
}
