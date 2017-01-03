using Photon;

namespace UnitTest
{
    partial class Program
    {
        static void TestFlow()
        {
            new TestBox().RunFile("ForLoop.pho")
                .CheckGlobalVarMatchValue("m", 2)
                .CheckGlobalVarMatchValue("n", 3)
                .CheckGlobalVarMatchValue("p", 2)
                .CheckGlobalVarMatchValue("q", 8)
                .CheckGlobalVarMatchValue("larr", 2)
                .CheckGlobalVarMatchValue("arrm1", 1)
                .CheckGlobalVarMatchValue("arrm2", 2)
                .CheckGlobalVarMatchValue("ldict", 2)
                .CheckGlobalVarMatchValue("dictm1", 12345)
                .CheckGlobalVarMatchValue("dictm2", 100);

            new TestBox().RunFile("If.pho")
                .CheckGlobalVarMatchValue("x", 1)
                .CheckGlobalVarMatchValue("y", 5);

            new TestBox().RunFile("WhileLoop.pho")
                .CheckGlobalVarMatchValue("x", 3);
        }
    }
}
