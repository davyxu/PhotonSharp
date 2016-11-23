using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    class ValueClosure : ValueFunc
    {
        List<Slot> _upvalues = new List<Slot>();

        internal ValueClosure(Procedure proc)
            : base(proc)
        {
            
        }

        internal void AddUpValue( Slot v )
        {
            _upvalues.Add(v);
        }

        internal void SetUpValue(int index, Value v)
        {
            _upvalues[index].SetData( v );
        }

        internal Value GetUpValue(int index)
        {
            return _upvalues[index].Data;
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
