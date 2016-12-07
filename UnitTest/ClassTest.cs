using Photon;

namespace UnitTest
{
    partial class Program
    {
        static void TestClass()
        {
            new TestBox().RunFile("Class.pho")
                .CheckGlobalVarMatchKind("a", ValueKind.ClassInstance)
                .CheckGlobalVarMatchValue("h", 5)
                .CheckGlobalVarMatchValue("n", "cat");

            new TestBox().RunFile("ClassInherit.pho")
                .CheckGlobalVarMatchKind("c", ValueKind.ClassInstance)
                .CheckGlobalVarMatchValue("k", "cat");                
        }
    }
}
