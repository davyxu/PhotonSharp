using Photon;

namespace UnitTest
{
    partial class Program
    {
        static void TestFlow()
        {
            new TestBox().RunFile("ForLoop.pho")
                .CheckGlobalVarMatchValue("x", 8)
                .CheckGlobalVarMatchValue("i", 3);

            new TestBox().RunFile("If.pho")
                .CheckGlobalVarMatchValue("x", 1)
                .CheckGlobalVarMatchValue("y", 5);

            new TestBox().RunFile("WhileLoop.pho")
                .CheckGlobalVarMatchValue("x", 3);
        }
    }
}
