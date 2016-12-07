using Photon;

namespace UnitTest
{
    partial class Program
    {
        static void TestFuncPackage()
        {
            new TestBox().RunFile("Scope.pho")
                .CheckGlobalVarMatchValue("a", 1)
                .CheckGlobalVarMatchValue("c", 1);

            new TestBox().RunFile("MultiCall.pho")
                .CheckGlobalVarMatchValue("y", 15);

            new TestBox().RunFile("Closure.pho")
                .CheckGlobalVarMatchKind("c", ValueKind.Func)
                .CheckGlobalVarMatchValue("x", 12);

            new TestBox().RunFile("ComplexClosure.pho")
                .CheckGlobalVarMatchKind("c", ValueKind.Func)
                .CheckGlobalVarMatchKind("x", ValueKind.Func)
                .CheckGlobalVarMatchValue("y", 15);

            new TestBox().RunFile("Package.pho")
                .CheckGlobalVarMatchValue("c", 3);
        }
    }
}
