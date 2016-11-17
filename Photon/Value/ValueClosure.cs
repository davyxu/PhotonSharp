using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    public class ValueClosure : ValueFunc
    {
        List<Slot> _upvalues = new List<Slot>();

        public ValueClosure(ValueFunc f )
            : base( f )
        {
            
        }

        internal void AddUpValue( Slot v )
        {
            _upvalues.Add(v);
        }

        public void SetUpValue(int index, Value v )
        {
            _upvalues[index].SetData( v );
        }

        public Value GetUpValue(int index )
        {
            return _upvalues[index].Data;
        }

        public override string ToString()
        {
            return string.Format("{0} (closure) {1}", _index.ToString(), _pos);
        }
    }

 
}
