using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    class RuntimeUpvalue
    {
        public Register Reg;
        public int Index;
    }

    class ValueClosure : ValueFunc
    {
        List<RuntimeUpvalue> _upvalues = new List<RuntimeUpvalue>();

        internal ValueClosure(Procedure proc)
            : base(proc)
        {
            
        }

        internal void AddUpValue( Register reg, int index )
        {
            RuntimeUpvalue ru = new RuntimeUpvalue() ;
            ru.Reg = reg;
            ru.Index = index;

            _upvalues.Add(ru);
        }

        internal void SetUpValue(int index, Value v)
        {
            var ru = _upvalues[index];
            ru.Reg.Set(ru.Index, v);
        }

        internal Value GetUpValue(int index)
        {
            var ru = _upvalues[index];
            return ru.Reg.Get(ru.Index);
        }

        internal RuntimeUpvalue GetRuntimeUpValue( int index )
        {
            return _upvalues[index];
        }

        public override string DebugString()
        {
            return string.Format("{0} (closure)", _proc.ToString());
        }

        public override string ToString()
        {
            return DebugString();
        }
    }

 
}
