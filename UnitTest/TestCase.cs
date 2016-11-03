
namespace UnitTest
{
    partial class Program
    {
        static void TestCase( )
        {
            (new TestBox("func var assign")).Run(@"

func add( a, b ){
    return a+b
}

add(1, 2)
").TestStackClear();


            (new TestBox("multicall")).Run(@"
func mul( a, b ){
    return a * b
}

func foo( a, b ){


    return a + mul( b, 2 )
}

var y = foo( 1, 2 )
").TestStackClear().TestRegEqual(2, 5);

            (new TestBox("if")).Run(@"
var x = 1
var y
if x >= 1 {
    var c = 5
    y = c
}else{
    y = 2
}
").TestStackClear().TestRegEqual(0, 1).TestRegEqual(1, 5);

            (new TestBox("swap var")).Run(@"
var x, y = 1, 2
x, y = y, x
").TestStackClear().TestRegEqual(0, 2).TestRegEqual(1, 1);


            (new TestBox("simple loop")).Run(@"
var x = 1

while x < 3 {
    x = x + 1
}
").TestStackClear().TestRegEqual(0, 3);

            (new TestBox("for loop")).Run(@"
var x = 10
for i = 1;i < 3;i=i+1 {
    x = x - 1
}
").TestStackClear().TestRegEqual(0, 8).TestRegEqual(1, 3);


        }
    }
}
