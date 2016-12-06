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

                testbox.RunFile("Map.pho");
            }

            //{
            //    var testbox = new TestBox();

            //    testbox.RunFile("Array.pho");
            //}
            
        }
    }
}
