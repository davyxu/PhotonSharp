using System.Diagnostics;
using System;
using Photon;

namespace PhotonCompiler
{
    class TestBox
    {
        Executable _exe = new Executable();
        VMachine _vm = new VMachine();

        string _caseName;        

        public TestBox Compile( string caseName, string src )
        {
            _caseName = caseName;            

            Debug.WriteLine(string.Format("==================={0}===================", _caseName));

            Compiler.Compile(_exe, src);            

            _exe.DebugPrint();            

            return this;
        }

        public TestBox Run( )
        {
            Debug.WriteLine(string.Format(">>>>>>>>>Start {0}", _caseName));
            _vm.ShowDebugInfo = true;

            _vm.Run(_exe );

            Debug.WriteLine(string.Format(">>>>>>>>>End {0}", _caseName));
            _vm.DataStack.DebugPrint();            

            _vm.LocalReg.DebugPrint();

            _vm.GetRuntimePackage(0).Reg.DebugPrint();
            return this;
        }

        public Executable Exe
        {
            get { return _exe; }
        }

        public TestBox CompileFile(string filename)
        {
            return Compile(filename, filename);
        }



        public TestBox RunFile( string filename )
        {                        
            return CompileFile(filename ).Run().TestStackClear();
        }

        public void Error( string info )
        {
            Debug.WriteLine("[{0}] failed, {1}", _caseName, info);
            throw new Exception(info);
        }

        public TestBox TestStackClear()
        {
            if (_vm.DataStack.Count != 0)
            {
                Error("Stack not clear");
            }

            return this;
        }        


        public TestBox TestLocalRegEqualNumber(int index, float num)
        {
            return TestRegEqualNumber(index, num, _vm.LocalReg);
        }

        public TestBox TestGlobalRegEqualNumber(int index, float num)
        {
            return TestRegEqualNumber(index, num, _vm.GetRuntimePackageByName("main").Reg);
        }

        TestBox TestRegEqualNumber(int index, float num, Register reg )
        {
            var v = reg.GetFloat32( index );            

            if (v != num)
            {
                Error(string.Format("value not equal on index: {0}, expect {1}", index, num));
                return this;
            }

            return this;
        }
    }
}
