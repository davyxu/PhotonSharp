using System.Collections.Generic;

namespace Photon
{
    class RuntimeUpValue
    {
        public Register Reg;
        public int Index;
    }

    class ValueClosure : ValueFunc
    {
        List<RuntimeUpValue> _upvalues = new List<RuntimeUpValue>();

        internal ValueClosure(Procedure proc)
            : base(proc)
        {
            
        }

        internal void AddUpValue( Register reg, int index )
        {
            RuntimeUpValue ru = new RuntimeUpValue() ;
            ru.Reg = reg;
            ru.Index = index;

            _upvalues.Add(ru);
        }

        internal void SetValue(int index, Value v)
        {
            var ru = _upvalues[index];
            ru.Reg.Set(ru.Index, v);
        }

        internal Value GetValue(int index)
        {
            var ru = _upvalues[index];
            return ru.Reg.Get(ru.Index);
        }

        internal RuntimeUpValue GetUpValue( int index )
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
