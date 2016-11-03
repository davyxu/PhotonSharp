
namespace UnitTest
{
    partial class Program
    {
        static void TestCase( )
        {
            (new TestBox("DataStackBalance")).Run(@"

func add( a, b ){
    return a+b
}

add(1, 2)
").TestStackClear();


            (new TestBox("MultiCall")).Run(@"
func mul( a, b ){
    return a * b
}

func foo( a, b ){


    return a + mul( b, 2 )
}

var y = foo( 1, 2 )
").TestStackClear().TestGlobalRegEqual(2, 5);

            (new TestBox("IfStatement")).Run(@"
var x = 1
var y
if x >= 1 {
    var c = 5
    y = c
}else{
    y = 2
}
").TestStackClear().TestGlobalRegEqual(0, 1).TestGlobalRegEqual(1, 5);

            (new TestBox("SwapVar")).Run(@"
var x, y = 1, 2
x, y = y, x
").TestStackClear().TestGlobalRegEqual(0, 2).TestGlobalRegEqual(1, 1);


            (new TestBox("WhileLoop")).Run(@"
var x = 1

while x < 3 {
    x = x + 1
}
").TestStackClear().TestGlobalRegEqual(0, 3);

            (new TestBox("ForLoop")).Run(@"
var x = 10
for i = 1;i < 3;i=i+1 {
    x = x - 1
}
").TestStackClear().TestGlobalRegEqual(0, 8).TestLocalRegEqual(0, 3);


        }
    }
}
