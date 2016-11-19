using System.Diagnostics;
using System;
using System.IO;
using Photon;

namespace UnitTest
{
    class TestBox
    {
        Executable _exe;
        VMachine _vm = new VMachine();

        string _caseName;        

        public TestBox Compile( string caseName, string src )
        {
            _caseName = caseName;            

            Debug.WriteLine(string.Format("==================={0}===================", _caseName));

            _exe = Compiler.Compile(new SourceFile(src));            

            _exe.DebugPrint();            

            return this;
        }

        public TestBox Run( )
        {
            Debug.WriteLine(string.Format(">>>>>>>>>Start {0}", _caseName));
            _vm.ShowDebugInfo = true;

            _vm.Run(_exe);

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

            var content = System.IO.File.ReadAllText(filename);

            return Compile(filename, content);
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
            return TestRegEqualNumber(index, num, _vm.GetRuntimePackage(0).Reg);
        }

        TestBox TestRegEqualNumber(int index, float num, Register reg )
        {
            var v = reg.Get( index );

            if ( v == null )
            {
                Error(string.Format("value nil on index: {0}, expect {1}", index, num));
                return this;
            }

            var nv = v as ValueNumber;
            if (nv == null)
            {
                Error(string.Format("value type error on index: {0}, expect 'number', have '{1}'", index, v.GetType().Name));
                return this;
            }

            if (nv.Number != num)
            {
                Error(string.Format("value not equal on index: {0}, expect {1}", index, num));
                return this;
            }

            return this;
        }
    }
}
