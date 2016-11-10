using SharpLexer;
using System.Collections.Generic;

namespace Photon.Model
{
    public class ValueClosure : ValueFunc
    {        
        List<Value> _upvalues = new List<Value>();

        public ValueClosure(ValueFunc f )
            : base( f )
        {
            
        }

        public void AddUpValue( Value v )
        {
            _upvalues.Add(v);
        }

        public void SetUpValue(int index, Value v )
        {
            _upvalues[index] = v;
        }

        public Value GetUpValue(int index )
        {
            return _upvalues[index];
        }

        public override string ToString()
        {
            return string.Format("{0} (closure) {1}", _index.ToString(), _pos);
        }
    }

 
}
