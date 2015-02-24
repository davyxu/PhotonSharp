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

            var src = @"
func mul( a, b ){
    return a * b
}

func foo( a, b ){


    return a + mul( b, 2 )
}


var y = foo( 1, 2 )

";
            Debug.WriteLine(src);

            parser.Init( src );

            var chunk = parser.ParseChunk();
            Parser.DebugPrint(chunk);

            var compiler = new Compiler();
            var exe = compiler.Walk(chunk, parser.ScopeInfoSet );
            exe.DebugPrint();

            Debug.WriteLine("");

            var vm = new VMachine();
            vm.Run(exe);
            vm.DebugPrint();

        }
    }
}
