
using System.Diagnostics;

namespace Photon
{
    public class Register : DataAccessor
    {
        Value[] _values;
        int _usedSlot = 0;
        string _usage;
        Scope _scope;

        internal Register( string usage, int maxReg )
        {
            _usage = usage;
            _values = new Value[maxReg];
            Clear();
        }

        public int Count
        {
            get { return _usedSlot; }
        }

        internal void AttachScope(Scope s)
        {
            _scope = s;
        }

        internal void SetUsedCount( int count )
        {            
            _usedSlot = count;
        }

        internal override void Set( int index, Value v )
        {            
            _values[index] = v;
        }

        internal override Value Get(int index)
        {
            return _values[index];
        }

        internal void Clear()
        {
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = Value.Nil;
            }

            _usedSlot = 0;
        }

        public override string ToString()
        {
            return string.Format("{0} used:{1}", _usage, _usedSlot);
        }

        public override string DebugString(int index)
        {
            var v = Get(index);

            var symbol = _scope.FindRegisterByIndex(index);


            return string.Format("{0}{1} ({2}): {3}", _usage, index, symbol != null ? symbol.Name:"#NA", v.ToString());
        }

        public void DebugPrint( )
        {   
            if ( _scope == null )
            {
                for (int i = 0; i < _usedSlot; i++)
                {
                    var v = _values[i];

                    Logger.DebugLine("{0}{1}: {2}", _usage, i, v.ToString());
                }
            }
            else
            {
                for (int i = 0; i < _usedSlot; i++)
                {

                    Logger.DebugLine(DebugString(i));
                }
            }



        }
    }
}
