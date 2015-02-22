using System.Text;
using Photon.Parser;
using Photon.AST;
using System.Diagnostics;

namespace UnitTest
{
    partial class Program
    {
        static void PrintAST( Node n, string indent )
        {          
            Debug.WriteLine(indent + n.ToString());

            foreach( var c in n.Child() )
            {
                PrintAST(c, indent + "\t" );
            }
        }


        static void Main(string[] args)
        {
            var parser = new Parser();

            parser.Init(@"
func foo( a, b ){
    a = b + 1
}


");

            var r = parser.Dummy();
            PrintAST(r, "");

        }
    }
}
