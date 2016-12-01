using System.Collections.Generic;

namespace Photon
{
    class RuntimeUpValue
    {
        public Register Reg;
        public int Index;
    }

    public class ValueClosure : ValueFunc
    {
        List<RuntimeUpValue> _upvalues = new List<RuntimeUpValue>();

        ValueFunc _func;
        internal ValueClosure( ValueFunc func )
            : base( func.Name )
        {
            _func = func;
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

        internal override bool Invoke(VMachine vm, int argCount, bool balanceStack, ValueClosure closure)
        {
            return _func.Invoke(vm, argCount, balanceStack, closure );
        }

        public override string DebugString()
        {
            return string.Format("{0} (closure)", _func.ToString());
        }


    }

 
}
