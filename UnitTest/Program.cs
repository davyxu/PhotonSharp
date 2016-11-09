

using System;
namespace UnitTest
{
    partial class Program
    {
        static void x()
        {

        }
        static int foo()
        {
            int a = 1;
            int b = 2;

            x();

            int c = 3;

            return a + b + c;
        }

        static void Main(string[] args)
        {
            foo();

            TestCase();
        }
    }
}
