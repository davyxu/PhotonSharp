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
                    .CheckGlobalVarMatchValue("x", 1678)
                    .CheckGlobalVarMatchValue("c", 1);
            }

            {
                var testbox = new TestBox();

                testbox.RunFile("Array.pho")
                    .CheckGlobalVarMatchKind("arr", ValueKind.NativeClassInstance)
                    .CheckGlobalVarMatchValue("x", 120)
                    .CheckGlobalVarMatchValue("g", 120)
                    .CheckGlobalVarMatchValue("count", 1)
                    .CheckGlobalVarMatchKind("v", ValueKind.Nil)
                    .CheckGlobalVarMatchValue("ok", false)
                    .CheckGlobalVarMatchKind("sugar", ValueKind.NativeClassInstance)
                    .CheckGlobalVarMatchValue("sugarlen", 2)
                    .CheckGlobalVarMatchValue("sugar1", 1)
                    .CheckGlobalVarMatchValue("sugar2", 2);
            }
            
        }
    }
}
