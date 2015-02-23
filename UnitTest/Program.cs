using System.Text;
using Photon.Parser;
using Photon.AST;
using Photon.Compiler;
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
    return a + b
    // LoadR Rb+0; S[3] = R[1]
    // LoadR Rb+1; S[4] = R[2]
    // Add       ; S[3] = S[3] + S[4]   1+2
}

var x = 1
// LoadC 1   ; S[0] = C[1]  1
// SetR 0    ; R[0] = S[0]  x = 1


var y = x + foo( 1, 2 )
// LoadR 0   ; S[0] = R[0]  1
// LoadC 0   ; S[1] = C[0]  foo
// LoadR 1   ; S[2] = C[1]  1
// LoadC 1   ; S[3] = C[2]  2
// Call      ; Rb = 1, R[1] = S[1] R[2]=S[2]  栈到寄存器, 将参数转为寄存器
// Ret       ; Rb = 0, 调整栈 S[1] = 3
// Add       ; S[0] = S[0] + S[1]  1+3
// SetR 1    ; R[1] = S[0]

");
            // C[0] = foo
            // C[1] = 1
            // C[2] = 2

            var chunk = parser.ParseChunk();
            PrintAST(chunk, "");

            var compiler = new Compiler();
            var exe = compiler.Walk(chunk);
            exe.DebugPrint();

        }
    }
}
