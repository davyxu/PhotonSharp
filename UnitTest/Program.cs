using System.Text;
using Photon.Parser;
using Photon.AST;
using Photon.Compiler;
using Photon.VM;
using System.Diagnostics;

namespace UnitTest
{
    partial class Program
    {


        static void Main(string[] args)
        {

            var parser = new Parser();

            parser.Init(@"
func foo( a, b ){
    return a + b
}

var x = 1

var y = x + foo( 1, 2 )

");

            var chunk = parser.ParseChunk();
            Parser.DebugPrint(chunk );

            var compiler = new Compiler();
            var exe = compiler.Walk(chunk);
            exe.DebugPrint();

            

            var vm = new VMachine();
            vm.Run(exe);
            vm.DebugPrint();

        }
    }
}
