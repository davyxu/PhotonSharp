using System.Diagnostics;
using System;
using Photon;

namespace UnitTest
{
    class TestBox
    {
        Executable _exe = new Executable();
        VMachine _vm = new VMachine();

        string _caseName;        

        public TestBox Compile( string caseName, string src )
        {
            _caseName = caseName;

            Debug.WriteLine(string.Format("################### {0} ###################", _caseName));

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

            var G = _vm.GetRuntimePackageByName("main").Reg;
            
            G.DebugPrint();
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
            return CompileFile(filename ).Run().CheckStackClear();
        }

        public void Error( string info )
        {
            Debug.WriteLine("[{0}] failed, {1}", _caseName, info);
            throw new Exception(info);
        }

        public TestBox CheckStackClear()
        {
            if (_vm.DataStack.Count != 0)
            {
                Error("Stack not clear");
            }

            return this;
        }        


        public TestBox CheckLocalRegEqualNumber(int index, float num)
        {
            return CheckRegEqualNumber(index, num, _vm.LocalReg);
        }

        public TestBox CheckGlobalRegEqualNumber(int index, float num)
        {
            return CheckRegEqualNumber(index, num, _vm.GetRuntimePackageByName("main").Reg);
        }

        public TestBox CheckGlobalRegEqualNil(int index)
        {
            return CheckRegEqualNil(index, _vm.GetRuntimePackageByName("main").Reg);
        }

        public TestBox CheckGlobalRegEqualString(int index, string s)
        {
            return TestRegEqualString(index, s, _vm.GetRuntimePackageByName("main").Reg);
        }

        TestBox CheckRegEqualNil(int index,  Register reg)
        {
            var v = reg.IsNil(index);

            if (!v)
            {
                Error(string.Format("value not equal on index: {0}, expect nil", index));
                return this;
            }

            return this;
        }

        TestBox CheckRegEqualNumber(int index, float num, Register reg )
        {
            var v = reg.GetFloat32( index );            

            if (v != num)
            {
                Error(string.Format("value not equal on index: {0}, expect {1}", index, num));
                return this;
            }

            return this;
        }

        TestBox TestRegEqualString(int index, string str, Register reg)
        {
            var v = reg.GetString(index);

            if (v != str)
            {
                Error(string.Format("value not equal on index: {0}, expect {1}", index, str));
                return this;
            }

            return this;
        }
    }
}
