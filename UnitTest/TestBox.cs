using Photon.API;
using Photon.VM;
using Photon.OpCode;
using System.Diagnostics;

namespace UnitTest
{
    class TestBox
    {
        Script _script = new Script();

        string _caseName;

        public TestBox( string caseName )
        {
            _caseName = caseName;
        }

        public TestBox Run( string src )
        {
            _script.DebugMode = true;

            Debug.WriteLine(string.Format("==================={0}===================", _caseName));

            _script.Run(_script.Compile(src));

            return this;
        }

        public void Error( string info )
        {
            Debug.WriteLine("[{0}] failed, {1}", _caseName, info);
        }

        public TestBox TestStackClear()
        {
            if (_script.VM.Stack.Count != 0)
            {
                Error("Stack not clear");
            }

            return this;
        }

        public TestBox TestRegEqual(int index, float num)
        {
            var v = _script.VM.Reg.Get( index );

            if ( v == null )
            {
                Error(string.Format("value nil on index: {0}, expect {1}", index, num));
                return this;
            }

            var nv = v as NumberValue;
            if (nv == null)
            {
                Error(string.Format("value type error on index: {0}, expect 'number'", index));
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
