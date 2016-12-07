using Photon;
using System.Collections.Generic;

namespace UnitTest
{



    partial class Program
    {
        static void TestContainer()
        {
            {
                var testbox = new TestBox();

                testbox.RunFile("Map.pho")
                    .CheckGlobalVarMatchKind("map", ValueKind.NativeClassInstance)
                    .CheckGlobalVarMatchValue("c", 1678);
            }

            {
                var testbox = new TestBox();

                testbox.RunFile("Array.pho")
                    .CheckGlobalVarMatchKind("arr", ValueKind.NativeClassInstance)
                    .CheckGlobalVarMatchValue("x", 120)
                    .CheckGlobalVarMatchValue("g", 120)
                    .CheckGlobalVarMatchValue("count", 1)
                    .CheckGlobalVarMatchKind("v", ValueKind.Nil)
                    .CheckGlobalVarMatchValue("ok", false);
            }
            
        }
    }
}
