using Photon;
using System.Collections.Generic;

namespace UnitTest
{



    partial class Program
    {
        static void TestArray()
        {
            var testbox = new TestBox();
            //testbox.Exe.RegisterNativeClass(typeof(Builtin.ArrayWrapper), "Builtin");

            testbox.RunFile("Array.pho");
        }
    }
}
