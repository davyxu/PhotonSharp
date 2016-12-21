using Photon;

namespace UnitTest
{
    public class DelegateTest
    {        
        [NativeEntry(NativeEntryType.StaticFunc, "add")]
        public static int AddValue(VMachine vm)
        {
            var a = vm.DataStack.GetInteger32(0);
            var b = vm.DataStack.GetInteger32(1);

            vm.DataStack.PushInteger32(a + b);

            return 1;
        }
    }


    partial class Program
    {
        static void TestDelegateExecute()
        {
            new TestBox().RegisterRunFile(delegate(Executable exe)
            {

                exe.RegisterNativeClass(typeof(DelegateTest), "DelegateTest");

            }, "Delegate.pho")
                .CheckGlobalVarMatchValue("a", 3);         


            {
                var testbox = new TestBox();
                testbox.CompileFile("Execute.pho");
                testbox.VM.ShowDebugInfo = true;

                var retValue = testbox.VM.Execute(testbox.Exe, "main", "foo", new object[] { 1, 2 }, 2);

                if ( retValue.Length != 2 )
                {
                    testbox.Error("ret value not match");
                }

                if ( (System.Int32)retValue[0] != 3 )
                {
                    testbox.Error("ret value not match");
                }

                if (!retValue[1].GetType().IsClass)
                {
                    testbox.Error("ret value not match");
                }
            }
        }
    }
}
