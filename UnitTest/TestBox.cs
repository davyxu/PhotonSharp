using Photon.API;
using Photon.VM;
using Photon.Model;
using System.Diagnostics;
using System;
using System.IO;

namespace UnitTest
{
    class TestBox
    {
        Script _script = new Script();

        string _caseName;        

        public TestBox Compile( string caseName, string src )
        {
            _caseName = caseName;

            _script.DebugMode = true;

            Debug.WriteLine(string.Format("==================={0}===================", _caseName));

            _script.Compile(src);            

            return this;
        }

        public TestBox Run( )
        {
            _script.Run();

            return this;
        }

        public Script Script
        {
            get { return _script; }
        }

        public TestBox CompileFile(string filename)
        {

            var content = File.ReadAllText(filename);

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
            if (_script.VM.Stack.Count != 0)
            {
                Error("Stack not clear");
            }

            return this;
        }        


        public TestBox TestLocalRegEqualNumber(int index, float num)
        {
            return TestRegEqualNumber(index, num, _script.VM.LocalRegister);
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
