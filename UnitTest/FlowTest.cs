using Photon;

namespace UnitTest
{
    partial class Program
    {
        static void TestFlow()
        {
            new TestBox().RunFile("ForLoop.pho")
                .CheckGlobalVarMatchValue("n", 2)
                .CheckGlobalVarMatchValue("m", 3)
                .CheckGlobalVarMatchValue("y", 2)                
                .CheckGlobalVarMatchValue("x", 8)                
                .CheckGlobalVarMatchValue("l", 2)
                .CheckGlobalVarMatchValue("z", 1)
                .CheckGlobalVarMatchValue("zz", 2);

            new TestBox().RunFile("If.pho")
                .CheckGlobalVarMatchValue("x", 1)
                .CheckGlobalVarMatchValue("y", 5);

            new TestBox().RunFile("WhileLoop.pho")
                .CheckGlobalVarMatchValue("x", 3);
        }
    }
}
